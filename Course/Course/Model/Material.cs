using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace Course.Model
{
   public class Material : INotifyPropertyChanged
    {
        private int materialId;
        private int numberEK; // номер Единой Книги
        private string story; // фабула
        private DateTime? dateOfRegistration;// дата регистрации 
        private DateTime? dateOfTerm; // срок
        private bool extension; //продленный или не продленный
        private string decision; // решение
        private bool executedOrNotExecuted;//исполнен или не исполнен
        private string perspective; //перспектива


        //private ICollection<Victim> victims;

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material()
        {
            //this.Employees = new HashSet<Employee>();
            //this.Victims = new HashSet<Victim>();
            this.Victims = new ObservableCollection<Victim>();
            this.Employees = new ObservableCollection<Employee>();
        }

        public int MaterialId
        {
            get { return materialId; }
            set
            {
                materialId = value;
                OnPropertyChanged("MaterialId");
            }
        }
        public int NumberEK
        {
            get { return numberEK; }
            set
            {
                numberEK = value;
                OnPropertyChanged("NumberEK");
            }
        } // номер Единой Книги
        public string Story
        {
            get { return story; }
            set
            {
                story = value;
                OnPropertyChanged("Story");
            }
        } // фабула
        public DateTime? DateOfRegistration
        {
            get { return dateOfRegistration; }
            set
            {
                dateOfRegistration = value;
                OnPropertyChanged("DateOfRegistration");
            }
        } // дата регистрации 
        public DateTime? DateOfTerm
        {
            get { return dateOfTerm; }
            set
            {
                dateOfTerm = value;
                OnPropertyChanged("DateOfTerm");
            }
        } // срок
        public bool Extension
        {
            get { return extension; }
            set
            {
                extension = value;
                OnPropertyChanged("Extension");
            }
        } //продленный или не продленный
        public string Decision
        {
            get { return decision; }
            set
            {
                decision = value;
                OnPropertyChanged("Decision");
            }
        } // решение
        public bool ExecutedOrNotExecuted
        {
            get { return executedOrNotExecuted; }
            set
            {
                executedOrNotExecuted = value;
                OnPropertyChanged("ExecutedOrNotExecuted");
            }
        } //исполнен или не исполнен

        public string Perspective
        {
            get { return perspective; }
            set
            {
                perspective = value;
                OnPropertyChanged("Perspective");
            }
        } // перспектива


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Employee> Employees { get; set; }
        public virtual ObservableCollection<Employee> Employees { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ObservableCollection<Victim> Victims { get; set; }

        //public virtual ICollection<Victim> Victims
        //{
        //    get { return victims; }
        //    set
        //    {
        //        victims = value;
        //        OnPropertyChanged("Victims");
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    }
