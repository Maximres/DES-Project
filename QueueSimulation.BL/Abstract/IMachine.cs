using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface IMachine<T> where T : Product
    {
        /// <summary>
        /// Происходит, когда станок выпускает объект.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnDequeue;

        /// <summary>
        /// Происходит, когда станок получает объект.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue;

        /// <summary>
        /// Эмулирует работу станка.
        /// </summary>
        void EmulateMachine();
    }
}
