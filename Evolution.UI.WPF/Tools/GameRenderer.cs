using Evolution.Core.Models;
using Evolution.Core.Tools;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Evolution.UI.WPF
{
    public class GameRenderer
    {
        private readonly Canvas _canvas;
        private const int CellSize = 10;
        private GameField _field;

        public GameRenderer(Canvas canvas, GameLoop gameLoop)
        {
            _canvas = canvas;
            _field = gameLoop.GameField;

            // Подписываем всех существующих ботов
            foreach (var bot in gameLoop.Bots)
            {
                bot.Moved += RenderCellChange;
            }

            // Подписываем новых ботов при создании
            gameLoop.OnBotCreated += bot =>
            {
                bot.Moved += RenderCellChange;
            };
        }

        public void Render(GameField field)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                _canvas.Children.Clear();

                for (int x = 0; x < field.width; x++)
                {
                    for (int y = 0; y < field.height; y++)
                    {
                        DrawCell(x, y, field.Cells[x, y]);
                    }
                }

                _canvas.UpdateLayout();
            });
        }

        public void RenderCellChange((int oldX, int oldY) oldPos, (int newX, int newY) newPos)
        {
            _canvas.Dispatcher.Invoke(() =>
            {
                DrawCell(oldPos.oldX, oldPos.oldY, _field.Cells[oldPos.oldX, oldPos.oldY]);
                DrawCell(newPos.newX, newPos.newY, _field.Cells[newPos.newX, newPos.newY]);
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
            if (cell.Content is Bot bot)
            {
                if (bot.Energy > 0)
                {
                    return Brushes.Blue; // Живой бот
                }
                else
                {
                    return Brushes.White; // Мертвый бот (убираем его с карты)
                }
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
