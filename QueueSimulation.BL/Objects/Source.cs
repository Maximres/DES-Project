using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class Source
    {
        /// <summary>
        /// Порт выхода объектов
        /// </summary>
        /// <remarks>1 каждую секунду</remarks>
        public abstract Conveyor<Product> PortOut { get; set; }

        /// <summary>
        /// Скорость поступления объектов
        /// </summary>
        public abstract double ArrivalRate { get; set; }

        /// <summary>
        /// Максимальное количество объектов
        /// </summary>
        public abstract long MaxArrivals { get; set; }



    }
}
