using Course.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Course.View
{
    /// <summary>
    /// Логика взаимодействия для АuthorizationWindow.xaml
    /// </summary>
    public partial class АuthorizationWindow : Window
    {
        public АuthorizationWindow(HttpClient client)
        {
            InitializeComponent();
            AuthorizationViewModel authorizationViewModel = new AuthorizationViewModel(client);
            this.DataContext = authorizationViewModel;
            authorizationViewModel.ExitCommand = new Commands.RelayCommand(x => this.Close());
        }
    }
}
