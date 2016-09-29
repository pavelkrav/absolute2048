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
                
            respawn();
        }

        public void respawn()
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
    }
}
