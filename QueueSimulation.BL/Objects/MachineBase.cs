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
    public abstract class MachineBase<T> : ContainerBase<T>, IMachine<T> where T : ProductBase
    {
        /// <summary>
        /// Crash rate.
        /// </summary>
        protected Exponential _exponential;
        static object locker = new object();
        bool _justCrashed = false;
        TimeSpan TimeStampCrash;
        protected DateTime Past { get; private set; }
        TimeSpan span;
        protected Queue<T> _productsQueue { get; set; }
        private bool IsEmpty()
        {
            return _productsQueue == null || _productsQueue.Count() <= 0;
        }

        public MachineBase()
        {
            Past = new DateTime();
            _productsQueue = new Queue<T>();
        }

        /// <summary>
        /// Определяет, можно ли добавить продукт на конвейер.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли добавить продуки на станок")]
        public abstract bool CanTakeProduct { get; }


        /// <summary>
        /// Определяет, можно ли передать продукт с конвейера
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли передать продукт с конвейера")]
        public abstract bool CanThrowProduct { get; }

        ///// <summary>
        ///// Порт выхода объекта.
        ///// </summary>
        //[Browsable(true)]
        //[ReadOnly(true)]
        //[Description("Название продукта продукта")]
        //public abstract ContainerBase<T> PortOut { get; set; }

        /// <summary>
        /// Задает значение задержки производства продукции (в секундах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Эмуляция работы станка. Задержка перед передачей объекта")]
        public abstract double Delay { get; set; }

        /// <summary>
        /// Определяет, сломан (неактивен) ли станок.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, сломан (неактивен) ли станок")]
        public abstract bool IsBroken { get; }

        /// <summary>
        /// Получает или задает время бездействия во время поломки станка.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Время бездействия во время поломки")]
        public abstract double InactiveTime { get; set; }

        /// <summary>
        /// Получает или задает шанс поломки (деактивности) станка.
        /// </summary>
        [Range(0.0, 1.0)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Шанс поломки (деактивности)")]
        public abstract double CrashChance { get; set; }

        /// <summary>
        /// Получает или задает среднеквадратичное отклонение.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Среднеквадратичное отклонение")]
        public abstract int CrashRatePerProduct { get; set; }
        public IDequeueable<T> PortIn { get; set; }
        public IDequeueable<T> PorOut { get; set; }

        public int Count => _productsQueue.Count();

        public string Name { get ; set; }

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
                    _productsQueue.Enqueue(e.Product);
                }
                this.OnEnqueue(this, new ProductEngagedEventArgs<T>(e.Product));
            }
        }

        public void Simulate()
        {
            if (CanThrowProduct)
            {
                Dequeue(this, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
            }
        }

        //public override void JoinWithPrevious(IDequeueable<T> node)
        //{
        //    if (PortIn != null)
        //    {
        //        RemoveNode(node);
        //    }
        //    node.PorOut = this;
        //    PortIn = node;
        //    node.OnDequeue += Enqueue;
        //}

        //public override void RemoveNode(IDequeueable<T> node)
        //{
        //    PortIn = null;
        //    node.OnDequeue -= Enqueue;
        //    node.PorOut = null;
        //}

        protected bool NotBroken()
        {
            //TODO: алгоритм работы станка с учетом поломки
            var crashed = _exponential.Sample();
            if (crashed > CrashChance)
            {
                if (_justCrashed == false)
                {
                    _justCrashed = true;
                    TimeStampCrash = new TimeSpan(0,0, (int)InactiveTime);
                    return false;
                }
                else
                {
                    if ((DateTime.Now - TimeStampCrash).Second > (int)InactiveTime)
                    {
                        _justCrashed = false;
                    }
                }

            }
            return true;
        }

        protected bool CanThrow()
        {
            span = DateTime.Now - Past;
            
            if (!IsEmpty() && span.Seconds > Delay)
            {
                Past = DateTime.Now;
                return true;
            }

            return false;
        }

        public void JoinWithPrevious(IDequeueable<T> node)
        {
            node.PorOut = this;
            PortIn = node;
            node.OnDequeue += Enqueue;
        }
    }
}
