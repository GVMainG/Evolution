using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Evolution.Core;
using Evolution.Core.Models;

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
            _canvas.Children.Clear();

            for (int x = 0; x < GameField.Width; x++)
            {
                for (int y = 0; y < GameField.Height; y++)
                {
                    DrawCell(x, y, field.Cells[x, y]);
                }
            }
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
            return cell.Type switch
            {
                CellType.Empty => Brushes.White,
                CellType.Wall => Brushes.Gray,
                CellType.Food => Brushes.Red,
                CellType.Poison => Brushes.Green,
                CellType.Bot => Brushes.Blue,
                _ => Brushes.White
            };
        }
    }
}
