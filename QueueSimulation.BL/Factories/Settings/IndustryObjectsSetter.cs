using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.Infrastructure
{
    public sealed class IndustryObjectsSetter<T> where T : ProductBase
    {
        private readonly IndustryAbstractFactory<T> _factory = null;
        public IndustryObjectsSetter(IndustryAbstractFactory<T> abstractFactory)
        {
            _factory = abstractFactory;
        }

        public void SetNewParametresToObject(dynamic obj)
        {
            _factory.SetParametresToObject(obj);
        }
    }
}
