using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive;
using ReactiveUI;
using CoreBackup.Models.IO;
using DynamicData;
using System.Collections.ObjectModel;
using DynamicData.Binding;
using Avalonia;
using System.Linq;
using System.IO;

namespace CoreBackup.ViewModels
{
    class TasksViewModel : ViewModelBase
    {

        // https://reactiveui.net/docs/handbook/collections/

        private readonly ReadOnlyObservableCollection<string> _derivedLocalFiles;
        public ReadOnlyObservableCollection<string> DerivedLocalFiles => _derivedLocalFiles;

        private readonly ReadOnlyObservableCollection<FileInformation> _derivedRemoteFiles;
        public ReadOnlyObservableCollection<FileInformation> DerivedRemoteFiles => _derivedRemoteFiles;

        public ObservableCollectionExtended<string> LocalFiles { get; }
        public ObservableCollectionExtended<FileInformation> RemoteFiles { get; }

        public ReactiveCommand<Unit, Unit> SendToRemoteCommand { get; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }


        private string tekst;

        public string Tekst
        {
            get => tekst;
            set => this.RaiseAndSetIfChanged(ref tekst, value);
        }

        public TasksViewModel()
        {
            tekst = "TESTOWY";
            SendToRemoteCommand = ReactiveCommand.Create(SendToRemote);
            PauseCommand = ReactiveCommand.Create(Pause);

            LocalFiles = new ObservableCollectionExtended<string>();
            LocalFiles.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _derivedLocalFiles)
            .Subscribe();
            // Update the source collection and the derived
            // collection will update as well.

            RemoteFiles = new ObservableCollectionExtended<FileInformation>();
            RemoteFiles.ToObservableChangeSet()
           .Transform(value => value)
           // No need to use the .ObserveOn() operator here, as
           // ObservableCollectionExtended is single-threaded.
           .Bind(out _derivedRemoteFiles)
           .Subscribe();


            BasicIO bIO = new BasicIO();
            LocalFiles.Where(l => l.Length != 0).ToList().All(i => LocalFiles.Remove(i));
            foreach (FileInfo f in bIO.getFilesInDirectory("C:\\"))
            {
                LocalFiles.Add(f.ToString());
                FileInformation fi = new FileInformation();
                fi.Filename = f.Name;
                fi.Size = f.Length;
                RemoteFiles.Add(fi);
                Tekst = "zmieniony";
            }
        }


        private void SendToRemote()
        {
            BasicIO bIO = new BasicIO();
            LocalFiles.Where(l => l.Length != 0).ToList().All(i => LocalFiles.Remove(i));
            foreach (FileInfo f in bIO.getFilesInDirectory("C:\\"))
            {
                LocalFiles.Add(f.ToString());
                FileInformation fi = new FileInformation();
                fi.Filename = f.Name;
                fi.Size = f.Length;
                RemoteFiles.Add(fi);
                Tekst = "zmieniony";
            }
        }

        private void Pause()
        {
            LocalFiles.Add("test");
        }
    }
}
