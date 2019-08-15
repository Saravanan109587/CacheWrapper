using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASCacheManager;
namespace SampleAPp
{
    class Program
    {
        static void Main(string[] args)
        {
            IASCacheProvider c = new ASCacheProvider("localhost:6379");
           
             var test  =c.Get<byte[]>("8866");
        }
    }
}
