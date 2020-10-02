using Course.Commands;
using Course.Model;
using Course.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Course.ViewModel
{
    class AuthorizationViewModel
    {
        HttpClient client;

        RelayCommand singInCommand;
        public RelayCommand ExitCommand { get; set; }
        public User User { get; private set; }
        public AuthorizationViewModel(HttpClient client)
        {
            this.client = client;
            User = new User();
        }

        public RelayCommand SingInCommand
        {
            get
            {
                return singInCommand ??
                  (singInCommand = new RelayCommand((o) =>
                  {                       
                      try
                      {
                          SingInAsync();
                      }
                      catch (Exception ex)
                      {
                          //logger.Error(ex, "Ошибка загрузки БД после добавления материала");
                      }

                  }, (o => User.Name != null && User.Password != null)
                  ));
            }
        }

        async void SingInAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Users/SingIn?Name=" + User.Name + "&Password="+User.Password);
            if (response.IsSuccessStatusCode)
            {
                MainWindow window = new MainWindow(client);
                window.Show();
                ExitCommand.Execute();
            }
            else
                MessageBox.Show("Введены неверные логин или пароль, попробуйте еще раз");

        }
    }
}
