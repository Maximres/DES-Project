using QueueSimulation.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public abstract class Product
    {
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Уникальный идентификатор продукта")]
        public abstract int Id { get; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Название продукта продукта")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Определяет, простаивается ли продукт
        /// </summary>
        /// <remarks>Простаивается во время ожидания очереди</remarks>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, простаивается ли продукт")]
        public abstract bool IsIdle { get; }

        /// <summary>
        /// Определяет размер продукции
        /// </summary>
        ///  [Browsable(true)]
        [Browsable(true)]
        [Description("Определяет размер продукции")]
        public abstract Size Size { get; set; }

        /// <summary>
        /// Тип продукции
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Тип продукции")]
        [TypeConverter(typeof(ProductTypeConveter))]
        public abstract ProductTypes ProductType { get; }

        /// <summary>
        /// Определяет, обработан ли продукт
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, обработан ли продукт")]
        public abstract bool IsProcessed { get; }

        /// <summary>
        /// Определяет, движется ли продукция по конвейеру
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, движется ли продукция по конвейеру")]
        public abstract bool IsMoving { get; }
    }
}
