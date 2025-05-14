using ProjectPV178.Views;
using System.Windows;

namespace ProjectPV178
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Main.Navigate(new LoginPage());
        }
    }
}