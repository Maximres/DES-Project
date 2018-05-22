using QueueSimulation.BL.Concrete.Products;
using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueueSimulation.Infrastructure.Nodes;
using QueueSimulation.BL.Concrete.Machines;
using QueueSimulation.BL.Concrete.Conveyors;

namespace QueueSimulation
{
    public class IndustryFactory<T> : IndustryObjectFactory<T> where T : ProductBase
    {
        NextNodeId NodeId = NextNodeId.GetInstance();

        public override Conveyor<T> CreateConveyor()
        {
            return new Conveyor<T>() { Name = "c"+NodeId.GetNextId() };
        }

        public override Machine<T> CreateMachine(string machineName)
        {
            switch (machineName)
            {
                case "Станок1":
                    return new Machine<T>()
                    {
                        Name = "m" + NodeId.GetNextId(),
                        Id = 3,
                        CrashChance = 0.1,
                        Delay = 3,
                        InactiveTime = 4,
                        CrashRatePerProduct = 100
                    };
                case "Станок2":
                    return new Machine<T>()
                    {
                        Name = "m" + NodeId.GetNextId(),
                        Id = 4,
                        CrashChance = 0.15,
                        Delay = 3,
                        InactiveTime = 4,
                        CrashRatePerProduct = 100
                    };
                case "Станок3":
                    return new Machine<T>()
                    {
                        Name = "m" + NodeId.GetNextId(),
                        Id = 3,
                        CrashChance = 0.1,
                        Delay = 2,
                        InactiveTime = 5,
                        CrashRatePerProduct = 100
                    };
                case "Станок4":
                    return new Machine<T>()
                    {
                        Name = "m" + NodeId.GetNextId(),
                        Id = 6,
                        CrashChance = 0.1,
                        Delay = 2,
                        InactiveTime = 5,
                        CrashRatePerProduct = 100
                    };
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
                    return CreateMachine("Станок1");
                case "Станок2":
                    return CreateMachine("Станок2");
                case "Станок3":
                    return CreateMachine("Станок3");
                case "Станок4":
                    return CreateMachine("Станок4");
                case "Продукт1":
                    return new MintBoxProduct(0, "p" + NodeId.GetNextId(), new System.Drawing.Size(1, 2));
                case "Продукт2":
                    return new RedBoxProduct(1, "p" + NodeId.GetNextId(), new System.Drawing.Size(2, 2));
                case "Продукт3":
                    return new OrangeBoxProduct(2, "p" + NodeId.GetNextId(), new System.Drawing.Size(2, 3));
                case null:
                    throw new ArgumentNullException(nameof(name), "Name must not be NULL");
                default:
                    return null;
            }
        }

        public override ProductBase CreateProduct(string productName)
        {
            switch (productName)
            {
                case "Продукт1":
                    return new MintBoxProduct(0, "p" + NodeId.GetNextId(), new System.Drawing.Size(1, 2));
                case "Продукт2":
                    return new RedBoxProduct(1, "p" + NodeId.GetNextId(), new System.Drawing.Size(2, 2));
                case "Продукт3":
                    return new OrangeBoxProduct(2, "p" + NodeId.GetNextId(), new System.Drawing.Size(2, 3));
                case null:
                    throw new ArgumentNullException(nameof(productName), "Name must not be NULL");
                default:
                    throw new ArgumentException(nameof(productName), "Name is not recognized");
            }
        }
    }
}
