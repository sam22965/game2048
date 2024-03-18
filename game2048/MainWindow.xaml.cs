using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Game2048
{
    public partial class MainWindow : Window
    {
        private const int GridSize = 4;

        private int[,] gridValues;
        private Label[,] gridLabels;

        private Random random;


        public MainWindow()
        {
            InitializeComponent();

            gridValues = new int[GridSize, GridSize];
            gridLabels = new Label[GridSize, GridSize];
            random = new Random();

            InitializeGrid();
            AddRandomTile();
            AddRandomTile();
        }


        private void InitializeGrid()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    gridValues[i, j] = 0;


                    Label label = new Label
                    {
                        Content = "",
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        FontSize = 28,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(5),
                        Background = Brushes.LightGray,
                    };

                    gridLabels[i, j] = label;


                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    grid.Children.Add(label);
                }
            }
        }


        private void AddRandomTile()
        {
            List<Tuple<int, int>> emptyCells = new List<Tuple<int, int>>();

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (gridValues[i, j] == 0)
                    {
                        emptyCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            if (emptyCells.Count > 0)
            {
                int randomIndex = random.Next(emptyCells.Count);
                var cell = emptyCells[randomIndex];
                int value = random.Next(10) == 0 ? 4 : 2;
                gridValues[cell.Item1, cell.Item2] = value;
                UpdateGrid();
                AnimateNewTile(cell.Item1, cell.Item2);
            }
        }


        private void UpdateGrid()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    int value = gridValues[i, j];

                    if (value == 0)
                    {
                        gridLabels[i, j].Content = "";
                        gridLabels[i, j].Background = Brushes.LightGray;
                    }
                    else
                    {
                        gridLabels[i, j].Content = value.ToString();


                        switch (value)
                        {
                            case 2:
                                gridLabels[i, j].Background = Brushes.LightYellow;
                                break;
                            case 4:
                                gridLabels[i, j].Background = Brushes.LightGoldenrodYellow;
                                break;
                            case 8:
                                gridLabels[i, j].Background = Brushes.LightSalmon;
                                break;
                            case 16:
                                gridLabels[i, j].Background = Brushes.Orange;
                                break;
                            case 32:
                                gridLabels[i, j].Background = Brushes.DarkOrange;
                                break;
                            case 64:
                                gridLabels[i, j].Background = Brushes.OrangeRed;
                                break;
                            case 128:
                                gridLabels[i, j].Background = Brushes.DarkRed;
                                break;
                            case 256:
                                gridLabels[i, j].Background = Brushes.LightGreen;
                                break;
                            case 512:
                                gridLabels[i, j].Background = Brushes.ForestGreen;
                                break;
                            case 1024:
                                gridLabels[i, j].Background = Brushes.LightBlue;
                                break;
                            case 2048:
                                gridLabels[i, j].Background = Brushes.Blue;
                                break;
                            default:
                                gridLabels[i, j].Background = Brushes.Purple;
                                break;
                        }
                    }
                }
            }
        }


        private void AnimateNewTile(int row, int column)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 1;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));

            gridLabels[row, column].BeginAnimation(OpacityProperty, animation);
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;

            switch (e.Key)
            {
                case Key.Up:
                    moved = MoveUp();
                    break;
                case Key.Down:
                    moved = MoveDown();
                    break;
                case Key.Left:
                    moved = MoveLeft();
                    break;
                case Key.Right:
                    moved = MoveRight();
                    break;
            }

            if (moved)
            {
                AddRandomTile();

                if (IsGameOver())
                {
                    MessageBox.Show("Game over!");
                }
            }
        }


        private bool IsGameOver()
        {
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    if (gridValues[i, j] == 0 ||
                        (i > 0 && gridValues[i, j] == gridValues[i - 1, j]) ||
                        (i < GridSize - 1 && gridValues[i, j] == gridValues[i + 1, j]) ||
                        (j > 0 && gridValues[i, j] == gridValues[i, j - 1]) ||
                        (j < GridSize - 1 && gridValues[i, j] == gridValues[i, j + 1]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        private bool MoveUp()
        {
            bool moved = false;

            for (int j = 0; j < GridSize; j++)
            {
                for (int i = 1; i < GridSize; i++)
                {
                    if (gridValues[i, j] != 0)
                    {
                        int k = i;

                        while (k > 0 && gridValues[k - 1, j] == 0)
                        {
                            gridValues[k - 1, j] = gridValues[k, j];
                            gridValues[k, j] = 0;
                            k--;
                            moved = true;
                        }

                        if (k > 0 && gridValues[k - 1, j] == gridValues[k, j])
                        {
                            gridValues[k - 1, j] *= 2;
                            gridValues[k, j] = 0;
                            moved = true;
                        }
                    }
                }
            }

            return moved;
        }


        private bool MoveDown()
        {
            bool moved = false;

            for (int j = 0; j < GridSize; j++)
            {
                for (int i = GridSize - 2; i >= 0; i--)
                {
                    if (gridValues[i, j] != 0)
                    {
                        int k = i;

                        while (k < GridSize - 1 && gridValues[k + 1, j] == 0)
                        {
                            gridValues[k + 1, j] = gridValues[k, j];
                            gridValues[k, j] = 0;
                            k++;
                            moved = true;
                        }

                        if (k < GridSize - 1 && gridValues[k + 1, j] == gridValues[k, j])
                        {
                            gridValues[k + 1, j] *= 2;
                            gridValues[k, j] = 0;
                            moved = true;
                        }
                    }
                }
            }

            return moved;
        }


        private bool MoveLeft()
        {
            bool moved = false;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 1; j < GridSize; j++)
                {
                    if (gridValues[i, j] != 0)
                    {
                        int k = j;

                        while (k > 0 && gridValues[i, k - 1] == 0)
                        {
                            gridValues[i, k - 1] = gridValues[i, k];
                            gridValues[i, k] = 0;
                            k--;
                            moved = true;
                        }

                        if (k > 0 && gridValues[i, k - 1] == gridValues[i, k])
                        {
                            gridValues[i, k - 1] *= 2;
                            gridValues[i, k] = 0;
                            moved = true;
                        }
                    }
                }
            }

            return moved;
        }


        private bool MoveRight()
        {
            bool moved = false;

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = GridSize - 2; j >= 0; j--)
                {
                    if (gridValues[i, j] != 0)
                    {
                        int k = j;

                        while (k < GridSize - 1 && gridValues[i, k + 1] == 0)
                        {
                            gridValues[i, k + 1] = gridValues[i, k];
                            gridValues[i, k] = 0;
                            k++;
                            moved = true;
                        }

                        if (k < GridSize - 1 && gridValues[i, k + 1] == gridValues[i, k])
                        {
                            gridValues[i, k + 1] *= 2;
                            gridValues[i, k] = 0;
                            moved = true;
                        }
                    }
                }
            }

            return moved;
        }
    }
}