using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reader
{
    [Serializable]
    public class Book
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int PageNow { get; set; }
        public int PagesNumber { get; set; } = 3;
        public List<Mark> marks;
        public Book() {
            marks = new List<Mark>();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
