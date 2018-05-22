using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QueueSimulation.BL.Objects
{
    public abstract class SourceBase<T> : ISource<T> where T : ProductBase
    {
        /// <summary>
        /// Очередь для хранения объектов продукции.
        /// </summary>
        protected Queue<T> _productsQueue { get; set; }
        protected IEnumerable<T> _productsCollection { get; set; }
        protected static DateTime Past { get; private set; }

        public event EventHandler OnEmpty = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnEnqueue = delegate { };
        public event EventHandler<ProductEngagedEventArgs<T>> OnDequeue = delegate { };

        public SourceBase()
        {
            _productsQueue = new Queue<T>();
        }

        /// <summary>
        /// Создает коллекцию одинаковых объектов и возвращает их полную копию.
        /// </summary>
        /// <param name="prodCount"></param>
        /// <param name="productType"></param>
        /// <returns></returns>
        protected IEnumerable<T> GenerateSameObjects(int prodCount, T productType, bool deepCopy = false)
        {
            var obj = Enumerable.Repeat(productType, prodCount);

            if (deepCopy == false)
            {
                return obj;
            }

            if (!obj.GetType().IsSerializable)
            {
                throw new ArgumentException(nameof(obj));
            }

            if (obj == null)
            {
                return null;
            }
            else
            {
                BinaryFormatter bf = new BinaryFormatter();

                using (MemoryStream ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    return (IEnumerable<T>)bf.Deserialize(ms);
                }
            }
        }

        public void Dequeue(object sender, ProductEngagedEventArgs<T> e)
        {
            //if (PortOut == null)
            //{
            //    throw new ArgumentNullException(nameof(PortOut));
            //}

                //var temp = this._productsQueue.Dequeue();
            OnDequeue(this, new ProductEngagedEventArgs<T>(e.Product));
            

        }

        public void Reset()
        {
            _productsQueue = new Queue<T>(_productsCollection);
        }

        public void Reset(T product)
        {
            this._productsQueue = new Queue<T>(this.GenerateSameObjects(this._productsQueue.Count, product));
        }

        public void Simulate()
        {
            TimeSpan timeSpan = DateTime.Now - Past;
            if (PorOut is ContainerBase<ProductBase> container && container.CanTakeProduct)
            {
                if (_productsQueue.Count > 0)
                {
                    if (timeSpan.Seconds >= ArrivalRate)
                    {
                        Past = DateTime.Now;
                        OnDequeue(this, new ProductEngagedEventArgs<T>(_productsQueue.Dequeue()));
                    }
                }
            }
            
            
                //OnEmpty(this, EventArgs.Empty);
        }


        public void Enqueue(object sender, ProductEngagedEventArgs<T> e)
        {
            throw new NotSupportedException();
        }

        public void JoinWithPrevious(IDequeueable<T> node)
        {
            node.PorOut = this;
            PortIn = node;
            node.OnDequeue += Enqueue;
        }

        /// <summary>
        /// Инициализирует очередь с определенным количеством размером.
        /// </summary>
        /// <param name="prodCount"></param>
        public SourceBase(int prodCount, T productType)
        {
            _productsCollection = Enumerable.Repeat(productType, prodCount);
            _productsQueue = new Queue<T>(_productsCollection);
            Id = productType.Id;

        }

        static SourceBase()
        {
            Past = new DateTime();
        }

        /// <summary>
        /// Определяет или задает скорость поступления объектов из источника.
        /// </summary>
        public abstract int ArrivalRate { get; set; }

        /// <summary>
        /// Определяет, не пуст ли источник.
        /// </summary>
        public abstract bool IsEmpty { get; }

        /// <summary>
        /// Определяет количество элементов в источнике.
        /// </summary>
        public IDequeueable<T> PortIn { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public IDequeueable<T> PorOut { get; set; }


        public int Id { get; set; }

        public int Count => _productsQueue.Count();

        public string Name { get; set; } = "Source";
    }
}
