using Course.Commands;
using Course.Context;
using Course.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Course.ViewModel
{
    class NotificationViewModel
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        HttpClient client;

        public ObservableCollection<Material> Materials { get; set; }

        public NotificationViewModel(bool b, HttpClient client)
        {
            this.client = client;
            Materials = new ObservableCollection<Material>();
            try
            {
                if (b == true)
                {
                    //db.Materials.ToList().Where(x => x.DateOfTerm == DateTime.Today.AddDays(1) && x.ExecutedOrNotExecuted != true).ToList().ForEach(x => Materials.Add(x));
                    GetMaterialsWhereTermTomorrowAsync();
                    logger.Info("Вызов уведомления таймером");
                }
                else
                    //db.Materials.ToList().Where(x => x.DateOfTerm == DateTime.Today && x.ExecutedOrNotExecuted != true).ToList().ForEach(x => Materials.Add(x));
                    GetMaterialsWhereTermTodayAsync();

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                logger.Error(exc, "Ошибка с загрузкой данных в уведомлениях");
            }

        }

        async void GetMaterialsWhereTermTodayAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Materials/");
            if (response.IsSuccessStatusCode)
            {
                var materialsTerm = await response.Content.ReadAsAsync<List<Material>>();
                for (int i = 0; i < materialsTerm.Count; i++)
                {
                    response.Dispose();
                    response = await client.GetAsync("Victims/" + materialsTerm[i].MaterialId);
                    var victims = await response.Content.ReadAsAsync<List<Victim>>();
                    victims.ForEach(x => materialsTerm[i].Victims.Add(x));
                    response.Dispose();
                    response = await client.GetAsync("Employees/GetEmployeesWhereTermToday/" + materialsTerm[i].MaterialId);
                    var employees = await response.Content.ReadAsAsync<List<Employee>>();
                    employees.ForEach(x => materialsTerm[i].Employees.Add(x));
                    //materialsTerm[i].Victims = await response.Content.ReadAsAsync<List<Victim>>();
                    //materialsTerm[i].Victims = await client.GetAsync("Victims/" + materialsTerm[i].MaterialId);
                }
                 materialsTerm.ForEach(x => Materials.Add(x));
                //Materials. 
            }

        }

        async void GetMaterialsWhereTermTomorrowAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Materials/GetMaterialsWhereTermTomorrow");
            if (response.IsSuccessStatusCode)
            {
                var materialsTerm = await response.Content.ReadAsAsync<List<Material>>();
                for (int i = 0; i < materialsTerm.Count; i++)
                {
                    response.Dispose();
                    response = await client.GetAsync("Victims/" + materialsTerm[i].MaterialId);
                    var victims = await response.Content.ReadAsAsync<List<Victim>>();
                    victims.ForEach(x => materialsTerm[i].Victims.Add(x));
                    response.Dispose();
                    response = await client.GetAsync("Employees/GetEmployeesWhereTermToday/" + materialsTerm[i].MaterialId);
                    var employee = await response.Content.ReadAsAsync<List<Employee>>();
                    employee.ForEach(x => materialsTerm[i].Employees.Add(x));

                }
                materialsTerm.ForEach(x => Materials.Add(x));

            }

        }

    }
}
