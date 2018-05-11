using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class OwnQueueBase<T> where T : Product
    {
        /// <summary>
        /// Количество элементов.
        /// </summary>
        public abstract int Count { get; }

        /// <summary>
        /// Добавить элемент в очередь.
        /// </summary>
        /// <param name="item"> Добавляемые данные. </param>
        public abstract void Enqueue(T item);

        /// <summary>
        /// Получить элемент из очереди с удалением.
        /// </summary>
        /// <returns> Элемент данных. </returns>
        public abstract T Dequeue();

        /// <summary>
        /// Прочитать элемент из очереди без удаления.
        /// </summary>
        /// <returns> Элемент данных. </returns>
        public abstract T Peek();

    }
}
