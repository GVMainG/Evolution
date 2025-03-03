using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Evolution.Core.Services;
using System.Windows.Input;

namespace Evolution.UI.ClientWPF.VM;

public partial class MainVM : ObservableObject
{
    private readonly GameManager _gameManager;
    private CancellationTokenSource? _cts;

    public ICommand StartCommand { get; }
    public ICommand StopCommand { get; }

    public MainVM()
    {
        _gameManager = new GameManager(1);

        StartCommand = new RelayCommand(StartSimulation);
        StopCommand = new RelayCommand(StopSimulation);
    }

    private async void StartSimulation()
    {
        _cts = new CancellationTokenSource();
        await _gameManager.StartGameAsync(_cts.Token);
    }

    private void StopSimulation()
    {
        _cts?.Cancel();
    }
}
