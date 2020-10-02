using Course.Context;
using Course.Model;
using Course.View;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Course
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        System.Threading.Mutex mutex;
        private void Application_Startup()
        {
            bool createdNew;
            string mutName = "Course";

            mutex = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                this.Shutdown();
            }

        }



        [STAThread]
        static void Main()
        {
            App app = new App();
            app.Application_Startup();
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Запуск приложения");

            var splash = new SplashScreen("Resources/preview.jpeg");
            splash.Show(true);
            ApplicationContext db;
            db = new ApplicationContext();

            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000/api/")
            };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            //MainWindow window = new MainWindow(db, client);
            АuthorizationWindow аuthorization = new АuthorizationWindow(client);

            //DispatcherTimer dispatcherTimer = new DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler((sender, e) => dispatcherTimer_Tick(sender, e, db, dispatcherTimer, client));
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            //dispatcherTimer.Start();

            app.Run(аuthorization);



        }

        //private static void dispatcherTimer_Tick(object sender, EventArgs e, ApplicationContext db, DispatcherTimer dispatcherTimer, HttpClient client)
        //{
        //    ArrayList listMaterials = new ArrayList();
        //    Logger logger = LogManager.GetCurrentClassLogger();

        //    try
        //    {
               
        //        GetMaterialsWhereTermTomorrowAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "Ошибка загрузки БД в таймере");
        //    }

        //    //if (listMaterials.Count != 0)
        //    //{
        //    //    NotificationWindow notificationWindow = new NotificationWindow(db, true, client);
        //    //    notificationWindow.ShowDialog();
        //    //}
        //    //dispatcherTimer.Interval = new TimeSpan(2, 0, 0);

        //    async void GetMaterialsWhereTermTomorrowAsync()
        //    {
        //        HttpResponseMessage response = await client.GetAsync("Materials/GetMaterialsWhereTermTomorrow");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var materialsTerm = await response.Content.ReadAsAsync<List<Material>>();

        //            materialsTerm.ForEach(x => listMaterials.Add(x));
        //            if (listMaterials.Count != 0)
        //            {
        //                dispatcherTimer.Interval = new TimeSpan(2, 0, 0);
        //                NotificationWindow notificationWindow = new NotificationWindow(db, true, client);
        //                notificationWindow.ShowDialog();
        //            }
        //            dispatcherTimer.Interval = new TimeSpan(2, 0, 0);

        //        }

        //    }
        //}
    }
}

