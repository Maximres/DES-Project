using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    [Serializable]
    public class Conveyor<T> : ContainerBase<T>, ISimulation<T> where T : ProductBase
    {
        static readonly object locker = new object();

        protected Queue<T> _productsQueue { get; set; }
        protected DateTime Past { get; private set; }
        TimeSpan span;
        DateTime spanAfterDequeue;
        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };
        public event EventHandler OnEmpty = delegate { };


        public Conveyor()
        {
            Past = new DateTime();
            _productsQueue = new Queue<T>();
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
        /// Определяет, не пуст ли конвейер.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, пуст ли объект")]
        public bool IsEmpty { get; }

        /// <summary>
        /// Определяет или задает длину конвейера (в метрах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Длина конвейера")]
        [Range(0, 10000)]
        public double Length { get; set; } = 100;

        /// <summary>
        /// Определяет или задает задержка перед передачей объекта (в секундах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Задержка перед передачей объекта")]
        [Range(0, 1000)]
        public int Delay { get; set; } = 3;

        /// <summary>
        /// Определяет или задает скорость движения объектов по конвейеру (в метрах на секунду).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Скорость")]
        [Range(0, 10000)]
        public double Speed { get; set; } = 5;

        [Browsable(false)]
        public IDequeueable<T> PortIn { get; set; }

        [Browsable(false)]
        public IDequeueable<T> PorOut { get; set; }

        public int Count => _productsQueue.Count();

        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Название объекта")]
        public string Name { get; set; } = "Matvey";

        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Размерность конвейера")]
        public override int Capacity { get; set; } = 1000;

        [Browsable(false)]
        [Description("Идентификатор")]
        public override int Id { get; set; } = 7;

        public override bool CanTakeProduct => _productsQueue.Count() < Capacity;

        public void Dequeue(object sender, ProductEngagedEventArgs<T> e)
        {
        }

        public void Enqueue(object sender, ProductEngagedEventArgs<T> e)
        {
            if (_productsQueue == null)
            {
                _productsQueue = new Queue<T>();
            }

            if (IsEmpty)
            {
                OnEmpty(this, EventArgs.Empty);
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
                    //OnEnqueue(this, new ProductEngagedEventArgs<T>(e.Product));
                }
            }
        }

        public void Simulate()
        {
            if (PorOut is ContainerBase<ProductBase> container && container.CanTakeProduct)
            {
                if (CanThrow())
                {
                    spanAfterDequeue = DateTime.Now;
                    OnDequeue(this, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
                    //Past = DateTime.Now;
                }
            }
        }

        private int GetTime()
        {
            var t = this.Length / Speed;
            return (int)t;
        }

        protected bool CanThrow()
        {
            span = DateTime.Now - Past;
            if (false == Empty() && span.Seconds > (Delay + GetTime()))
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
        private bool Empty()
        {
            return _productsQueue == null || _productsQueue.Count() <= 0;
        }

        public void Reset()
        {
            this._productsQueue = new Queue<T>();
        }
    }
}
