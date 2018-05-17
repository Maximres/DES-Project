using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface ICustomQueue<T> where T : ProductBase
    {
        /// <summary>
        /// Количество элементов.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Добавить элемент в очередь.
        /// </summary>
        /// <param name="item"> Добавляемый объект </param>
        void Enqueue(T item);

        /// <summary>
        /// Получить объект из очереди с удалением.
        /// </summary>
        /// <returns> Элемент данных. </returns>
        T Dequeue();

        /// <summary>
        /// Прочитать элемент из очереди без удаления.
        /// </summary>
        /// <returns> Элемент данных. </returns>
        T Peek();
    }
}
