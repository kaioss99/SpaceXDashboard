using PdfSharp.Drawing;
using PdfSharp.Pdf;
using SpaceXDashboard.Commands;
using SpaceXDashboard.Models;
using SpaceXDashboard.Repositories;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
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
        public ICommand GeneratePdfCommand { get; }

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(async _ => await LoadDataAsync());
            GeneratePdfCommand = new RelayCommand(_ => GeneratePdf());
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

        private void GeneratePdf()
        {
            try
            {
                var document = new PdfDocument();
                document.Info.Title = "SpaceX Dashboard Report";

                var fontTitle = new XFont("Verdana", 18, XFontStyleEx.Bold);
                var fontSub = new XFont("Verdana", 13, XFontStyleEx.Bold);
                var fontNormal = new XFont("Verdana", 11, XFontStyleEx.Regular);

                // ── PÁGINA 1: ESTATÍSTICAS ──────────────
                var page1 = document.AddPage();
                var gfx1 = XGraphics.FromPdfPage(page1);

                gfx1.DrawString("SpaceX Dashboard — Relatório", fontTitle, XBrushes.Black,
                    new XRect(40, 40, page1.Width, 30), XStringFormats.TopLeft);
                gfx1.DrawString($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}", fontNormal, XBrushes.Gray,
                    new XRect(40, 65, page1.Width, 20), XStringFormats.TopLeft);
                gfx1.DrawLine(XPens.LightGray, 40, 85, page1.Width - 40, 85);

                gfx1.DrawString("Estatísticas Gerais", fontSub, XBrushes.Black,
                    new XRect(40, 100, page1.Width, 25), XStringFormats.TopLeft);
                gfx1.DrawString($"Total de lançamentos:      {Stats?.TotalLaunches}", fontNormal, XBrushes.Black, new XRect(40, 130, page1.Width, 20), XStringFormats.TopLeft);
                gfx1.DrawString($"Lançamentos com sucesso:   {Stats?.SuccessfulLaunches}", fontNormal, XBrushes.Black, new XRect(40, 150, page1.Width, 20), XStringFormats.TopLeft);
                gfx1.DrawString($"Lançamentos com falha:     {Stats?.FailedLaunches}", fontNormal, XBrushes.Black, new XRect(40, 170, page1.Width, 20), XStringFormats.TopLeft);
                gfx1.DrawString($"Taxa de sucesso:           {Stats?.SuccessRate:F1}%", fontNormal, XBrushes.Black, new XRect(40, 190, page1.Width, 20), XStringFormats.TopLeft);

                // ── PÁGINA 2: LANÇAMENTOS ───────────────
                var page2 = document.AddPage();
                var gfx2 = XGraphics.FromPdfPage(page2);

                gfx2.DrawString("Lançamentos", fontSub, XBrushes.Black,
                    new XRect(40, 40, page2.Width, 25), XStringFormats.TopLeft);
                gfx2.DrawLine(XPens.LightGray, 40, 65, page2.Width - 40, 65);

                gfx2.DrawString("Nome", fontNormal, XBrushes.Gray, new XRect(40, 75, 200, 20), XStringFormats.TopLeft);
                gfx2.DrawString("Status", fontNormal, XBrushes.Gray, new XRect(260, 75, 80, 20), XStringFormats.TopLeft);
                gfx2.DrawString("Detalhes", fontNormal, XBrushes.Gray, new XRect(350, 75, 300, 20), XStringFormats.TopLeft);

                int y = 100;
                foreach (var l in Launches)
                {
                    if (y > page2.Height - 60) break;
                    var status = l.Success == true ? "Sucesso" : "Falha";
                    gfx2.DrawString(l.Name ?? "", fontNormal, XBrushes.Black, new XRect(40, y, 200, 20), XStringFormats.TopLeft);
                    gfx2.DrawString(status, fontNormal, XBrushes.Black, new XRect(260, y, 80, 20), XStringFormats.TopLeft);
                    gfx2.DrawString(l.Details ?? "", fontNormal, XBrushes.Black, new XRect(350, y, 300, 20), XStringFormats.TopLeft);
                    y += 22;
                }

                // ── PÁGINA 3: FOGUETES ──────────────────
                var page3 = document.AddPage();
                var gfx3 = XGraphics.FromPdfPage(page3);

                gfx3.DrawString("Foguetes", fontSub, XBrushes.Black,
                    new XRect(40, 40, page3.Width, 25), XStringFormats.TopLeft);
                gfx3.DrawLine(XPens.LightGray, 40, 65, page3.Width - 40, 65);

                gfx3.DrawString("Nome", fontNormal, XBrushes.Gray, new XRect(40, 75, 150, 20), XStringFormats.TopLeft);
                gfx3.DrawString("Ativo", fontNormal, XBrushes.Gray, new XRect(200, 75, 80, 20), XStringFormats.TopLeft);
                gfx3.DrawString("Taxa sucesso", fontNormal, XBrushes.Gray, new XRect(290, 75, 100, 20), XStringFormats.TopLeft);
                gfx3.DrawString("Descrição", fontNormal, XBrushes.Gray, new XRect(400, 75, 250, 20), XStringFormats.TopLeft);

                int y3 = 100;
                foreach (var r in Rockets)
                {
                    if (y3 > page3.Height - 60) break;
                    gfx3.DrawString(r.Name ?? "", fontNormal, XBrushes.Black, new XRect(40, y3, 150, 20), XStringFormats.TopLeft);
                    gfx3.DrawString(r.Active ? "Sim" : "Não", fontNormal, XBrushes.Black, new XRect(200, y3, 80, 20), XStringFormats.TopLeft);
                    gfx3.DrawString($"{r.SuccessRatePct}%", fontNormal, XBrushes.Black, new XRect(290, y3, 100, 20), XStringFormats.TopLeft);
                    gfx3.DrawString(r.Description ?? "", fontNormal, XBrushes.Black, new XRect(400, y3, 250, 20), XStringFormats.TopLeft);
                    y3 += 22;
                }

                // ── SALVAR ──────────────────────────────
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "SpaceX_Report.pdf");
                document.Save(path);
                Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });

                Status = $"PDF gerado! Salvo em: {path}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao gerar PDF: {ex.Message}");
            }
        }
    }
}