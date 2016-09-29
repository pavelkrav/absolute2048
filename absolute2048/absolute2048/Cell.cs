using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace absolute2048
{
    class Cell
    {
        public int value { get; set; }
		public string label { get { return value.ToString(); } }

        public Cell(int value)
        {
            this.value = value;
        }
    }
}
