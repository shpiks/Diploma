using Course.Commands;
using Course.Context;
using Course.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Course.ViewModel
{
    class EmployeeViewModel : INotifyPropertyChanged
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        HttpClient client;
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand AcceptCommand { get; private set; }        
        public List<string> PositionList { get; set; }
        public List<string> RankList { get; set; }

        public Employee Employee { get; private set; }

        public EmployeeViewModel(Employee employee, HttpClient client)
        {
            
            this.client = client;
            PositionList = new List<string> {"оперуполномоченный","ст. оперуполномоченный", "ст. оперуполномоченный по ОВД" };
            RankList = new List<string> {"мл.лейтенант","лейтенант", "ст.лейтенант", "капитан", "майор",
                "подполковник" };

            if (employee == null)
            {
                Employee = new Employee()
                {                    
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Patronymic = "Иванович",
                    Rank = "",
                    Position = ""
                };
                AcceptCommand = new RelayCommand(AddCommand);
            }
            else
            {
                Employee = new Employee()
                {
                    EmployeeId = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Patronymic = employee.Patronymic,
                    Rank = employee.Rank,
                    Position = employee.Position
                };
                AcceptCommand = new RelayCommand(EditCommand);
            }
        }

        private void AddCommand(object obj)
        {

            Employee employee = new Employee()
            {
                FirstName = Employee.FirstName,
                LastName = Employee.LastName,
                Patronymic = Employee.Patronymic,
                Rank = Employee.Rank,
                Position = Employee.Position


            };
            try
            {

                CreateEmployeeAsync(employee);

                logger.Info("Сотрудник " + employee.FirstName + " " + employee.LastName + " добавлен в БД");
                MessageBox.Show("Сотрудник добавлен");
                
                ExitCommand.Execute();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                logger.Error(exc, "Ошибка c добавлением сотрудника в БД ");
            }
        }

        private void EditCommand(object obj)
        {

            try
            {
                UpdateEmployeeAsync(Employee);

                logger.Info("Данные сотрудника изменены на " + Employee.FirstName + " " + Employee.LastName);
                MessageBox.Show("Данные сотрудника изменены");
                ExitCommand.Execute();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                logger.Error(exc, "Ошибка c изменением данных сотрудника");
            }

        }

        async void CreateEmployeeAsync(Employee employee)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("Employees/", employee);
            response.EnsureSuccessStatusCode();
        }

        async void UpdateEmployeeAsync(Employee employee)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync("Employees/" +employee.EmployeeId, employee);
            response.EnsureSuccessStatusCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
