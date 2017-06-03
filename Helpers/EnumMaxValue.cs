using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCreator.Helpers
{
    public class EnumMaxValue
    {
        public static int GetMaxValue(Type enumerator)
        {
            return Enum.GetValues(enumerator).Cast<int>().Max();
        }
    }
}
