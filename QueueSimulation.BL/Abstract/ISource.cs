using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface ISource<T> where T : Product
    {
        /// <summary>
        /// Высвобождает первоый объектв в очереди.
        /// </summary>
        /// <returns></returns>
        T Dequeue();

        /// <summary>
        /// Обновляет источник.
        /// </summary>
        void Reset();

        /// <summary>
        /// Происходит, когда источник высвобождает объект.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnDequeue;

        /// <summary>
        /// Происходит, когда в источнике заканчиваются объекты
        /// </summary>
        event EventHandler OnEmpty;

        /// <summary>
        /// Эмуляция работы источника
        /// </summary>
        void EmulateSource();
    }
}
