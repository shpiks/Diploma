using Course.Context;
using Course.Model;
using Course.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        public ObservableCollection<Material> Materials { get; set; }

    public NotificationWindow(bool b, HttpClient client )
        {
            InitializeComponent();
            NotificationViewModel notificationViewModel = new NotificationViewModel(b, client);
            this.DataContext = notificationViewModel;

            //Materials = new ObservableCollection<Material>();
            //db.Materials.ToList().Where(x => x.DateOfTerm == DateTime.Today.AddDays(1) && x.ExecutedOrNotExecuted != true).ToList().ForEach(x => Materials.Add(x));
            //this.DataContext = notificationViewModel;
            //LBNotifications.ItemsSource = Materials;
        }
    }
}
