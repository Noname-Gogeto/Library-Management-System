using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Management_System
{
    internal class ObjBook
    {
        public int BookID { get; set; }
        public String BookName { get; set; }
        public int BookQuantity { get; set; }
        public override string ToString()
        {
            return this.BookName;
        }
    }
}
