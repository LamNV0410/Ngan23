using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamThem
{
    public class TourPriceException : Exception
    {
        public TourPriceException()
        {

        }
        public TourPriceException(string infor)
           : base(infor)
        {
        }
    }
}
