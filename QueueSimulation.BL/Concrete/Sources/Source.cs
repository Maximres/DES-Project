using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Sources
{
    public class Source<T> : SourceBase<T> where T : ProductBase
    {
        public Source(int productCount, T product): base(productCount, product)
        {
            
        }

        public override int ArrivalRate { get; set; } = 5;

        public override bool IsEmpty => !_productsQueue.Any();


    }
}
