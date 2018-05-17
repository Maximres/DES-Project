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
        bool _justCrashed = false;

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
        [Range(0.0,1.0)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Шанс поломки (деактивности)")]
        public abstract double CrashChance { get; set; }

        /// <summary>
        /// Получает или задает среднеквадратичное отклонение.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Среднеквадратичное отклонение.")]
        public abstract int CrashRatePerProduct { get; set; }

        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };
        public event EventHandler OnEmpty = delegate { };


        public void Dequeue(object sender, ProductEngagedEventArgs<T> e)
        {
            throw new NotImplementedException();
        }

        public void Enqueue(object sender, ProductEngagedEventArgs<T> e)
        {
            throw new NotImplementedException();
        }

        public void Simulate()
        {
            throw new NotImplementedException();
        }

        public override void AddNode(IDequeueable<T> node)
        {
            PortIn = node;
            node.OnDequeue += Enqueue;
        }
        
        protected bool CanThrow()
        {
            //TODO: алгоритм работы станка с учетом поломки
            var crashed = _exponential.Sample();
            if (crashed < CrashChance)
            {
                _justCrashed = true;
                
            }
            return true;
        }
    }
}
