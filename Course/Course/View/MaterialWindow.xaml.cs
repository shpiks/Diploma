using Course.Context;
using Course.Model;
using Course.ViewModel;
using System.Net.Http;
using System.Windows;


namespace Course.View
{
    /// <summary>
    /// Логика взаимодействия для MaterialWindow.xaml
    /// </summary>
    public partial class MaterialWindow : Window
    {
        
        public MaterialWindow(Material material, Employee employee, HttpClient client)
        {
            InitializeComponent();
            MaterialViewModel materialViewModel = new MaterialViewModel(material, employee, client);
            materialViewModel.ExitCommand = new Commands.RelayCommand(x => this.Close());
            this.DataContext = materialViewModel;
        }



    }
}
