using Prism.Mvvm;
using System.Windows.Media;
using Evolution.Core.Entities;

namespace Evolution.UI.WPF.ViewModels
{
    public class CellVM : BindableBase
    {
        public readonly Guid id;

        private Brush _color;
        private CellType _type;

        public CellType Type 
        { 
            get => _type;
            set
            {
                _type = value;
                Color = GetCellColor();
            } 
        }

        public Brush Color
        {
            get => _color;
            private set => SetProperty(ref _color, value);
        }

        public CellVM(Guid id)
        {
            this.id = id;
        }

        public void Update(CellType type)
        {
            Type = type;
        }

        private Brush GetCellColor()
        {
            return Type switch
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
