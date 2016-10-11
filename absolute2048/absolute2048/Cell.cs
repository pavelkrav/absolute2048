using System;

namespace absolute2048
{
    class Cell
    {
		public int X { get; set; }
		public int Y { get; set; }

		public int value { get; set; }
		public string label { get { return value.ToString(); } }

		public bool isNew { get; set; }

        public Cell(int value)
        {
            this.value = value;
        }
    }
}
