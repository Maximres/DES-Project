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

        public override ConveyorBase<T> CreateConveyor()
        {
            return new MainConveyor<T>() { Name = "c"+NodeId.GetNextId() };
        }

        public override MachineBase<T> CreateMachine(string machineName)
        {
            switch (machineName)
            {
                case "Станок1":
                    return new FirstMachine<T>() { Name = "m" + NodeId.GetNextId() };
                case "Станок2":
                    return new SecondMachine<T>() { Name = "m" + NodeId.GetNextId() };
                case "Станок3":
                    return new ThirdMachine<T>() { Name = "m" + NodeId.GetNextId() };
                case "Станок4":
                    return new FourthMachine<T>() { Name = "m" + NodeId.GetNextId() };
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
                    return new MainConveyor<T>() { Name = "c" + NodeId.GetNextId() };
                case "Станок1":
                    return new FirstMachine<T>() { Name = "m" + NodeId.GetNextId() };
                case "Станок2":
                    return new SecondMachine<T>() { Name = "m" + NodeId.GetNextId() };
                case "Станок3":
                    return new ThirdMachine<T>() { Name = "m" + NodeId.GetNextId() };
                case "Станок4":
                    return new FourthMachine<T>() { Name = "m" + NodeId.GetNextId() };
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
