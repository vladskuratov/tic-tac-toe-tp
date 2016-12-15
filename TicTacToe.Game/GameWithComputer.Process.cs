﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe.Game
{
    public partial class GameWithComputer
    {
        const int X = 1;
        const int O = 2;
        const int step = 85;

        bool isFieldBlocked = false;
        bool computersTurn = false;
        int[,] field = new int[3, 3];
        int userPuts = X;
        int computerPuts = O;

        int stepsMade = 0;

        int userWins = 0;
        int computerWins = 0;

        IComputerBrain cb;

        private void DrawX(double i, double j)
        {
            Line line = new Line();

            line = new Line();
            line.Stroke = Brushes.LightGray;
            line.StrokeThickness = 8;

            line.X1 = 30 + step * j + 20;
            line.X2 = 30 + step * (j + 1) - 20;
            line.Y1 = 30 + step * i + 20;
            line.Y2 = 30 + step * (i + 1) - 20;

            grid.Children.Add(line);


            line = new Line();
            line.Stroke = Brushes.LightGray;
            line.StrokeThickness = 8;

            line.X1 = 30 + step * (j + 1) - 20;
            line.X2 = 30 + step * j + 20;
            line.Y1 = 30 + step * i + 20;
            line.Y2 = 30 + step * (i + 1) - 20;

            grid.Children.Add(line);
        }

        private void DrawO(double i, double j)
        {
            Ellipse ellipse = new Ellipse();

            SolidColorBrush brush = new SolidColorBrush();

            ellipse.StrokeThickness = 8;
            ellipse.Stroke = Brushes.LightGray;

            ellipse.Width = 55;
            ellipse.Height = 55;

            Canvas.SetLeft(ellipse, 44 + step * j);
            Canvas.SetTop(ellipse, 44 + step * i);

            grid.Children.Add(ellipse);
        }

        private async void ComputersTurn()
        {
            cb = Factory.Default.GetComputerBrain();

            await Task.Delay(500);

            int x = cb.MyTurnYX(field)[1];
            int y = cb.MyTurnYX(field)[0];

            field[y, x] = computerPuts;
            stepsMade++;

            if (computerPuts == X)
            {
                DrawX(y, x);
            }
            else
            {
                DrawO(y, x);
            }

            if (CheckForWinOrDraw() == false)
            {
                computersTurn = false;
                return;
            }
            else return;
        }

        public void DrawLine(double x1, double x2, double x3, double x4)
        {
            Line line = new Line();

            line = new Line();
            line.Stroke = Brushes.LightGray;
            line.StrokeThickness = 8;

            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = x3;
            line.Y2 = x4;

            grid.Children.Add(line);
        }

        private bool? CheckForWinOrDraw()
        {
            if (stepsMade < 5) return false;

            for (int i = 0; i < 3; i++)
            {
                if (field[i, 0] == field[i, 1] && field[i, 1] == field[i, 2] && field[i, 2] != 0)
                {
                    Win(i, 2, 2);
                    return true;
                }

                else if (field[0, i] == field[1, i] && field[1, i] == field[2, i] && field[2, i] != 0)
                {
                    Win(2, i, 3);
                    return true;
                }
            }

            if (field[0, 0] == field[1, 1] && field[1, 1] == field[2, 2] && field[2, 2] != 0)
            {
                Win(2, 2, 0);
                return true;
            }

            if (field[0, 2] == field[1, 1] && field[1, 1] == field[2, 0] && field[2, 0] != 0)
            {
                Win(2, 0, 1);
                return true;
            }

            if (stepsMade >= 9)
            {
                Draw();
                return null;
            }

            return false;
        }

        private void Win(int y, int x, int pos)
        {
            isFieldBlocked = true;

            if (pos == 0) DrawLine(30, 30 + 85 * 3, 30, 30 + 85 * 3);
            if (pos == 1) DrawLine(30 + 85 * 3, 30, 30, 30 + 85 * 3);
            if (pos == 2) DrawLine(30, 85 * 3 + 30, 30 + 85 * y + 85 / 2, 30 + 85 * y + 85 / 2);
            if (pos == 3) DrawLine(30 + 85 * x + 85 / 2, 30 + 85 * x + 85 / 2, 30, 85 * 3 + 30);

            if (field[y, x] == computerPuts) button3.Content = ++computerWins;
            if (field[y, x] == userPuts) button2.Content = ++userWins;

            MessageBox.Show(string.Format("Выиграл {0}", (field[y, x] == userPuts) ? "ты" : "компьютер"), "Партия!");
        }

        private void Draw()
        {
            isFieldBlocked = true;

            MessageBox.Show("Произошла ничья", "Партия!");
        }
    }
}
