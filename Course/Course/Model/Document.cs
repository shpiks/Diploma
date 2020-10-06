using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Course.Model
{
    public class Document : INotifyPropertyChanged
    {
        public int DocumentId { get; set; }
        private string fileName { get; set; }
        private string title { get; set; }
        public byte[] DocumentData { get; set; }

        public int MaterialsMaterialId { get; set; }
        //public Material Materials { get; set; }

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
