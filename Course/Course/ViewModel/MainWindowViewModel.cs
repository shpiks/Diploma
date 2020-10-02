using Course.Commands;
using Course.Context;
using Course.Model;
using Course.View;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Course.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        HttpClient client;
        private Material selectedMaterial;
        private Employee selectedEmployee;

        RelayCommand addMaterialCommand;
        RelayCommand deleteMaterialCommand;
        RelayCommand editMaterialCommand;
        RelayCommand addEmployeeCommand;
        RelayCommand deleteEmployeeCommand;
        RelayCommand editEmployeeCommand;
        RelayCommand lookVictimCommand;
        RelayCommand addVictimCommand;
        RelayCommand lookAllMaterialCommand;
        RelayCommand rewriteMaterialCommand;
        RelayCommand getInfoAboutApp;
        RelayCommand lookTermOnTodayCommand;

        private ObservableCollection<Material> materials;
        private ObservableCollection<Employee> employees;

        public RelayCommand ExitCommand { get; set; }



        public ObservableCollection<Material> Materials
        {
            get { return materials; }

            set
            {
                materials = value;
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged("Employees");
            }
        }

        public Material SelectedMaterial
        {
            get { return selectedMaterial; }
            set
            {
                selectedMaterial = value;
                OnPropertyChanged("SelectedMaterial");


                if (selectedMaterial != null)
                {
                    selectedMaterial.Victims.Clear();
                    try
                    {
                        GetVictimsForMaterialAsync(selectedMaterial.MaterialId);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Ошибка загрузки БД свойстве SelectedMaterial");
                    }
                }
                
            }
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
                materials.Clear();
                if (selectedEmployee != null)
                {
                    try
                    {
                        GetEmployeeMaterialsAsync(selectedEmployee.EmployeeId);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Ошибка загрузки БД свойстве SelectedEmployee");
                    }

                }
            }

        }

        public MainWindowViewModel(HttpClient client)
        { 
            this.client = client;
            this.Materials = new ObservableCollection<Material>();
            //this.Employees = new ObservableCollection<Employee>();
            try
            {
                DispatcherTimer dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler((sender, e) => dispatcherTimer_Tick(sender, e, dispatcherTimer, client));
                dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
                dispatcherTimer.Start();

                //this.Employees = new ObservableCollection<Employee>(GetAllEmployeesAsync(this.client).Result);
                this.Employees = new ObservableCollection<Employee>();
                GetAllEmployeesAsync();
                //GetAllEmployeesAsync();

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки БД в конструкторе ");
            }
        }

        private static void dispatcherTimer_Tick(object sender, EventArgs e, DispatcherTimer dispatcherTimer, HttpClient client)
        {
            ArrayList listMaterials = new ArrayList();
            Logger logger = LogManager.GetCurrentClassLogger();

            try
            {
                GetMaterialsWhereTermTomorrowAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки БД в таймере");
            }

            //if (listMaterials.Count != 0)
            //{
            //    NotificationWindow notificationWindow = new NotificationWindow(db, true, client);
            //    notificationWindow.ShowDialog();
            //}
            //dispatcherTimer.Interval = new TimeSpan(2, 0, 0);

            async void GetMaterialsWhereTermTomorrowAsync()
            {
                HttpResponseMessage response = await client.GetAsync("Materials/GetMaterialsWhereTermTomorrow");
                if (response.IsSuccessStatusCode)
                {
                    var materialsTerm = await response.Content.ReadAsAsync<List<Material>>();

                    materialsTerm.ForEach(x => listMaterials.Add(x));
                    if (listMaterials.Count != 0)
                    {
                        dispatcherTimer.Interval = new TimeSpan(4, 0, 0);
                        NotificationWindow notificationWindow = new NotificationWindow(true, client);
                        notificationWindow.ShowDialog();
                    }
                    dispatcherTimer.Interval = new TimeSpan(2, 0, 0);
                }
            }
        }

        public RelayCommand AddMaterialCommand
        {
            get
            {
                return addMaterialCommand ??
                  (addMaterialCommand = new RelayCommand((o) =>
                  {
                      MaterialWindow materialWindow = new MaterialWindow(null, selectedEmployee, client);
                      materialWindow.ShowDialog();

                      materials.Clear();
                      try
                      {
                          //db.Employees.FirstOrDefault(x => x.EmployeeId == selectedEmployee.EmployeeId).Materials.Where(x => x.ExecutedOrNotExecuted != true).ToList().ForEach(x => materials.Add(x));
                          GetEmployeeMaterialsAsync(selectedEmployee.EmployeeId);
                      }
                      catch (Exception ex)
                      {
                          logger.Error(ex, "Ошибка загрузки БД после добавления материала");
                      }


                  }, (o => SelectedEmployee != null)
                  ));
            }
        }

        public RelayCommand EditMaterialCommand
        {
            get
            {
                return editMaterialCommand ??
                  (editMaterialCommand = new RelayCommand((o) =>
                  {
                      var result = MessageBox.Show("Изменить информацию о материале?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);
                      if (result == MessageBoxResult.Yes)
                      {
                          var material = o as Material;
                          MaterialWindow materialWindow = new MaterialWindow(material, selectedEmployee, client);
                          materialWindow.ShowDialog();

                          materials.Clear();
                          try
                          {
                              //db.Employees.FirstOrDefault(x => x.EmployeeId == selectedEmployee.EmployeeId).Materials.Where(x => x.ExecutedOrNotExecuted != true).ToList().ForEach(x => materials.Add(x));
                              GetEmployeeMaterialsAsync(selectedEmployee.EmployeeId);
                          }
                          catch (Exception ex)
                          {
                              logger.Error(ex, "Ошибка загрузки БД после изменения материала");
                          }


                      }
                  }, (o => SelectedMaterial != null)
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
                          var material = o as Material;
                          if (material != null)
                          {
                              try
                              {
                                  DeleteMaterialAsync(material.MaterialId);

                                  logger.Info("Материал ЕК№" + material.NumberEK + " удален из БД");
                              }
                              catch (Exception ex)
                              {
                                  logger.Error(ex, "Ошибка загрузки БД после удаления материала");
                              }

                              materials.Remove(material);
                          }
                      }
                  }, (o => SelectedMaterial != null))
                  );
            }
        }

        public RelayCommand AddEmployeeCommand
        {
            get
            {
                return addEmployeeCommand ??
                  (addEmployeeCommand = new RelayCommand((o) =>
                  {
                      EmployeeWindow employeeWindow = new EmployeeWindow(null, client);
                      employeeWindow.ShowDialog();
                      try
                      {
                          employees.Clear();
                          GetAllEmployeesAsync();

                      }
                      catch (Exception ex)
                      {
                          logger.Error(ex, "Ошибка загрузки БД после добавления сотрудника");
                      }
                      //MessageBox.Show("Сотрудник добавлен");
                  }));
            }
        }

        public RelayCommand EditEmployeeCommand
        {
            get
            {
                return editEmployeeCommand ??
                  (editEmployeeCommand = new RelayCommand((o) =>
                  {
                      var result = MessageBox.Show("Изменить информацию о сотруднике?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);
                      if (result == MessageBoxResult.Yes)
                      {
                          var employee = o as Employee;
                          EmployeeWindow employeeWindow = new EmployeeWindow(employee, client);
                          employeeWindow.ShowDialog();
                          employees.Clear();
                          GetAllEmployeesAsync();

                      }
                  }, (o => SelectedEmployee != null)
                  ));
            }
        }

        public RelayCommand DeleteEmployeeCommand
        {
            get
            {
                return deleteEmployeeCommand ??
                  (deleteEmployeeCommand = new RelayCommand((o) =>
                  {
                      var result = MessageBox.Show("Удалить сотрудника?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);

                      if (result == MessageBoxResult.Yes)
                      {
                          var employee = o as Employee;
                          if (employee != null)
                          {
                              try
                              {
                                  //db.Employees.Remove(db.Employees.Where(x => x.EmployeeId == employee.EmployeeId).First());
                                  //db.SaveChanges();
                                  DeleteEmployeeAsync(employee.EmployeeId);
                                  logger.Info("Сотрудник " + employee.FirstName + employee.LastName + " удален из БД");
                              }
                              catch (Exception ex)
                              {
                                  logger.Error(ex, "Ошибка загрузки БД после удаления сотрудника");
                              }

                              Employees.Remove(employee);
                          }
                      }
                  }, (o => SelectedEmployee != null))
                  );
            }
        }

        public RelayCommand LookVictimCommand
        {
            get
            {
                return lookVictimCommand ??
                  (lookVictimCommand = new RelayCommand((o) =>
                  {
                      var victim = o as Victim;
                      LookVictimWindow victimWindow = new LookVictimWindow(null, victim, client);
                      victimWindow.ShowDialog();

                  }, (o => SelectedMaterial != null)
                  ));
            }
        }

        public RelayCommand AddVictimCommand 
        {
            get
            {
                return addVictimCommand ??
                  (addVictimCommand = new RelayCommand((o) =>
                  {
                      VictimWindow victimWindow = new VictimWindow(SelectedMaterial, null, client);
                      victimWindow.ShowDialog();


                  }, (o => SelectedMaterial != null)
                  ));
            }
        }

        public RelayCommand LookAllMaterialCommand
        {
            get
            {
                return lookAllMaterialCommand ??
                  (lookAllMaterialCommand = new RelayCommand((o) =>
                  {
                      AllMaterialWindow allMaterialWindow = new AllMaterialWindow(client);
                      allMaterialWindow.ShowDialog();
                  }
                  ));
            }
        }

        public RelayCommand RewriteMaterialCommand
        {
            get
            {
                return rewriteMaterialCommand ??
                  (rewriteMaterialCommand = new RelayCommand((o) =>
                  {
                      RewriteMaterialWindow rewriteMaterialWindow = new RewriteMaterialWindow(SelectedMaterial, SelectedEmployee, client);                     
                      rewriteMaterialWindow.ShowDialog();
                      Materials.Clear();

                      try
                      {
                          //db.Employees.FirstOrDefault(x => x.EmployeeId == selectedEmployee.EmployeeId).Materials.Where(x => x.ExecutedOrNotExecuted != true).ToList().ForEach(x => materials.Add(x));
                          GetEmployeeMaterialsAsync(selectedEmployee.EmployeeId);
                      }
                      catch (Exception ex)
                      {
                          logger.Error(ex, "Ошибка загрузки БД после того как материал был переписан на другого сотрудника");
                      }

                  }, (o => SelectedMaterial != null)
                  ));
            }
        }


        public RelayCommand GetInfoAboutApp
        {
            get
            {
                return getInfoAboutApp ??
                  (getInfoAboutApp = new RelayCommand((o) =>
                  {
                      MessageBox.Show("Developed by Andrew Shpakovskiy");
                  }
                  ));
            }
        }

        public RelayCommand LookTermOnTodayCommand
        {
            get
            {
                return lookTermOnTodayCommand ??
                  (lookTermOnTodayCommand = new RelayCommand((o) =>
                  {
                      NotificationWindow notificationWindow = new NotificationWindow(false, client);
                      notificationWindow.ShowDialog();
                  }
                  ));
            }
        }


        static async Task<List<Employee>> GetAllEmployeesAsync(HttpClient client)
        {

            HttpResponseMessage response = await client.GetAsync("Employees");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Employee>>();
            }
            else
                return null;
        }



        async void GetAllEmployeesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Employees");
            if (response.IsSuccessStatusCode)
            {
                 var employeesTerm = await response.Content.ReadAsAsync<List<Employee>>();
                employeesTerm.ForEach(x => employees.Add(x));
            }

        }


        async void GetEmployeeMaterialsAsync(int id)
        {
            HttpResponseMessage response = await client.GetAsync("Employees/" + id);
            if (response.IsSuccessStatusCode)
            {
                var materialsTerm = await response.Content.ReadAsAsync<List<Material>>();
                materialsTerm.ForEach(x => materials.Add(x));
            }  
        }

        async void DeleteEmployeeAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("Employees/" + id);
        }

        async void DeleteMaterialAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("Materials/" + id);
        }

        async void GetVictimsForMaterialAsync (int id)
        {
            HttpResponseMessage response = await client.GetAsync("Victims/" + id);
            if (response.IsSuccessStatusCode)
            {
                var victimsTerm = await response.Content.ReadAsAsync<List<Victim>>();
                victimsTerm.ForEach(x => selectedMaterial.Victims.Add(x));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
