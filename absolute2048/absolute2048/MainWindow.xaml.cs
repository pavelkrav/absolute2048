﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace absolute2048
{
    public partial class MainWindow : Window
    {
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
				drawNewField();
			}
            if (e.Key == Key.Down)
            {
				drawNewField();
			}
            if (e.Key == Key.Up)
            {
				drawNewField();
			}
            if (e.Key == Key.Left)
            {
				drawNewField();
			}
            if (e.Key == Key.N)
            {
                drawNewField();
            }
        }

        private void drawNewField()
        {
            gameGrid.Children.Clear();
            double multiplier = Global.windowSizeModifier / (Global.widthX + Global.heightY);
            this.Width = Global.widthX * multiplier + 16.6;
            this.Height = Global.heightY * multiplier + 39.6;

            gameGrid.Height = this.Height - 39;
            gameGrid.Width = this.Width - 16;

            Field field = new Field();
			drawLayout(gameGrid);
			drawNumbers(gameGrid, field);
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
			foreach (Cell c in field.cells)
			{
				if (c.value != 0)
				{
					drawNumber(new Canvas(), c);
				}
			}
		}

		private void drawNumber(Canvas canvas, Cell cell)
		{
			double cellHeight = gameGrid.Height / Global.heightY;
			double cellWidth = gameGrid.Width / Global.widthX;

			gameGrid.Children.Add(canvas);
			TextBox ini = new TextBox()
			{
				Foreground = Global.lineColor,
				Background = Global.backgroundColor,
				BorderBrush = Global.backgroundColor,
				FontSize = cellHeight / 2,
				Height = canvas.Height,
				Width = canvas.Width,
				VerticalContentAlignment = VerticalAlignment.Center,
				HorizontalContentAlignment = HorizontalAlignment.Center,
				IsReadOnly = true,
				Cursor = Cursors.None
			};
			ini.Text = cell.label;
			ini.FontWeight = FontWeights.Bold;
			canvas.Children.Add(ini);
			Canvas.SetLeft(ini, (cell.X - 1) * gameGrid.Width / Global.widthX + cellWidth / 4);
			Canvas.SetTop(ini, (cell.Y - 1) * gameGrid.Height / Global.heightY + cellHeight / 4);
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
				Background = canvas.Background,
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
