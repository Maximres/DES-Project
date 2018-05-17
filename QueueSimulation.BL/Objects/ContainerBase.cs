using QueueSimulation.BL.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class ContainerBase<T> where T : ProductBase
    {
        /// <summary>
        /// Добавляет вершину к объекту.
        /// </summary>
        /// <param name="node"></param>
        public abstract void AddNode(IDequeueable<T> node);

        /// <summary>
        /// Порт входа объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Порт входа объекта")]
        public abstract IDequeueable<T> PortIn { get; set; }

        /// <summary>
        /// Порт выхода объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Порт выхода объекта")]
        public abstract IDequeueable<T> PortOut { get; set; }

        /// <summary>
        /// Определяет или задает вместительность конвейера.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Вместительность конвейера")]
        public abstract int Capacity { get; set; }
    }
}
