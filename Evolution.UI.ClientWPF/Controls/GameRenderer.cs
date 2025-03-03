using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Evolution.Core.Models;

namespace Evolution.UI.ClientWPF.Controls
{
    public class GameRenderer
    {
        private readonly GameField _gameField;
        private readonly Canvas _canvas;
        private const int CellSize = 10;

        public GameRenderer(GameField gameField, Canvas canvas)
        {
            _gameField = gameField;
            _canvas = canvas;
        }

        public void Render()
        {
            _canvas.Children.Clear();

            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    var cell = _gameField.GetCell(x, y);

                    if (cell.Foods.Count > 0)
                    {
                        DrawRectangle(x, y, Brushes.Green);
                    }
                    else if (cell.Units.Count > 0)
                    {
                        DrawRectangle(x, y, Brushes.Blue);
                    }
                    else
                    {
                        DrawRectangle(x, y, Brushes.Black);
                    }
                }
            }
        }

        private void DrawRectangle(int x, int y, Brush color)
        {
            var rect = new Rectangle
            {
                Width = CellSize,
                Height = CellSize,
                Fill = color
            };
            Canvas.SetLeft(rect, x * CellSize);
            Canvas.SetTop(rect, y * CellSize);
            _canvas.Children.Add(rect);
        }
    }
}
