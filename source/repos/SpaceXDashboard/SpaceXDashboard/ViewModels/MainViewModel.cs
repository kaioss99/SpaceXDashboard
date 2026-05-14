using SpaceXDashboard.Commands;
using SpaceXDashboard.Models;
using SpaceXDashboard.repositories;
using SpaceXDashboard.Repositories;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SpaceXDashboard.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ILaunchRepository _launchRepo = new LaunchRepository();
        private readonly IRocketRepository _rocketRepo = new RocketRepository();
        private readonly IStatsRepository _statsRepo = new StatsRepository();

        public ObservableCollection<Launch> Launches { get; set; } = new();
        public ObservableCollection<Rocket> Rockets { get; set; } = new();

        private Stats _stats;
        public Stats Stats
        {
            get => _stats;
            set { _stats = value; OnPropertyChanged(nameof(Stats)); }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public ICommand RefreshCommand { get; }

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
        }

        public async Task LoadDataAsync()
        {
            Status = "Carregando dados...";
            await LoadLaunchesAsync();
            await LoadRocketsAsync();
            await LoadStatsAsync();
            Status = "Dados carregados com sucesso!";
        }

        private async Task LoadLaunchesAsync()
        {
            var launches = await _launchRepo.GetAllAsync();
            Launches.Clear();
            foreach (var l in launches) Launches.Add(l);
        }

        private async Task LoadRocketsAsync()
        {
            var rockets = await _rocketRepo.GetAllAsync();
            Rockets.Clear();
            foreach (var r in rockets) Rockets.Add(r);
        }

        private async Task LoadStatsAsync()
        {
            Stats = await _statsRepo.GetAsync();
        }
    }
}