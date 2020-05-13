using CoreBackup.Models.Config;
using CoreBackup.Models.Tasks;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;

namespace CoreBackup.ViewModels
{
    class InfoPanelViewModel : ViewModelBase
    {
        private int ID;

        private ObservableCollection<InfoConfig> infoConfigsList;

        public ObservableCollection<InfoConfig> InfoConfigsList
        {
            get => infoConfigsList;
            set => this.RaiseAndSetIfChanged(ref infoConfigsList, value);
        }

        public InfoPanelViewModel()
        {
            ID = 0;
            InfoConfigsList = new ObservableCollection<InfoConfig>();
        }

        public void OnSavedConfigurationNotified(object o, EventArgs e)
        {
            Debug.WriteLine("OnSavedConfiguration event successfully raised from Infopanel");
            UpdateInfoConfigsList();
        }

        public void OnDictionaryPropertyChanged(object o, EventArgs e)
        {
            Debug.WriteLine("OnDictionaryPropertyChanged event successfully raised");
            UpdateInfoConfigsList();
        }

        private void UpdateInfoConfigsList()
        {
            ID = 1;
            InfoConfigsList.Clear();
            foreach (KeyValuePair<string, ConfigHub> entry in CoreTask.tasksList)
            {
                InfoConfig infoConfig = new InfoConfig(ID, entry.Key, entry.Value.IsActive);
                infoConfig.DictionaryPropertyChanged += OnDictionaryPropertyChanged;
                InfoConfigsList.Add(infoConfig);
                ID++;
            }
        }
    }

    class InfoConfig
    {
        public ReactiveCommand<string, Unit> RemoveConfigurationCommand { get; }

        public InfoConfig(int id, string name, bool isActive)
        {
            RemoveConfigurationCommand = ReactiveCommand.Create<string>(RemoveConfiguration);
            ID = id;
            this.name = name;
            this.isActive = isActive;
        }

        private string name;
        public string Name
        {
            get {
                return name;
            }
            set {
                var oldName = name;
                name = value;
                UpdateDictionary(oldName);
            }
        }

        private bool isActive;
        public bool IsActive
        {
            get {
                return isActive;
            }
            set {
                var oldIsActive = isActive;
                isActive = value;
                UpdateDictionary(oldIsActive);
            }
        } 
        public int ID { get; set; }

        public event EventHandler<EventArgs> DictionaryPropertyChanged;

        protected virtual void OnDictionaryPropertyChanged()
        {
            DictionaryPropertyChanged(this, EventArgs.Empty);
        }

        private void UpdateDictionary(string oldConfName)
        {
            CoreTask.UpdateTasksList(oldConfName, name);
        }

        private void UpdateDictionary(bool status)
        {
            CoreTask.UpdateTasksList(name, status);
        }

        private void RemoveConfiguration(string name)
        {
            CoreTask.RemoveTaskEntry(name);
            OnDictionaryPropertyChanged();
        }
    }
}
