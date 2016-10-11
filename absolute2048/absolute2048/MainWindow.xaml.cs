using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace absolute2048
{
    public partial class MainWindow : Window
    {
		private Field currentField;
		private bool gameOver;

		public MainWindow()
        {
            InitializeComponent();
        }

		// initialization event
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

			gameOver = false;

			fieldGrid.Background = null;
			fieldGrid.Columns = Global.widthX;
			fieldGrid.Rows = Global.heightY;

			newCellsGrid.Background = null;
			newCellsGrid.Columns = Global.widthX;
			newCellsGrid.Rows = Global.heightY;

			Global.backgroundColor = getBackground(0);

			// window size determination
            double multiplier = Global.windowSizeModifier / (Global.widthX + Global.heightY);
            this.Width = Global.widthX * multiplier + 16.6;
            this.Height = Global.heightY * multiplier + 39.6;
            this.Background = Global.backgroundColor;

			// grid size determination
            gameGrid.Height = this.Height - 39;
            gameGrid.Width = this.Width - 16;
			fieldGrid.Height = gameGrid.Height;
			fieldGrid.Width = gameGrid.Width;
			newCellsGrid.Height = gameGrid.Height;
			newCellsGrid.Width = gameGrid.Width;

			testCellColors();
			drawSpecialText(gameGrid, new Canvas(), "Press N to start new game");
        }

		private void OnGameOver(Object sender, GameOverEventArgs e)
		{
			drawFrame(gameGrid, currentField);
			gameOverAnimation(e);
			gameOver = true;
			currentField = null;
		}

		private void gameOverAnimation(GameOverEventArgs e)
		{
			DoubleAnimation goAnimation = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(4.0 * Global.animationSpeed / 15) };
			goAnimation.From = 1.0;
			goAnimation.To = 0.0;
			fieldGrid.BeginAnimation(OpacityProperty, goAnimation);
			newCellsGrid.BeginAnimation(OpacityProperty, goAnimation);

			TextBox txt = new TextBox()
			{
				Foreground = Global.lineColor,
				Background = null,
				BorderBrush = null,
				FontSize = Global.windowSizeModifier / Global.widthX / 11,
				Height = gameGrid.Height,
				Width = gameGrid.Width,
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				IsReadOnly = true,
				Cursor = Cursors.None
			};
			txt.Text = $"Game over. Your score {e.score}\n\nPress N to start new game";
			txt.FontWeight = FontWeights.Bold;
			gameGrid.Children.Add(txt);
		}

		// on key down event
        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
				try
				{ 
					currentField.moveRight();
					if (!gameOver)
						drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
            if (e.Key == Key.Down)
            {
				try
				{
					currentField.moveDown();
					if (!gameOver)
						drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
			if (e.Key == Key.Up)
			{
				try
				{
					currentField.moveUp();
					if (!gameOver)
						drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
			if (e.Key == Key.Left)
			{
				try
				{
					currentField.moveLeft();
					if (!gameOver)
						drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
            if (e.Key == Key.N)
            {
				iniNewField();
            }
        }

		/// <summary>
		/// Initialize new field (method on "N" key down)
		/// </summary>
		private void iniNewField()
        {
			// window size determination
			gameGrid.Children.Clear();
			fieldGrid.Children.Clear();
			newCellsGrid.Children.Clear();

			double multiplier = Global.windowSizeModifier / (Global.widthX + Global.heightY);
            this.Width = Global.widthX * multiplier + 16.6;
            this.Height = Global.heightY * multiplier + 39.6;

			// grid size determination
			gameGrid.Height = this.Height - 39;
            gameGrid.Width = this.Width - 16;
			fieldGrid.Height = gameGrid.Height;
			fieldGrid.Width = gameGrid.Width;
			newCellsGrid.Height = gameGrid.Height;
			newCellsGrid.Width = gameGrid.Width;

			// field initialization
			currentField = new Field();
			gameOver = false;
			currentField.GameOver += OnGameOver;

			DoubleAnimation appearence = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.05) };
			appearence.From = 0.0;
			appearence.To = 1.0;
			fieldGrid.BeginAnimation(OpacityProperty, appearence);

			drawFrame(gameGrid, currentField);
		}

		/// <summary>
		/// Draw frame (after every step)
		/// </summary>
		private void drawFrame(Grid grid, Field field)
		{
			grid.Children.Clear();
			//grid.Children.Remove(fieldGrid);		// leaks of RAM somewhere
			drawNumbers(grid, field);
			if (Global.enableLines)
				drawLayout(grid);

			DoubleAnimation appearence = new DoubleAnimation() { Duration = TimeSpan.FromSeconds(0.01 * Global.animationSpeed) };
			appearence.From = 0.0;
			appearence.To = 1.0;
			newCellsGrid.BeginAnimation(OpacityProperty, appearence);
		}

		/// <summary>
		///  Draw layout grid
		/// </summary>
		private void drawLayout(Grid grid)
        {
			// draw vertical lines
			for (int i = 0; i <= Global.widthX; i++)
			{
				Line currentLine = new Line();
				currentLine.X1 = grid.Width / Global.widthX * i;
				currentLine.X2 = grid.Width / Global.widthX * i;
				currentLine.Y1 = 0;
				currentLine.Y2 = grid.Height;
				currentLine.Stroke = Global.lineColor;
				if (i == 0 || i == Global.widthX)
					currentLine.StrokeThickness = 3; // borders
				else
					currentLine.StrokeThickness = 2;
				grid.Children.Add(currentLine);
			}

			// draw horizontal lines
			for (int i = 0; i <= Global.heightY; i++)
			{
				Line currentLine = new Line();
				currentLine.X1 = 0;
				currentLine.X2 = grid.Width;
				currentLine.Y1 = grid.Height / Global.heightY * i;
				currentLine.Y2 = grid.Height / Global.heightY * i;
				currentLine.Stroke = Global.lineColor;
				if (i == 0 || i == Global.heightY)
					currentLine.StrokeThickness = 3; // borders
				else
					currentLine.StrokeThickness = 2;
				grid.Children.Add(currentLine);
			}
		}

		/// <summary>
		/// Draw all numbers on grid
		/// </summary>
		private void drawNumbers(Grid grid, Field field)
		{
			// adding UniformGrid
			grid.Children.Add(fieldGrid);
			grid.Children.Add(newCellsGrid);
			fieldGrid.Children.Clear();
			newCellsGrid.Children.Clear();

			// filling cells
			for (int i = 0; i < Global.heightY; i++)
			{
				for (int j = 0; j < Global.widthX; j++)
				{
					if (field.cells[j, i].value != 0)
					{
						drawNumber(field.cells[j, i]);
					}
					else
					{
						fieldGrid.Children.Add(new Canvas());
						newCellsGrid.Children.Add(new Canvas());
					}
				}
			}
		}

		/// <summary>
		/// Draw single number of a cell
		/// </summary>
		private void drawNumber(Cell cell)
		{
			Brush background = getBackground(cell.value);

			TextBox txt = new TextBox()
			{
				Foreground = Global.lineColor,
				Background = background,
				BorderBrush = null,
				FontSize = 32,
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				IsReadOnly = true,
				Cursor = Cursors.None
			};
			txt.Text = cell.label;
			txt.FontWeight = FontWeights.Bold;
			while (txt.FontSize * (txt.Text.Length - 1) > gameGrid.Width / Global.widthX)
				txt.FontSize /= 1.2;
			if (cell.isNew)
			{
				fieldGrid.Children.Add(new Canvas());
				newCellsGrid.Children.Add(txt);
			}
			else
			{
				fieldGrid.Children.Add(txt);
				newCellsGrid.Children.Add(new Canvas());
			}
		}

		/// <summary>
		/// Set background color for cell
		/// </summary>
		/// <param name="value">Value of cell</param>
		/// <returns>Brush for background</returns>
		private Brush getBackground(int value)
		{
			int i = 0;
			while (value != 1 && value != 0)
			{
				value /= Global.basisValue;
				i++;
			}

			int alpha = 255;
			int red = 255;
			int green = 255;
			int blue = 255;

			switch (Global.clrScheme)
			{
				case Global.colorScheme.coffee:
					Global.lineColor = new SolidColorBrush(Color.FromArgb(255, 90, 55, 22));
					alpha = 225;
					if (i < 10)
					{
						red = 253 - i * 2;
						green = 245 - i * 8;
						blue = 230 - i * 12;
					}
					else
					{
						red = 240 - (i - 9) * 4;
						green = 240 - (i - 9) * 30;
						blue = 2 * i;
					}
					break;

				case Global.colorScheme.sky:
					Global.lineColor = new SolidColorBrush(Color.FromArgb(255, 44, 44, 66));
					alpha = 255;
					if (i < 10)
					{
						red = 210 - i * 10;
						green = 245 - i * 10;
						blue = 255;
					}
					else
					{
						red = 215 - (i - 9) * 15;
						green = 160 - (i - 9) * 25;
						blue = 200 - (i - 9) * 15;
					}
					break;

				default :
					break;
			}

			var brush = new SolidColorBrush(Color.FromArgb((byte)alpha, (byte)red, (byte)green, (byte)blue));
			return brush;
		}

		/// <summary>
		/// Draw centered unclickable message
		/// </summary>
		private static void drawCenteredText(Grid grid, Canvas canvas, string text)
        {
            try
            {
				grid.Children.Clear();
				grid.Children.Add(canvas);
            }
            catch (System.ArgumentException) {; }
            canvas.Children.Clear();
            TextBox ini = new TextBox()
            {
				Foreground = Global.lineColor,
				Background = null,
				BorderBrush = null,
				FontSize = Global.windowSizeModifier / Global.widthX / 11,
                Height = grid.Height,
                Width = grid.Width,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                IsReadOnly = true,
                Cursor = Cursors.None
            };
            ini.Text = text;
            ini.FontWeight = FontWeights.Bold;
            canvas.Children.Add(ini);
        }

		private static void drawSpecialText(Grid grid, Canvas canvas, string text)
		{
			grid.Children.Add(canvas);

			canvas.Children.Clear();
			TextBox ini = new TextBox()
			{
				Foreground = Global.lineColor,
				Background = null,
				BorderBrush = null,
				FontSize = Global.windowSizeModifier / Global.widthX / 11,
				Height = grid.Height,
				Width = grid.Width,
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				IsReadOnly = true,
				Cursor = Cursors.None
			};
			ini.Text = text;
			ini.FontWeight = FontWeights.Bold;
			canvas.Children.Add(ini);
		}

		/// <summary>
		/// Output available cells in order for color and font test
		/// </summary>
		private void testCellColors()
		{
			gameGrid.Children.Clear();
			fieldGrid.Children.Clear();

			gameGrid.Children.Add(fieldGrid);
			for (int i = Global.widthX * Global.heightY - 1; i >= 1 ; i--)
			{
				int value = i == 0 ? 0 : (int)(Math.Pow(Global.basisValue, i));
				Brush background = getBackground(value);

				TextBox txt = new TextBox()
				{
					Foreground = Global.lineColor,
					Background = background,
					BorderBrush = null,
					FontSize = Global.windowSizeModifier / Global.widthX / 6,
					VerticalContentAlignment = VerticalAlignment.Center,
					HorizontalContentAlignment = HorizontalAlignment.Center,
					IsReadOnly = true,
					Cursor = Cursors.None
				};
				txt.Text = value == 0 ? "" : value.ToString();
				txt.FontWeight = FontWeights.Bold;
				while (txt.FontSize * (txt.Text.Length - 1.8) > gameGrid.Width / Global.widthX)        
					txt.FontSize /= 1.2;
				fieldGrid.Children.Add(txt);
			}

			if (Global.enableLines)
				drawLayout(gameGrid);
		}
    }
}
