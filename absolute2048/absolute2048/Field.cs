using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace absolute2048
{
    class Field
    {
        public Cell[,] cells { get; set; } = new Cell[Global.widthX, Global.heightY];

        public Field()
        {
            for (int i = 0; i < Global.widthX; i++)
            {
                for (int j = 0; j < Global.heightY; j++)
                {
                    cells[i, j] = new Cell(0);
					cells[i, j].X = i + 1;
					cells[i, j].Y = j + 1;
				}
            }
                
            spawn();
        }

        private void spawn()
        {
			Random rand = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < Global.spawnValue; i++)
            {
                int x = 0;
                int y = 0;
                do
                {
                    x = rand.Next(Global.widthX);
                    y = rand.Next(Global.heightY);
                }
                while (cells[x, y].value != 0);
                cells[x, y].value = Global.basisValue;
            }
        }

		public void moveUp()
		{
			Cell[,] currentCells = cells;

			for (int i = 0; i < Global.heightY; i++)
			{
				for (int j = 0; j < Global.widthX; j++)
				{
					try
					{
						if (cells[j, i].value == cells[j, i + 1].value && cells[j, i].value != 0)
						{
							cells[j, i].value += cells[j, i + 1].value;
							cells[j, i + 1].value = 0;							
						}
						if (cells[j, i].value == 0)
						{
							for (int k = i; k < Global.heightY; k++)
							{
								cells[j, k].value += cells[j, k + 1].value;
								cells[j, k + 1].value = 0;
							}
						}
					}
					catch (System.IndexOutOfRangeException) {; }
				}
			}

			//if (currentCells != cells)
				spawn();
		}

		public void moveDown()
		{

		}

		public void moveLeft()
		{

		}

		public void moveRight()
		{

		}
    }
}
