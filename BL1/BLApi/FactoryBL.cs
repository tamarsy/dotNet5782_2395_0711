using BlApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLApi
{
    public class FactoryBL
    {
        public static IBL GetBL()
        {
            return BL.BL.Instance;
        }
    }
}
