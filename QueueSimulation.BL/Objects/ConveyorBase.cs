﻿using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class ConveyorBase<T> : ContainerBase<T>, ISimulation<T> where T : ProductBase
    {
        static object locker = new object();

        protected Queue<T> _productsQueue { get; set; }
        protected DateTime Past { get; private set; }
        TimeSpan span;
        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };
        public event EventHandler OnEmpty = delegate { };


        //public ConveyorBase(ConveyorBase<T> conveyor):this()
        //{
        //    this.PortIn = conveyor;
        //    conveyor.OnDequeue += Enqueue;
        //    //this.OnDequeue += Dequeue;
        //}

        //public ConveyorBase(MachineBase<T> machine):this()
        //{
        //    this.PortIn = machine;
        //    machine.OnDequeue += Enqueue;
        //    //this.OnDequeue += Dequeue;
        //}

        //public ConveyorBase():this()
        //{

        //}

        public ConveyorBase()
        {
            Past = DateTime.Now;
        }

        public override void AddNode(IDequeueable<T> node)
        {
            PortIn = node;
            node.OnDequeue += Enqueue;
        }

        /// <summary>
        /// Определяет, можно ли добавить продукт на конвейер.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли добавить продуки на конвейер")]
        public abstract bool CanTakeProduct { get; }

        /// <summary>
        /// Определяет, можно ли передать продукт с конвейера
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли передать продукт с конвейера")]
        public abstract bool CanThrowProduct { get;}

        /// <summary>
        /// Определяет, не пуст ли конвейер.
        /// </summary>
        public abstract bool IsEmpty { get; }

        

        /// <summary>
        /// Определяет или задает длину конвейера (в метрах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Длина конвейера")]
        public abstract double Length { get; set; }

        /// <summary>
        /// Расстояние между объектами конвейера
        /// </summary>
        //public abstract int Space { get; set; }

        /// <summary>
        /// Определяет или задает задержка перед передачей объекта (в секундах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Задержка перед передачей объекта")]
        public abstract int Delay { get; set; }

        /// <summary>
        /// Определяет или задает скорость движения объектов по конвейеру (в метрах на секунду).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Скорость")]
        public abstract double Speed { get; set; }


        public void Dequeue(object sender, ProductEngagedEventArgs<T> e)
        {
            if (PortIn == null)
            {
                throw new ArgumentNullException(nameof(PortIn));
            }


            if (CanThrowProduct)
            {
                lock (locker)
                {
                    OnDequeue(sender, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
                }
            }

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
                    _productsQueue.Enqueue(e.Product);
                    OnEnqueue(sender, e);
                }
            }
        }

        public void Simulate()
        {
            TimeSpan span = DateTime.Now - Past;
            if (PortIn != null && CanThrowProduct)
            {
                Dequeue(this, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
                Past = DateTime.Now;
            }
        }

        private int GetTime()
        {
            var t = this.Length / Speed / _productsQueue.Count;
            return (int)t;
        }

        protected bool CanThrow()
        {
            span = DateTime.Now - Past;
            if (span.Seconds >= (Delay + GetTime()))
            {
                Past = DateTime.Now;
                return true;
            }
            return false;
        }
    }
}