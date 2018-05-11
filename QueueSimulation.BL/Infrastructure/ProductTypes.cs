using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulation.BL.Objects
{
    public enum ProductTypes
    {
        [Description("Mint box")]
        Mintbox = 0,
        [Description("Red box")]
        RedBox,
        [Description("Orange box")]
        OrangeBox
    }
}
