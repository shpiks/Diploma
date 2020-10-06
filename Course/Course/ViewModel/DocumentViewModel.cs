using Course.Commands;
using Course.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Xps.Packaging;

namespace Course.ViewModel
{
    class DocumentViewModel : INotifyPropertyChanged
    {
        HttpClient client;
        Material material;
        Document selectedDocument;
        Document newDocument;
        OpenFileDialog openFileDialog;
        SaveFileDialog saveFileDialog;

        RelayCommand overviewDocumentCommand;
        RelayCommand addDocumentCommand;
        RelayCommand saveDocumentCommand;
        RelayCommand deleteDocumentCommand;

        private ObservableCollection<Document> documents;

        public ObservableCollection<Document> Documents
        {
            get { return documents; }

            set
            {
                documents = value;
            }
        }

        public Document SelectedDocument { get => selectedDocument; set => selectedDocument = value; }
        public Document NewDocument 
        {
            get { return newDocument; }
            set
            {
                newDocument = value;
                OnPropertyChanged("NewDocument");
            }
        }

        private string fileName;
        public string FileName { get => fileName; set => fileName = value; }


        public DocumentViewModel(Material material, HttpClient client)
        {
            this.client = client;
            this.material = material;
            this.Documents = new ObservableCollection<Document>();
            NewDocument = new Document()
            {
                MaterialsMaterialId = material.MaterialId
                //Materials = material
            };
            GetDocumentAsync(material);

        }

        public RelayCommand OverviewDocumentCommand
        {
            get
            {
                return overviewDocumentCommand ??
                  (overviewDocumentCommand = new RelayCommand((o) =>
                  {
                      try
                      {
                          openFileDialog = new OpenFileDialog();
                          openFileDialog.InitialDirectory = "E:\\";
                          openFileDialog.Filter = "Word Documents|*.doc";
                          openFileDialog.CheckFileExists = true;
                          openFileDialog.Multiselect = true;
                          openFileDialog.ShowDialog();

                          NewDocument.FileName = openFileDialog.FileName;

                      }
                      catch (Exception ex)
                      {
                          //logger.Error(ex, "Ошибка загрузки БД после добавления материала");
                      }

                  }/*, (o => NewDocument.Title != null)*/
                  ));
            }
        }


        public RelayCommand AddDocumentCommand
        {
            get
            {
                return addDocumentCommand ??
                  (addDocumentCommand = new RelayCommand((o) =>
                  {
                      try
                      {
                          using (FileStream fs = new FileStream(NewDocument.FileName, FileMode.Open))
                          {
                              NewDocument.DocumentData = new byte[fs.Length];
                              fs.Read(NewDocument.DocumentData, 0, NewDocument.DocumentData.Length);
                          }
                          CreateDocumentAsync(NewDocument);
                          Documents.Clear();

                          

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show("Путь к файлу веден неверно, попробуйте еще раз");
                          //logger.Error(ex, "Ошибка загрузки БД после добавления материала");
                      }
                     
                  }, (o => NewDocument.Title != null && NewDocument.FileName != null)
                  ));
            }
        }

        public RelayCommand SaveDocumentCommand
        {
            get
            {
                return saveDocumentCommand ??
                  (saveDocumentCommand = new RelayCommand((o) =>
                  {
                      try
                      {
                          saveFileDialog = new SaveFileDialog();
                          saveFileDialog.InitialDirectory = "E:\\";
                          saveFileDialog.Filter = "Word Documents|*.doc";
                          saveFileDialog.OverwritePrompt = true;
                          saveFileDialog.Title = selectedDocument.Title;
                          saveFileDialog.ShowDialog();
                          using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate))
                          {
                              fs.Write(selectedDocument.DocumentData, 0, selectedDocument.DocumentData.Length);
                              //Console.WriteLine("Изображение '{0}' сохранено", images[0].Title);
                          }
                      }
                      catch (Exception ex)
                      {
                          //logger.Error(ex, "Ошибка загрузки БД после добавления материала");
                      }

                  }, (o => SelectedDocument != null)
                  ));
            }
        }

        public RelayCommand DeleteDocumentCommand
        {
            get
            {
                return deleteDocumentCommand ??
                  (deleteDocumentCommand = new RelayCommand((o) =>
                  {
                      var result = MessageBox.Show("Удалить " +selectedDocument.Title + " ?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);

                      if (result == MessageBoxResult.Yes)
                      {
                          //var document = o as Document;
                          if (selectedDocument != null)
                          {
                              try
                              {
                                  DeleteDocumentAsync(selectedDocument.DocumentId);
                                  //logger.Info("Сотрудник " + employee.FirstName + employee.LastName + " удален из БД");
                              }
                              catch (Exception ex)
                              {
                                  //logger.Error(ex, "Ошибка загрузки БД после удаления сотрудника");
                              }

                              Documents.Remove(selectedDocument);
                          }
                      }
                  }, (o => SelectedDocument != null))
                  );
            }
        }

        async void CreateDocumentAsync(Document document)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("Documents/", document);
            response.EnsureSuccessStatusCode();
            NewDocument.FileName = null;
            NewDocument.Title = null;
            GetDocumentAsync(material);
        }

        async void GetDocumentAsync(Material material)
        {
            HttpResponseMessage response = await client.GetAsync("Documents/" + material.MaterialId);
            if (response.IsSuccessStatusCode)
            {
                var documentTerm = await response.Content.ReadAsAsync<List<Document>>();
                documentTerm.ForEach(x => documents.Add(x));
            }
        }

        async void DeleteDocumentAsync(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync("Documents/" + id);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
