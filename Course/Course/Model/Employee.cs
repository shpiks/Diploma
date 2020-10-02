using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace Course.Model
{
    public class Employee : INotifyPropertyChanged
    {
        private int employeeId { get; set; }
        private string firstName { get; set; } // Имя
        private string lastName { get; set; } // Фамилия
        private string patronymic { get; set; } // Отчество
        private string rank { get; set; } // Звание
        private int numberMaterialsOnPerformance { get; set; } // Материалов на исполнении
        private int numberMaterialsPerformed { get; set; } // Исполнено материалов
        private string position { get; set; } // Должность

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            this.Materials = new HashSet<Material>();
        }

        public int EmployeeId
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                OnPropertyChanged("EmployeesId");
            }
        }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        } // Имя
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        } // Фамилия
        public string Patronymic
        {
            get { return patronymic; }
            set
            {
                patronymic = value;
                OnPropertyChanged("Patronymic");
            }
        } // Отчество
        public string Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
            }
        } // Звание
        public int NumberMaterialsOnPerformance
        {
            get { return numberMaterialsOnPerformance; }
            set
            {
                numberMaterialsOnPerformance = value;
                OnPropertyChanged("NumberMaterialsOnPerformance");
            }
        } // Материалов на исполнении
        public int NumberMaterialsPerformed
        {
            get { return numberMaterialsPerformed; }
            set
            {
                numberMaterialsPerformed = value;
                OnPropertyChanged("NumberMaterialsPerformed");
            }
        } // Исполнено материалов

        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        } // Должность

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Material> Materials { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
