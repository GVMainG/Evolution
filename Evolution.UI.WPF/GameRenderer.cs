using Evolution.Core.Entities;
using Evolution.Core.Infrastructure;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Evolution.UI.WPF
{
    public class GameRenderer
    {
        private readonly Canvas _canvas;
        private const int CellSize = 10;
        private FieldBase _field;

        public GameRenderer(Canvas canvas, GameLoop gameLoop)
        {
            _canvas = canvas;
            _field = gameLoop.GameField;

            // Подписываем ботов на рендер при движении
            gameLoop.OnBotCreated += bot =>
            {
                bot.OnPosition += RenderCellChange;
            };

            // Подписываемся на изменение каждой клетки
            foreach (var cell in _field.Cells)
            {
                cell.CellChanged += (updatedCell) =>
                {
                    var pos = _field.GetCellPosition(updatedCell);
                    _canvas.Dispatcher.Invoke(() =>
                    {
                        DrawCell(pos.x, pos.y, cell);
                    });
                };
            }

            // Запускаем первый рендер
            Render(_field);
        }

        public void Render(FieldBase field)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                _canvas.Children.Clear();

                for (int x = 0; x < field.Width; x++)
                {
                    for (int y = 0; y < field.Height; y++)
                    {
                        DrawCell(x, y, field.Cells[x, y]);
                    }
                }

                _canvas.UpdateLayout();
            });
        }

        public void RenderCellChange((int x, int y) oldPos, (int x, int y) newPos)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                // Проверяем, что старая позиция находится в пределах поля
                if (IsValidPosition(oldPos.x, oldPos.y))
                {
                    DrawCell(oldPos.x, oldPos.y, _field.Cells[oldPos.x, oldPos.y]);
                }

                // Проверяем, что новая позиция находится в пределах поля
                if (IsValidPosition(newPos.x, newPos.y))
                {
                    DrawCell(newPos.x, newPos.y, _field.Cells[newPos.x, newPos.y]);
                }
            });
        }

        /// <summary>
        /// Проверяет, является ли указанная позиция допустимой.
        /// </summary>
        private bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < _field.Width && y >= 0 && y < _field.Height;
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
            if (cell.Content is Bot bot)
            {
                return bot.Energy > 0 ? Brushes.Blue : Brushes.White;
            }

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
