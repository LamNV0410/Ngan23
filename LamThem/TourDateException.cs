using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamThem
{
    public class TourDateException : Exception
    {
        public TourDateException()
        {

        }
        public TourDateException(string infor)
            : base(infor)
        {
        }
    }
}
