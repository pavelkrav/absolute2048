using System;
using System.Linq;

namespace absolute2048
{
	class Field
	{
		public Cell[,] cells { get; set; } = new Cell[Global.widthX, Global.heightY];
		public int score { get { return getScore(); } }

		public Field()
		{
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					cells[i, j] = new Cell(0);
					cells[i, j].X = i + 1;
					cells[i, j].Y = j + 1;
					cells[i, j].isNew = false;
				}
			}

			spawn();
		}		

		private int getZeros()
		{
			int i = 0;
			foreach (Cell c in cells)
			{
				if (c.value == 0)
					i++;
			}
			return i;
		}

		private int getScore()
		{
			int score = 0;
			foreach (Cell c in cells)
			{
				score += c.value;
			}
			return score;
		}

		private void fillAll()
		{
			for (int i = 0; i < Global.heightY; i++)
			{
				for (int j = 0; j < Global.widthX; j++)
				{
					if (cells[j, i].value == 0)
					{
						cells[j, i].value = Global.basisValue;
						cells[j, i].isNew = true;
					}
				}
			}
		}

		private void makeThemOld()
		{
			for (int i = 0; i < Global.heightY; i++)
			{
				for (int j = 0; j < Global.widthX; j++)
				{
					cells[j, i].isNew = false;
				}
			}
		}

		private void consoleOutput()
		{
			for (int i = 0; i < Global.heightY; i++)
			{
				for (int j = 0; j < Global.widthX; j++)
				{
					Console.Write($"{cells[j, i].value} ");
				}
				Console.Write("\n");
			}
		}

		private void spawn()
		{
			makeThemOld();
			if (getZeros() >= Global.spawnValue)
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
					cells[x, y].isNew = true;
				}
			}
			else
			{
				if (GameOver != null)
				{
					fillAll();
					//consoleOutput();
					GameOverEventArgs e = new GameOverEventArgs(score);
					GameOver(this, e);
				}
			}
		}


		private int[] step(int[] line)
		{
			int len = line.Length;

			if (line.Sum() == 0)
				return line;
			for (int i = 0; i < len - 1; i++)
			{
				// shift through empty cells
				for (int j = 0; j < len; j++)
				{
					// empty-check
					int sum = 0;
					for (int rest = j; rest < len; rest++)
					{
						sum += line[rest];
					}
					if (sum == 0)
						break;

					// shift
					while (line[j] == 0)
					{
						for (int n = j; n < len - 1; n++)
						{
							line[n] = line[n + 1];
							line[n + 1] = 0;
						}
					}
				}

				// addition
				for (int j = 0; j < len - 1; j++)
				{
					if (line[j] == line[j + 1] && line[j] != 0)
					{
						line[j] *= Global.basisValue;
						line[j + 1] = 0;
					}
				}
			}

			return line;
		}

		public void moveUp()
		{
			// saving state
			int[,] preState = new int[Global.widthX, Global.heightY];
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					preState[i, j] = cells[i, j].value;
				}
			}

			// step
			for (int i = 0; i < Global.widthX; i++)
			{
				int[] temp = new int[Global.heightY];

				for (int j = 0; j < Global.heightY; j++)
				{
					temp[j] = cells[i, j].value;
				}

				temp = step(temp);

				for (int j = 0; j < Global.heightY; j++)
				{
					cells[i, j].value = temp[j];
				}
			}

			// cheking state
			bool spwn = false;
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					if (cells[i, j].value != preState[i, j])
					{
						spwn = true;
						if (spwn)
							break;
					}
				}
				if (spwn)
					break;
			}
			if (spwn)
				spawn();
			else
				makeThemOld();
		}

		public void moveDown()
		{
			// saving state
			int[,] preState = new int[Global.widthX, Global.heightY];
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					preState[i, j] = cells[i, j].value;
				}
			}

			// step
			for (int i = 0; i < Global.widthX; i++)
			{
				int[] temp = new int[Global.heightY];

				for (int j = 0; j < Global.heightY; j++)
				{
					temp[j] = cells[i, Global.heightY - 1 - j].value;
				}

				temp = step(temp);

				for (int j = 0; j < Global.heightY; j++)
				{
					cells[i, Global.heightY - 1 - j].value = temp[j];
				}
			}

			// cheking state
			bool spwn = false;
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					if (cells[i, j].value != preState[i, j])
					{
						spwn = true;
						if (spwn)
							break;
					}
				}
				if (spwn)
					break;
			}
			if (spwn)
				spawn();
			else
				makeThemOld();
		}

		public void moveLeft()
		{
			// saving state
			int[,] preState = new int[Global.widthX, Global.heightY];
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					preState[i, j] = cells[i, j].value;
				}
			}

			// step
			for (int i = 0; i < Global.heightY; i++)
			{
				int[] temp = new int[Global.widthX];

				for (int j = 0; j < Global.widthX; j++)
				{
					temp[j] = cells[j, i].value;
				}

				temp = step(temp);

				for (int j = 0; j < Global.widthX; j++)
				{
					cells[j, i].value = temp[j];
				}
			}

			// cheking state
			bool spwn = false;
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					if (cells[i, j].value != preState[i, j])
					{
						spwn = true;
						if (spwn)
							break;
					}
				}
				if (spwn)
					break;
			}
			if (spwn)
				spawn();
			else
				makeThemOld();
		}

		public void moveRight()
		{
			// saving state
			int[,] preState = new int[Global.widthX, Global.heightY];
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					preState[i, j] = cells[i, j].value;
				}
			}

			// step
			for (int i = 0; i < Global.heightY; i++)
			{
				int[] temp = new int[Global.widthX];

				for (int j = 0; j < Global.widthX; j++)
				{
					temp[j] = cells[Global.widthX - 1 - j, i].value;
				}

				temp = step(temp);

				for (int j = 0; j < Global.widthX; j++)
				{
					cells[Global.widthX - 1 - j, i].value = temp[j];
				}
			}

			// cheking state
			bool spwn = false;
			for (int i = 0; i < Global.widthX; i++)
			{
				for (int j = 0; j < Global.heightY; j++)
				{
					if (cells[i, j].value != preState[i, j])
					{
						spwn = true;
						if (spwn)
							break;
					}
				}
				if (spwn)
					break;
			}
			if (spwn)
				spawn();
			else
				makeThemOld();
		}

		public event EventHandler<GameOverEventArgs> GameOver;
	}

	public class GameOverEventArgs : EventArgs
	{
		public int score;
		public GameOverEventArgs(int score)
		{
			this.score = score;
		}
	}

}
