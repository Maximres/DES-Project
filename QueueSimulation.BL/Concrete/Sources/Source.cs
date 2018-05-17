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
    //public class Source1<T> : SourceBase<T>, ISource<T> where T : ProductBase
    //{
    //    int _capacity;
    //    private static DateTime past;

    //    static Source1()
    //    {
    //        past = new DateTime();
    //    }

    //    public Source1(int capacity, T product) : base(capacity, product)
    //    {
    //        _capacity = capacity;
    //    }

    //    public override ConveyorBase<T> PortOut { get; set; }
    //    public override int ArrivalRate { get; set; } = 3;

    //    public override bool IsEmpty => this._productsQueue.Any();

    //    public override int Count => this._productsQueue.Count();

    //    public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };
    //    public event EventHandler OnEmpty = delegate { };

    //    public T Dequeue()
    //    {
    //        if (PortOut == null)
    //        {
    //            throw new ArgumentNullException(nameof(PortOut));
    //        }

    //        if (IsEmpty == false)
    //        {
    //            var temp = this._productsQueue.Dequeue();
    //            OnDequeue(this, new ProductEngagedEventArgs<T>(temp));
    //            return temp;
    //        }
    //        OnEmpty(this, EventArgs.Empty);
    //        return null;
    //    }

    //    public void Simulate()
    //    {
    //        while (IsEmpty == false)
    //        {
    //            TimeSpan timeSpan = DateTime.Now - past;
    //            if (timeSpan.Seconds >= ArrivalRate)
    //            {
    //                past = DateTime.Now;
    //                Dequeue();
    //            }
    //        }
    //    }

    //    public void Reset()
    //    {
    //        this._productsQueue = new Queue<T>(this._productsCollection);
    //    }

    //    public void Reset(T product)
    //    {
    //        this._productsQueue = new Queue<T>(this.GenerateSameObjects(this._productsQueue.Count, product));
    //    }
    //}

    public class Source<T> : SourceBase<T> where T : ProductBase
    {
        public Source(int productCount, T product): base(productCount, product)
        {
            
        }

        public override ConveyorBase<T> PortOut { get; set; }
        public override int ArrivalRate { get; set; } = 3;

        public override bool IsEmpty => !_productsQueue.Any();

        public override int Count => this._productsQueue.Count;

        
    }
}
