using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader
{
    [Serializable]
    public class Mark
    {
        public string name { get; set; }
        public int page { get; set; }
        public override string ToString()
        {
            return $"{name} стр.{page}";
        }
    }
}
