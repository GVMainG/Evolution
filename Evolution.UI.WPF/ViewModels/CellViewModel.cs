using Prism.Mvvm;
using System.Windows.Media;
using Evolution.Core.Entities;

namespace Evolution.UI.WPF.ViewModels
{
    public class CellViewModel : BindableBase
    {
        private readonly Cell _cell;
        private Brush _color;

        public Brush Color
        {
            get => _color;
            private set => SetProperty(ref _color, value);
        }

        public CellViewModel(Cell cell)
        {
            _cell = cell;
            Color = GetCellColor();
            _cell.CellChanged += _ => Update();
        }

        public void Update()
        {
            Color = GetCellColor();
        }

        private Brush GetCellColor()
        {
            return _cell.Type switch
            {
                CellType.Wall => Brushes.Gray,
                CellType.Food => Brushes.Red,
                CellType.Poison => Brushes.Green,
                CellType.Bot => Brushes.Blue,
                _ => Brushes.White
            };
        }
    }
}
