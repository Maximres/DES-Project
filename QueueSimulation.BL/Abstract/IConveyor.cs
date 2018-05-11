using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface IConveyor<T> where T : Product
    {
        /// <summary>
        /// Происходит, когда конвейер выпускает продукт.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnDequeue;

        /// <summary>
        /// Происходит, когда конвейер получает продукт.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue;

        /// <summary>
        /// Эмулирует работу конвейера.
        /// </summary>
        void EmulateConveyor();
    }
}
