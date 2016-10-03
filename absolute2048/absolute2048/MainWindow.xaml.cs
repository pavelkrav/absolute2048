using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace absolute2048
{
    public partial class MainWindow : Window
    {
		private Field currentField;

		public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            double multiplier = Global.windowSizeModifier / (Global.widthX + Global.heightY);
            this.Width = Global.widthX * multiplier + 16.6;
            this.Height = Global.heightY * multiplier + 39.6;
            this.Background = Global.backgroundColor;

            gameGrid.Height = this.Height - 39;
            gameGrid.Width = this.Width - 16;
            drawCenteredText(gameGrid, new Canvas(), "Press N to start new game");
        }

        private void keyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
				try
				{ 
					currentField.moveRight();
					drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
            if (e.Key == Key.Down)
            {
				try
				{
					currentField.moveDown();
					drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
            if (e.Key == Key.Up)
            {
				try
				{
					currentField.moveUp();
					drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
            if (e.Key == Key.Left)
            {
				try
				{
					currentField.moveLeft();
					drawFrame(gameGrid, currentField);
				}
				catch (System.NullReferenceException) {; }
			}
            if (e.Key == Key.N)
            {
                iniNewField();
            }
        }

        private void iniNewField()
        {
            gameGrid.Children.Clear();
            double multiplier = Global.windowSizeModifier / (Global.widthX + Global.heightY);
            this.Width = Global.widthX * multiplier + 16.6;
            this.Height = Global.heightY * multiplier + 39.6;

            gameGrid.Height = this.Height - 39;
            gameGrid.Width = this.Width - 16;

            currentField = new Field();
			drawFrame(gameGrid, currentField);
		}

		private void drawFrame(Grid grid, Field field)
		{
			grid.Children.Clear();
			//grid.Children.Remove(fieldGrid);
			drawLayout(grid);
			drawNumbers(grid, field);
		}

        private void drawLayout(Grid grid)
        {
			for (int i = 0; i <= Global.widthX; i++)
			{
				Line currentLine = new Line();
				currentLine.X1 = grid.Width / Global.widthX * i;
				currentLine.X2 = grid.Width / Global.widthX * i;
				currentLine.Y1 = 0;
				currentLine.Y2 = grid.Height;
				currentLine.Stroke = Global.lineColor;
				if (i == 0 || i == Global.widthX)
					currentLine.StrokeThickness = 3;
				else
					currentLine.StrokeThickness = 2;
				grid.Children.Add(currentLine);
			}

			for (int i = 0; i <= Global.heightY; i++)
			{
				Line currentLine = new Line();
				currentLine.X1 = 0;
				currentLine.X2 = grid.Width;
				currentLine.Y1 = grid.Height / Global.heightY * i;
				currentLine.Y2 = grid.Height / Global.heightY * i;
				currentLine.Stroke = Global.lineColor;
				if (i == 0 || i == Global.heightY)
					currentLine.StrokeThickness = 3;
				else
					currentLine.StrokeThickness = 2;
				grid.Children.Add(currentLine);
			}
		}

		private void drawNumbers(Grid grid, Field field)
		{
			grid.Children.Add(fieldGrid);
			fieldGrid.Children.Clear();
			fieldGrid.Background = null;
			fieldGrid.Columns = Global.widthX;
			fieldGrid.Rows = Global.heightY;

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
					}
				}
			}
		}

		private void drawNumber(Cell cell)
		{
			TextBox txt = new TextBox()
			{
				Foreground = Global.lineColor,
				Background = null,
				BorderBrush = null,
				FontSize = 32,
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				IsReadOnly = true,
				Cursor = Cursors.None
			};
			txt.Text = cell.label;
			txt.FontWeight = FontWeights.Bold;
			fieldGrid.Children.Add(txt);
		}

		public static void drawCenteredText(Grid grid, Canvas canvas, string text)
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
				Background = Global.backgroundColor,
				BorderBrush = Global.backgroundColor,
				FontSize = 15,
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
    }
}
