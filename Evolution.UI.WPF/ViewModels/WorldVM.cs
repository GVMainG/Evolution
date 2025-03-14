using Evolution.Core.Core;
using Evolution.Core.Entities;
using Evolution.UI.WPF.ViewModels;
using Prism.Mvvm;
using System.Collections.ObjectModel;

public class WorldVM : BindableBase
{
    public readonly SemulationLoop gameLoop;
    public ObservableCollection<CellVM> Cells { get; }

    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set => SetProperty(ref _isRunning, value);
    }

    public WorldVM(SemulationLoop gameLoop)
    {
        this.gameLoop = gameLoop;
        Cells = [];

        foreach(var c in gameLoop.World.Cells)
        {
            var newCellVM = new CellVM(c.Id);
            c.CellChanged += newCellVM.Update;
            Cells.Add(newCellVM);

            newCellVM.Update(c.Type);
        }
    }

    public void Start()
    {
        if (!IsRunning)
        {
            gameLoop.Start();
            IsRunning = true;
        }
    }

    public void Stop()
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