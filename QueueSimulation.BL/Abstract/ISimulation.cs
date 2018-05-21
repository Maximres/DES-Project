using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Abstract
{
    public interface ISimulation<T> : IEnqueueable<T>, IDequeueable<T> where T : ProductBase
    {
        /// <summary>
        /// Эмуляция процесса работы.
        /// </summary>
        void Simulate();

        

        /// <summary>
        /// Происходит, когда в источнике заканчиваются объекты
        /// </summary>
        event EventHandler OnEmpty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Dequeue(object sender, ProductEngagedEventArgs<T> e);

        /// <summary>
        /// Добавляет элемент
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Enqueue(object sender, ProductEngagedEventArgs<T> e);

        int Id { get; set; }

        int Count { get; }

        string Name { get; set; }

         void JoinWithPrevious(IDequeueable<T> node);
    }
}
