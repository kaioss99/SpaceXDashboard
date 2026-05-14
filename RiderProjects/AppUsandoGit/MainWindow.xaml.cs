using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AppUsandoGit;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
       
        double n1 = Convert.ToDouble(cxNota1.Text);
        double n2 = Convert.ToDouble(cxNota2.Text);
        double n3 = Convert.ToDouble(cxNota3.Text);

        double media = (n1 + n2 + n3) / 3;

    
        if (media >= 60)
        {
            MessageBox.Show("APROVADO! Com a nota de: " + media);
        }
        else
        {
            MessageBox.Show("REPROVADO. Com a nota de: " + media);
        }
    }
}