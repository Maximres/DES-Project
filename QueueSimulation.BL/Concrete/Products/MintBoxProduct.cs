using QueueSimulation.BL.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Concrete.Products
{
    public class MintBoxProduct : Product
    {
        int _id;
        string _name;
        bool _isIdle;
        bool _isProccessed;
        bool _isMoving;
        Size _size;
        ProductTypes _prodType;

        public MintBoxProduct()
        {
            _id = 1;
            _name = "Mint Box";
            _isIdle = true;
            _isProccessed = false;
            _isMoving = false;
            _size = new Size(1, 1);
            _prodType = ProductTypes.Mintbox;
        }

        /// <summary>
        /// Возвращает значение <see cref="ProductTypes"/> для текущего объекта
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this._prodType.ToString();
        }

        public override int Id => _id;

        public override string Name { get => _name; set => _name = value; }

        public override bool IsIdle => _isIdle;

        public override Size Size { get => _size; set => _size = value; }

        public override ProductTypes ProductType => _prodType;

        public override bool IsProcessed => _isProccessed;

        public override bool IsMoving => _isMoving;
    }
}
