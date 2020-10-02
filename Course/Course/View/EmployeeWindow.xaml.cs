using Course.Context;
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
    /// Логика взаимодействия для EmployeeWindow.xaml
    /// </summary>
    public partial class EmployeeWindow : Window
    {
        public EmployeeWindow(Employee employee, HttpClient client)
        {
            InitializeComponent();
            EmployeeViewModel employeeViewModel = new EmployeeViewModel(employee, client);
            employeeViewModel.ExitCommand = new Commands.RelayCommand(x => this.Close());
            //Material = p;
            //this.DataContext = Material;
            this.DataContext = employeeViewModel;
        }
    }
}
