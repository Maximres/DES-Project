using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using MathNet.Numerics.Distributions;

namespace QueueSimulation.BL.Objects
{
    /// <summary>
    /// Станок
    /// </summary>
    [Serializable]
    public class Machine<T> : ContainerBase<T>, IMachine<T> where T : ProductBase
    {
        /// <summary>
        /// Crash rate.
        /// </summary>
        //protected Exponential _exponential;
        static readonly object locker = new object();
        private static readonly Random random1to100 = new Random();
        protected DateTime Past { get; private set; }
        TimeSpan span;
        DateTime spanAfterDequeue;
        protected Queue<T> _productsQueue { get; set; }
        private bool IsEmpty()
        {
            return _productsQueue == null || _productsQueue.Count() <= 0;
        }

        public Machine()
        {
            Past = new DateTime();
            _productsQueue = new Queue<T>();
            //_exponential = new Exponential(1000);
            spanAfterDequeue = DateTime.Now;
        }

        /// <summary>
        /// Определяет, можно ли передать продукт с конвейера
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли передать продукт с конвейера")]
        public bool CanThrowProduct { get; }

        /// <summary>
        /// Задает значение задержки производства продукции (в секундах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Задержка перед передачей объекта")]
        [Range(0, 10000)]
        public double Delay { get; set; }

        /// <summary>
        /// Определяет, сломан (неактивен) ли станок.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, сломан (неактивен) ли станок")]
        public bool IsBroken { get; set; } = false;

        /// <summary>
        /// Получает или задает время бездействия во время поломки станка.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Время бездействия во время поломки")]
        public double InactiveTime { get; set; }

        /// <summary>
        /// Получает или задает шанс поломки (деактивности) станка.
        /// </summary>
        [Range(0.0, 1.0)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Шанс поломки (деактивности)")]
        public double CrashChance { get; set; }

        /// <summary>
        /// Получает или задает среднеквадратичное отклонение.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Range(0.0, 1000)]
        [Description("Среднеквадратичное отклонение")]
        public int CrashRatePerProduct { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Входной станок/конвейер")]
        public IDequeueable<T> PortIn { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Выходной станок/конвейер")]
        public IDequeueable<T> PorOut { get; set; }

        public int Count => _productsQueue.Count();

        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Определяет имя объекта")]
        [RegularExpression(@"^[A-Za-z0-9]+$")]
        public string Name { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет размерность объекта")]
        public override int Capacity { get; set; } = 1000;

        [Browsable(false)]
        [ReadOnly(true)]
        [Description("Идентификатор объекта")]
        public override int Id { get; set; } = 3;

        [Browsable(false)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли взять продукт с конвейера")]
        public override bool CanTakeProduct => _productsQueue.Count() < Capacity;

        //public IDequeueable<T> PorOut { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };
        public event EventHandler OnEmpty = delegate { };


        public void Dequeue(object sender, ProductEngagedEventArgs<T> e)
        {
            if (PorOut == null)
            {
                throw new ArgumentNullException(nameof(PorOut));
            }

            OnDequeue(sender, new ProductEngagedEventArgs<T>(e.Product));
        }

        public void Enqueue(object sender, ProductEngagedEventArgs<T> e)
        {

            if (_productsQueue == null)
            {
                _productsQueue = new Queue<T>();
            }

            if (CanTakeProduct)
            {
                lock (locker)
                {
                    if (spanAfterDequeue.Second > Delay)
                    {
                        Past = DateTime.Now;
                    }
                    _productsQueue.Enqueue(e.Product);
                }
                this.OnEnqueue(this, new ProductEngagedEventArgs<T>(e.Product));
            }
        }

        public void Simulate()
        {
            if (PorOut is ContainerBase<ProductBase> container && container.CanTakeProduct)
            {
                if (CanThrow() && NotBroken())
                {
                    //Dequeue(this, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
                    spanAfterDequeue = DateTime.Now;
                    OnDequeue(this, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
                }
            }


        }

        protected bool NotBroken()
        {
            //TODO: алгоритм работы станка с учетом поломки
            //if (IsBroken && CanThrow())
            //{
            //    IsBroken = false;
            //    return true;
            //}
            var chance = CrashChance * 100;
            if (random1to100.Next(1,101) <= chance)
            {
                Past.AddSeconds(InactiveTime);
                IsBroken = true;
                return false;
            }
            IsBroken = false;
            return true;
        }

        protected bool CanThrow()
        {
            span = DateTime.Now - Past;

            if (!IsEmpty() && span.Seconds > Delay)
            {
                return true;
            }

            return false;
        }

        public void JoinPrevious(IDequeueable<T> node)
        {
            node.PorOut = this;
            PortIn = node;
            node.OnDequeue += Enqueue;
        }

        public void Reset()
        {
            _productsQueue = new Queue<T>();
        }
    }
}
