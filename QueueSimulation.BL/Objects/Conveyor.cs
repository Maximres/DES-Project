using QueueSimulation.BL.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class Conveyor<T> : Container<T> where T : Product
    {
        

        

        /// <summary>
        /// Определяет, можно ли передать продукт с конвейера
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли передать продукт с конвейера")]
        public abstract bool CanThrowProduct { get;}

        /// <summary>
        /// Порт выхода объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Порт выхода объекта")]
        public abstract Machine<T> PortOut { get; set; }

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


    }
}
