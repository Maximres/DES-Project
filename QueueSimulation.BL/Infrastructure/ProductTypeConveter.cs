using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Infrastructure
{
    public class ProductTypeConveter : EnumConverter
    {
        Type _type;

        public ProductTypeConveter(Type type) : base(type)
        {
            _type = type;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            FieldInfo fi = _type.GetField(Enum.GetName(_type, value));
            DescriptionAttribute descAttr =
          (DescriptionAttribute)Attribute.GetCustomAttribute(
            fi, typeof(DescriptionAttribute));

            if (descAttr != null)
                return descAttr.Description;
            else
                return value.ToString();
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            foreach (FieldInfo fi in _type.GetFields())
            {
                DescriptionAttribute descAttr =
                  (DescriptionAttribute)Attribute.GetCustomAttribute(
                    fi, typeof(DescriptionAttribute));

                if ((descAttr != null) && ((string)value == descAttr.Description))
                    return Enum.Parse(_type, fi.Name);
            }
            return Enum.Parse(_type, (string)value);
        }
    }
}
