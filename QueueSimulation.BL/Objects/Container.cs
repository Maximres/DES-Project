using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class Container<T> where T : Product
    {
        /// <summary>
        /// Определяет, можно ли добавить продукт на конвейер.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, можно ли добавить продуки на конвейер")]
        public abstract bool CanTakeProduct { get; }

        

        /// <summary>
        /// Порт входа объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Порт входа объекта")]
        public abstract Machine<T> PortIn { get; set; }

        /// <summary>
        /// Определяет или задает вместительность конвейера.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Вместительность конвейера")]
        public abstract int Capacity { get; set; }
    }
}
