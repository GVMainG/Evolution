using Evolution.Core.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows;

namespace Evolution.UI.WPF.ViewModels
{
    public class SimulationViewModel : BindableBase
    {
        public readonly GameLoop gameLoop;
        public ObservableCollection<CellViewModel> Cells { get; }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public SimulationViewModel(GameLoop gameLoop)
        {
            this.gameLoop = gameLoop;
            Cells = new ObservableCollection<CellViewModel>();

            // Подписываемся на обновления
            this.gameLoop.EvolutionManager.OnBotsUpdated += () =>
            {
                Application.Current.Dispatcher.Invoke(UpdateCells);
            };

            this.gameLoop.EvolutionManager.OnGenerationChanged += (generation) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Console.WriteLine($"Смена поколения: {generation}");
                });
            };

            InitializeCells();
        }

        private void InitializeCells()
        {
            for (int x = 0; x < gameLoop.GameField.Width; x++)
            {
                for (int y = 0; y < gameLoop.GameField.Height; y++)
                {
                    Cells.Add(new CellViewModel(gameLoop.GameField.Cells[x, y]));
                }
            }
        }

        private void UpdateCells()
        {
            foreach (var cell in Cells)
            {
                cell.Update();
            }
        }

        public void StartSimulation()
        {
            if (!IsRunning)
            {
                gameLoop.Start();
                IsRunning = true;
            }
        }

        public void StopSimulation()
        {
            if (IsRunning)
            {
                gameLoop.Stop();
                IsRunning = false;
            }
        }

        public void SetSimulationSpeed(int speed)
        {
            gameLoop.SetGameSpeed(speed);
        }
    }
}
