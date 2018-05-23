using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Products
{
    [Serializable]
    public class MintBoxProduct : ProductBase
    {
        readonly int _id;
        string _name;
        Size _size;

        public MintBoxProduct(int id, string name, Size boxSize)
        {
            _id = id;
            _name = name;
            _size = boxSize;
        }

        /// <summary>
        /// Возвращает значение <see cref="ProductTypes"/> для текущего объекта
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ProductType.ToString();
        }

        public override int Id => _id;

        public override string Name { get => _name; set => _name = value; }

        public override bool IsIdle => true;

        public override Size Size { get => _size; set => _size = value; }

        public override ProductTypes ProductType => ProductTypes.Mintbox;

        public override bool IsProcessed => false;

        public override bool IsMoving => false;
    }
}
