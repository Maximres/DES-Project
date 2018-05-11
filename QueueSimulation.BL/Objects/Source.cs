using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class Source<T> where T : Product
    {
        /// <summary>
        /// Очередь для хранения объектов продукции.
        /// </summary>
        protected Queue<T> _products { get; set; }

        /// <summary>
        /// Инициализирует очередь.
        /// </summary>
        /// <param name="prodCount"></param>
        public Source(int prodCount)
        {
            _products = new Queue<T>(prodCount);
        }

        /// <summary>
        /// Порт выхода объектов.
        /// </summary>
        /// <remarks>1 каждую секунду</remarks>
        public abstract Conveyor<T> PortOut { get; set; }

        /// <summary>
        /// Определяет или задает скорость поступления объектов из источника.
        /// </summary>
        public abstract double ArrivalRate { get; set; }

        /// <summary>
        /// Определяет, не пуст ли источник.
        /// </summary>
        public abstract bool IsEmpty { get; }

        /// <summary>
        /// Определяет или задает максимальное количество объектов в очереди.
        /// </summary>
        public abstract int Capacity { get; }

        /// <summary>
        /// Определяет количество элементов в источнике.
        /// </summary>
        public abstract int Count { get; }
    }
}
