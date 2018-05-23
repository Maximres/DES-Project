using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface ISource<T> : ISimulation<T> where T : ProductBase
    {

        /// <summary>
        /// Обновляет источник.
        /// </summary>
        void Reset();

        /// <summary>
        /// Обновляет источник новыми объектами
        /// </summary>
        void Reset(int count, T product);

        
    }
}
