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
    [Serializable]
    public abstract class ProductBase
    {
        /// <summary>
        /// Получает уникальный идентификатов объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Уникальный идентификатор продукта")]
        public abstract int Id { get; }

        /// <summary>
        /// Получает или задает имя объекта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Название продукта продукта")]
        public abstract string Name { get; set; }

        /// <summary>
        /// Определяет, простаивается ли продукт.
        /// </summary>
        /// <remarks>Простаивается в очереди</remarks>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, простаивается ли продукт")]
        public abstract bool IsIdle { get; }

        /// <summary>
        /// Определяет или задает размер продукта (в метрах).
        /// </summary>
        ///  [Browsable(true)]
        [Browsable(true)]
        [Description("Определяет размер продукции")]
        public abstract Size Size { get; set; }

        /// <summary>
        /// Определяет тип продукта.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Тип продукции")]
        [TypeConverter(typeof(ProductTypeConveter))]
        public abstract ProductTypes ProductType { get; }

        /// <summary>
        /// Определяет, обработан ли продукт и готов ли к передаче.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, обработан ли продукт")]
        public abstract bool IsProcessed { get; }

        /// <summary>
        /// Определяет, движется ли продукция по конвейеру.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(true)]
        [Description("Определяет, движется ли продукция по конвейеру")]
        public abstract bool IsMoving { get; }


    }
}
