using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QueueSimulation.BL.Objects
{
    /// <summary>
    /// Станок
    /// </summary>
    public abstract class Machine<T> where T : Product
    {

        /// <summary>
        /// Порт входа объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Название продукта продукта")]
        public abstract Conveyor<T> PortIn { get; set; }

        /// <summary>
        /// Порт выхода объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Название продукта продукта")]
        public abstract Container<T> PortOut { get; set; }

        /// <summary>
        /// Задает значение задержки производства продукции (в секундах).
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Эмуляция работы станка. Задержка перед передачей объекта")]
        public abstract double Delay { get; set; }

        /// <summary>
        /// Получает или задает вместительность станка.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Вместительность станка")]
        public abstract long Capacity { get; set; }

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
        public abstract float CrashChance { get; set; }
    }
}
