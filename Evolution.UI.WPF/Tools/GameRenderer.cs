using Evolution.Core.Models;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Evolution.UI.WPF
{
    public class GameRenderer
    {
        private readonly Canvas _canvas;
        private const int CellSize = 10;

        public GameRenderer(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void Render(GameField field)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                _canvas.Children.Clear();

                for (int x = 0; x < GameField.width; x++)
                {
                    for (int y = 0; y < GameField.height; y++)
                    {
                        DrawCell(x, y, field.Cells[x, y]);
                    }
                }

                _canvas.UpdateLayout(); // Принудительное обновление UI
            });
        }

        private void DrawCell(int x, int y, Cell cell)
        {
            Rectangle rect = new()
            {
                Width = CellSize,
                Height = CellSize,
                Fill = GetCellColor(cell),
                Stroke = Brushes.Black,
                StrokeThickness = 0.5
            };
            Canvas.SetLeft(rect, x * CellSize);
            Canvas.SetTop(rect, y * CellSize);
            _canvas.Children.Add(rect);
        }

        private Brush GetCellColor(Cell cell)
        {
            // Если в клетке есть бот, он должен быть виден поверх других объектов
            if (cell.Bot != null) return Brushes.Blue;

            return cell.Type switch
            {
                CellType.Wall => Brushes.Gray,
                CellType.Food => Brushes.Red,
                CellType.Poison => Brushes.Green,
                CellType.Empty => Brushes.White,
                _ => Brushes.White
            };
        }
    }
}
