using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class ContainerBase<T> where T : ProductBase
    {
        /// <summary>
        /// Добавляет вершину к объекту.
        /// </summary>
        /// <param name="node"></param>
        public virtual void AddNode(IDequeueable<T> node)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveNode(IDequeueable<T> node) => throw new NotImplementedException();

        [Browsable(true)]
        [ReadOnly(false)]
        [Description("Вместительность объекта")]
        public abstract int Capacity { get; set; }

        //[Browsable(true)]
        //[ReadOnly(true)]
        //[Description("Название объекта")]
        //public abstract string Name { get; set; }

        public abstract int Id { get; set; }
    }
}
