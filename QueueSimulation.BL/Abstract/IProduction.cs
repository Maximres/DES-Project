using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface IProduction<T> where T : Product
    {
        /// <summary>
        /// Происходит, когда продукт готов к передаче в другой объект.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnProcessed;

        /// <summary>
        /// Происходит, когда продукт начинает двигаться по конвейеру.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnMoving;

        /// <summary>
        /// Происходит, когда продукт находится в очереди.
        /// </summary>
        event EventHandler<ProductEngagedEventArgs<T>> OnIdle;
    }
}
