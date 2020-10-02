using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Model
{
    public class Victim : INotifyPropertyChanged
    {
        private int victimId { get; set; }
        private string firstName { get; set; } // Имя
        private string lastName { get; set; } // Фамилия
        private string patronymic { get; set; } // Отчество
        private string phoneNumber { get; set; } //Номер телефона
        private string city { get; set; } // Город
        private string street { get; set; } // Улица
        private string home { get; set; } // Дом
        private string corps { get; set; } // Корпус
        private string flat { get; set; } //Квартира
        private DateTime dateOfBirth { get; set; } // Дата рождения

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Victim()
        {
            this.Materials = new HashSet<Material>();
        }

        public int VictimId
        {
            get { return victimId; }
            set
            {
                victimId = value;
                OnPropertyChanged("VictimId");
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
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        } //Номер телефона
        public string City
        {
            get { return city; }
            set
            {
                city = value;
                OnPropertyChanged("City");
            }
        } // Город
        public string Street
        {
            get { return street; }
            set
            {
                street = value;
                OnPropertyChanged("Street");
            }
        } // Улица
        public string Home
        {
            get { return home; }
            set
            {
                home = value;
                OnPropertyChanged("Home");
            }
        } // Дом

        public string Corps
        {
            get { return corps; }
            set
            {
                corps = value;
                OnPropertyChanged("Corps");
            }
        } // Корпус
        
        public string Flat
        {
            get { return flat; }
            set
            {
                flat = value;
                OnPropertyChanged("Flat");
            }
        } //Квартира
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
            }
        } // Дата рождения

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
