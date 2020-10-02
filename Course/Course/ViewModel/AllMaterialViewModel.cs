using Course.Commands;
using Course.Context;
using Course.Model;
using Course.View;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Course.ViewModel
{
    class AllMaterialViewModel
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        RelayCommand filterMaterialsCommand;
        RelayCommand showMaterialCommand;
        RelayCommand showVictimCommand;
        RelayCommand deleteMaterialCommand;

        HttpClient client;
        private Employee selectedEmployee = new Employee() { LastName = " " };
        private string selectedDecision;
        private Material selectedMaterial;
        private Victim selectedVictim;


        private ObservableCollection<Material> materials;
        private ObservableCollection<Employee> employees;
        public List<string> DecisionList { get; set; }

        public DateTime StartData { get; set; }
        public DateTime FinishData { get; set; }

        public Victim VictimToggle { get; set; }

        public string SelectedDecision
        {
            get
            {
                if (selectedDecision == " ")
                    return null;
                else
                    return selectedDecision;
            }

            set
            {
                selectedDecision = value;
                OnPropertyChanged("SelectedDecision");
            }
        }

        public ObservableCollection<Material> Materials
        {
            get { return materials; }

            set
            {
                materials = value;
                OnPropertyChanged("Materials");
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                
            }
        }

        public Employee SelectedEmployee
        {
            //get { return selectedEmployee; }

            get
            {
                if (selectedEmployee.LastName == " ")
                    return null;
                else
                    return selectedEmployee;
            }
            set
            {
                selectedEmployee = value;
            }
        }

        public Material SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                selectedMaterial = value;
                OnPropertyChanged("SelectedMaterial");
                if (value != null)
                VictimToggle = SelectedMaterial.Victims.FirstOrDefault();
            }
        }

        public Victim SelectedVictim
        {
            get { return selectedVictim; }
            set
            {
                selectedVictim = value;
                OnPropertyChanged("SelectedVictim");

            }
        }

        public AllMaterialViewModel(HttpClient client)
        {
            this.client = client;
            Materials = new ObservableCollection<Material>();
            Employees = new ObservableCollection<Employee>();
            try
            {
                GetMaterialsWhereNotExecutedAsync();
                GetAllEmployeesAsync();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки БД в конструкторе ");
            }

            DecisionList = new List<string> {" ", "Отказано в ВУД", "ВУД(факт)", "ВУД(лицо)", "Передано по территориальности",
                "Передано в др. службу", "Списано в дело" };


            StartData = DateTime.Today;
            FinishData = DateTime.Today;
        }

        public RelayCommand FilterMaterialsCommand
        {
            get
            {
                return filterMaterialsCommand ??
                  (filterMaterialsCommand = new RelayCommand((o) =>
                  {
                      Materials.Clear();
                      try
                      {
                          if (SelectedEmployee != null && SelectedDecision != null)
                          {
                              GetMaterialsByFilterSelectedEmployeeAndSelectedDecision(StartData, FinishData, SelectedEmployee.EmployeeId, SelectedDecision);
                          }
                          else if (SelectedEmployee == null && SelectedDecision != null)
                          {
                              GetMaterialsByFilterSelectedDecision(StartData, FinishData, SelectedDecision);
                          }
                          else if (SelectedEmployee != null && SelectedDecision == null)
                          {
                              GetMaterialsByFilterEmployeeId(StartData, FinishData, SelectedEmployee.EmployeeId);
                          }
                          else
                          {
                              GetMaterialsByFilterDates(StartData, FinishData);
                          }
                      }
                      catch (Exception ex)
                      {
                          logger.Error(ex, "Ошибка при фильрации данных из БД");
                      }

                  }
                  ));
            }
        }

        public RelayCommand ShowMaterialCommand
        {
            get
            {
                return showMaterialCommand ??
                  (showMaterialCommand = new RelayCommand((o) =>
                  {
                      LookMaterialWindow lookMaterialWindow = new LookMaterialWindow();
                      lookMaterialWindow.DataContext = SelectedMaterial;
                      lookMaterialWindow.ShowDialog();
                  }, (o => SelectedMaterial != null)
                  ));
            }
        }

        public RelayCommand ShowVictimCommand
        {
            get
            {
                return showVictimCommand ??
                  (showVictimCommand = new RelayCommand((o) =>
                  {
                      try
                      {
                          SelectedVictim = SelectedMaterial.Victims.FirstOrDefault();
                      }
                      catch (Exception ex)
                      {
                          logger.Error(ex, "Ошибка c БД в ShowVictimCommand");
                      }

                      LookVictimWindow lookVictimWindow = new LookVictimWindow(null, SelectedVictim, client);
                      lookVictimWindow.ShowDialog();
                  }, (o => VictimToggle != null)
                  ));
            }
        }

        public RelayCommand DeleteMaterialCommand
        {
            get
            {
                return deleteMaterialCommand ??
                  (deleteMaterialCommand = new RelayCommand((o) =>
                  {
                      var result = MessageBox.Show("Удалить материал?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);

                      if (result == MessageBoxResult.Yes)
                      {
                          try
                          {
                              //db.Materials.Remove(db.Materials.Where(x => x.MaterialId == SelectedMaterial.MaterialId).First());
                              //db.SaveChanges();
                              DeleteMaterialAsync(SelectedMaterial.MaterialId);
                              logger.Info("Материал ЕК№" + SelectedMaterial.NumberEK + " удален из БД");
                          }
                          catch (Exception ex)
                          {
                              logger.Error(ex, "Ошибка c удалением материала в БД ");
                          }
                          materials.Remove(SelectedMaterial);
                      }

                      }, (o => SelectedMaterial != null)
                      
                  ));
            }
        }

        async void GetMaterialsWhereNotExecutedAsync()
        {
            HttpResponseMessage response = await client.GetAsync("AllMaterials/");
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

        
        async void GetMaterialsByFilterSelectedEmployeeAndSelectedDecision(DateTime StartData, DateTime FinishData, int EmployeeId, string SelectedDecision)
        {
            string stringJson = JsonConvert.SerializeObject(StartData) + "|" + JsonConvert.SerializeObject(FinishData) + "|" + JsonConvert.SerializeObject(EmployeeId) + "|" + JsonConvert.SerializeObject(SelectedDecision);
            HttpResponseMessage response = await client.PutAsJsonAsync("AllMaterials/GetMaterialsByFilterSelectedEmployeeAndSelectedDecision/", stringJson);
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

        async void GetMaterialsByFilterSelectedDecision(DateTime StartData, DateTime FinishData,  string SelectedDecision)
        {
            string stringJson = JsonConvert.SerializeObject(StartData) + "|" + JsonConvert.SerializeObject(FinishData) + "|" + JsonConvert.SerializeObject(SelectedDecision);
            HttpResponseMessage response = await client.PutAsJsonAsync("AllMaterials/GetMaterialsByFilterSelectedDecision/", stringJson);
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

        async void GetMaterialsByFilterEmployeeId(DateTime StartData, DateTime FinishData, int EmployeeId)
        {
            string stringJson = JsonConvert.SerializeObject(StartData) + "|" + JsonConvert.SerializeObject(FinishData) + "|" + JsonConvert.SerializeObject(EmployeeId);
            HttpResponseMessage response = await client.PutAsJsonAsync("AllMaterials/GetMaterialsByFilterEmployeeId/", stringJson);
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

        async void GetMaterialsByFilterDates(DateTime StartData, DateTime FinishData)
        {
            string stringJson = JsonConvert.SerializeObject(StartData) + "|" + JsonConvert.SerializeObject(FinishData);
            HttpResponseMessage response = await client.PutAsJsonAsync("AllMaterials/GetMaterialsByFilterDates/", stringJson);
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

        async void GetAllEmployeesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Employees");
            if (response.IsSuccessStatusCode)
            {
                var employeesTerm = await response.Content.ReadAsAsync<List<Employee>>();
                employees.Add(new Employee() { LastName = " " });
                employeesTerm.ForEach(x => employees.Add(x));
            }

        }

        async void DeleteMaterialAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("Materials/" + id);
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
