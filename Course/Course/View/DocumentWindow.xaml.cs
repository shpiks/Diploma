using Course.Model;
using Course.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для DocumentWindow.xaml
    /// </summary>
    public partial class DocumentWindow : Window
    {
        public DocumentWindow(Material material, HttpClient client)
        {
            InitializeComponent();
            DocumentViewModel documentViewModel = new DocumentViewModel(material, client);
            this.DataContext = documentViewModel;
            //authorizationViewModel.ExitCommand = new Commands.RelayCommand(x => this.Close());

        }
    }
}
