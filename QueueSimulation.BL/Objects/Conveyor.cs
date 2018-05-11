using QueueSimulation.BL.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class Conveyor<T> where T : Product
    {
        /// <summary>
        /// Порт входа объекта
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Порт входа объекта")]
        public abstract Machine<T> PortIn { get; set; }

        /// <summary>
        /// Определяет, можно ли добавить продукт на конвейер
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
        /// Порт выхода объекта
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Порт выхода объекта")]
        public abstract Machine<T> PortOut { get; set; }

        /// <summary>
        /// Длина конвейера
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Длина конвейера")]
        public abstract long Length { get; set; }

        /// <summary>
        /// Расстояние между объектами конвейера
        /// </summary>
        //public abstract int Space { get; set; }

        /// <summary>
        /// Вместительность конвейера
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Вместительность конвейера")]
        public abstract int Capacity { get; set; }

        /// <summary>
        /// Задержка перед передачей объекта
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Задержка перед передачей объекта")]
        public abstract int Delay { get; set; }

        /// <summary>
        /// Скорость 
        /// </summary>
        /// 
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Скорость")]
        public abstract long Speed { get; set; }


    }
}
