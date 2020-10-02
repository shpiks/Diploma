using Course.Commands;
using Course.Context;
using Course.Model;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Course.ViewModel
{
    class RewriteMaterialViewModelcs : INotifyPropertyChanged
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        HttpClient client;
        private Employee selectedEmployee;
        private Material material;
        private Employee oldEmployee;

        private ObservableCollection<Employee> employees;

        RelayCommand rewriteMaterialCommand;
        public RelayCommand ExitCommand { get; set; }



        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged("Employees");
            }
        }

        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                //OnPropertyChanged("SelectedMaterial");
            }
        }

        public RewriteMaterialViewModelcs(Material material, Employee employee, HttpClient client)
        {
            this.Employees = new ObservableCollection<Employee>();
            this.client = client;
            this.material = material;

            oldEmployee = employee;

            try
            {
                GetAllEmployeesAsync();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                logger.Error(exc, "Ошибка с загрузкой данных из БД в конструкторе");
            }
            //Employees.Remove(oldEmployee);
        }

        public RelayCommand RewriteMaterialCommand
        {
            get
            {
                return rewriteMaterialCommand ??
                  (rewriteMaterialCommand = new RelayCommand((o) =>
                  {
                      try
                      {
                          UpdateEmployeeAsync(selectedEmployee, material);

                          MessageBox.Show("Материал ЕК№" + material.NumberEK + " переписан на " + SelectedEmployee.LastName);
                          logger.Info("Материал ЕК№" + material.NumberEK + " переписан на " + SelectedEmployee.LastName);
                          ExitCommand.Execute();
                      }
                      catch (Exception exc)
                      {
                          MessageBox.Show(exc.Message);
                          logger.Error(exc, "Ошибка в комманде RewriteMaterialCommand");
                      }
                  }, (o => SelectedEmployee != null)
                  ));
            }
        }

        async void GetAllEmployeesAsync()
        {
            HttpResponseMessage response = await client.GetAsync("Employees");
            if (response.IsSuccessStatusCode)
            {
                var employeesTerm = await response.Content.ReadAsAsync<List<Employee>>();
                var deleteEmployee = employeesTerm.FirstOrDefault(x => x.EmployeeId == oldEmployee.EmployeeId);

                employeesTerm.Remove(deleteEmployee); 
                employeesTerm.ForEach(x => employees.Add(x));
                
            }

        }

        async void UpdateEmployeeAsync(Employee employee, Material material)
        {
            string stringJson = JsonConvert.SerializeObject(material) + "|" + JsonConvert.SerializeObject(employee);
            HttpResponseMessage response = await client.PutAsJsonAsync("Employees/RewriteEmployees", stringJson);
            response.EnsureSuccessStatusCode();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
