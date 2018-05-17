using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Products
{
    public class OrangeBoxProduct : ProductBase
    {
        int _id;

        public OrangeBoxProduct(int id, string name, Size boxSize)
        {
            this._id = id;
            this.Size = boxSize;
            this.Name = name;
        }

        public override int Id => _id;

        public override string Name { get; set; }

        public override bool IsIdle => true;

        public override Size Size { get; set; }

        public override ProductTypes ProductType => ProductTypes.OrangeBox;

        public override bool IsProcessed => false;

        public override bool IsMoving => false;
    }
}
