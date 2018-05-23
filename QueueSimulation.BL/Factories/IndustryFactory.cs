using QueueSimulation.BL.Abstract;
using QueueSimulation.BL.Concrete.Products;
using QueueSimulation.BL.Objects;
using QueueSimulation.Infrastructure.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation
{
    public class IndustryFactory<T> : IndustryAbstractFactory<T> where T : ProductBase
    {
        static NextNodeId IdContainer = NextNodeId.GetInstance();
        static MintBoxProduct mintPrototype = new MintBoxProduct(0, "p" + IdContainer.GetNextId(), new System.Drawing.Size(1, 2));
        static RedBoxProduct redPrototype = new RedBoxProduct(1, "p" + IdContainer.GetNextId(), new System.Drawing.Size(2, 2));
        static OrangeBoxProduct orangePrototype = new OrangeBoxProduct(2, "p" + IdContainer.GetNextId(), new System.Drawing.Size(2, 3));
        static Conveyor<T> conveyorPrototype = new Conveyor<T>()
        {
            Name = "c" + IdContainer.GetNextId(),
            Delay = 0,
            Capacity = 200,
            Length = 12,
            Id = 7,
            Speed = 6
        };
        static Machine<T> firstMachinePrototype = new Machine<T>()
        {
            Name = "m" + IdContainer.GetNextId(),
            Id = 3,
            CrashChance = 0.1,
            Delay = 3,
            InactiveTime = 4,
            CrashRatePerProduct = 100
        };

        static Machine<T> secondMachinePrototype = new Machine<T>()
        {
            Name = "m" + IdContainer.GetNextId(),
            Id = 4,
            CrashChance = 0.15,
            Delay = 3,
            InactiveTime = 4,
            CrashRatePerProduct = 100
        };
        static Machine<T> thirdMachinePrototype = new Machine<T>()
        {
            Name = "m" + IdContainer.GetNextId(),
            Id = 5,
            CrashChance = 0.1,
            Delay = 2,
            InactiveTime = 5,
            CrashRatePerProduct = 100
        };
        static Machine<T> fourthMachinePrototype = new Machine<T>()
        {
            Name = "m" + IdContainer.GetNextId(),
            Id = 6,
            CrashChance = 0.1,
            Delay = 2,
            InactiveTime = 5,
            CrashRatePerProduct = 100
        };

        private object CreateDeepCopy(object obj)
        {
            if (obj.GetType().IsSerializable == false)
            {
                return new ArgumentException(nameof(obj), "Невозможно сериализовать");
            }

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            BinaryFormatter bf = new BinaryFormatter();

            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return bf.Deserialize(ms);
            }
        }

        public override Conveyor<T> CreateConveyor()
        {
            var conv = (Conveyor<T>)CreateDeepCopy(conveyorPrototype);
            conv.Name = "c" + IdContainer.GetNextId();
            return conv;
        }

        public override Machine<T> CreateMachine(string machineName)
        {
            dynamic obj;
            switch (machineName)
            {
                case "Станок1":
                    obj = (CreateDeepCopy(firstMachinePrototype) as Machine<T>);
                    obj.Name = "m" + IdContainer.GetNextId();
                    return obj;
                case "Станок2":
                    obj = (Machine<T>)CreateDeepCopy(secondMachinePrototype);
                    obj.Name = "m" + IdContainer.GetNextId();
                    return obj;
                case "Станок3":
                    obj = (Machine<T>)CreateDeepCopy(thirdMachinePrototype);
                    obj.Name = "m" + IdContainer.GetNextId();
                    return obj;
                case "Станок4":
                    obj = (Machine<T>)CreateDeepCopy(fourthMachinePrototype);
                    obj.Name = "m" + IdContainer.GetNextId();
                    return obj;
                case null:
                    throw new ArgumentNullException(nameof(machineName), "Name must not be NULL");
                default:
                    throw new ArgumentException(nameof(machineName), "Name is not recognized");
            }
        }

        public override object CreateObject(string name)
        {
            switch (name)
            {
                case "Конвейер":
                    return CreateConveyor();
                case "Станок1":
                    return CreateMachine(name);
                case "Станок2":
                    return CreateMachine(name);
                case "Станок3":
                    return CreateMachine(name);
                case "Станок4":
                    return CreateMachine(name);
                case "Продукт1":
                    return CreateProduct(name);
                case "Продукт2":
                    return CreateProduct(name);
                case "Продукт3":
                    return CreateProduct(name);
                case null:
                    throw new ArgumentNullException(nameof(name), "Name must not be NULL");
                default:
                    return null;
            }
        }

        public override ProductBase CreateProduct(string productName)
        {
            dynamic obj;
            switch (productName)
            {
                case "Продукт1":
                    obj = (ProductBase)CreateDeepCopy(mintPrototype);
                    obj.Name = "p" + IdContainer.GetNextId();
                    return obj;
                case "Продукт2":
                    obj = (ProductBase)CreateDeepCopy(redPrototype);
                    obj.Name = "p" + IdContainer.GetNextId();
                    return obj;
                case "Продукт3":
                    obj = (ProductBase)CreateDeepCopy(orangePrototype);
                    obj.Name = "p" + IdContainer.GetNextId();
                    return obj;
                case null:
                    throw new ArgumentNullException(nameof(productName), "Name must not be NULL");
                default:
                    throw new ArgumentException(nameof(productName), "Name is not recognized");
            }
        }

        public override ProductBase CreateProduct(int id)
        {
            switch (id)
            {
                case 0:
                    return CreateProduct("Продукт1");
                case 1:
                    return CreateProduct("Продукт2");
                case 2:
                    return CreateProduct("Продукт3");
                default:
                    throw new ArgumentException(nameof(id), "Name is not recognized");
            }
        }

        internal override void SetParametresToObject(dynamic obj)
        {
            if (obj is ISimulation<T> || obj is ProductBase)
            {
            }
            else
            {
                return;
            }


            switch (obj)
            {
                case Machine<T> m:
                    DetermineAndUpdate(m);
                    break;
                case Conveyor<T> con:
                    conveyorPrototype = con;
                    break;
                case MintBoxProduct mint:
                    mintPrototype = mint;
                    break;
                case RedBoxProduct red:
                    redPrototype = red;
                    break;
                case OrangeBoxProduct orange:
                    orangePrototype = orange;
                    break;
                case null:
                default:
                    break;
            }
        }

        private void DetermineAndUpdate(Machine<T> m)
        {
            switch (m.Id)
            {
                case 3:
                    firstMachinePrototype = m;
                break;
                case 4:
                    secondMachinePrototype = m;
                break;
                case 5:
                    thirdMachinePrototype = m;
                break;
                case 6:
                    fourthMachinePrototype = m;
                break;
                default:
                    break;
            }
        }

        public override object CreateObject(int id)
        {
            switch (id)
            {
                case 0:
                    return CreateProduct("Продукт1");
                case 1:
                    return CreateProduct("Продукт2");
                case 2:
                    return CreateProduct("Продукт3");
                case 3:
                    return CreateMachine("Станок1");
                case 4:
                    return CreateMachine("Станок2");
                case 5:
                    return CreateMachine("Станок3");
                case 6:
                    return CreateMachine("Станок4");
                case 7:
                    return CreateConveyor();
                default:
                    throw new ArgumentException(nameof(id), "Name is not recognized");
            }
        }
    }
}
