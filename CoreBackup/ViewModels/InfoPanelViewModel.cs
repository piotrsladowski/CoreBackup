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
            foreach (KeyValuePair<string, ConfigHub> entry in CoreTask.tasksList)
            {
                if(!InfoConfigsList.Any(u => u.Name == entry.Key))
                {
                    InfoConfig infoConfig = new InfoConfig(ID, entry.Key, entry.Value.isActive);
                    infoConfig.DictionaryPropertyChanged += OnDictionaryPropertyChanged;
                    InfoConfigsList.Add(infoConfig);
                }
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

        private void UpdateDictionary(string confName)
        {
            ConfigHub confValue = CoreTask.tasksList[confName];
            CoreTask.tasksList.Remove(confName);
            CoreTask.AddTaskEntry(name, confValue);
            OnDictionaryPropertyChanged();
        }

        private void UpdateDictionary(bool status)
        {
            CoreTask.tasksList[name].isActive = status;
            OnDictionaryPropertyChanged();
        }

        private void RemoveConfiguration(string name)
        {
            Debug.WriteLine(name);
            OnDictionaryPropertyChanged();
        }
    }
}
