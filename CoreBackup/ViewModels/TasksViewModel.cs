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
using CoreBackup.Models.Tasks;
using CoreBackup.Models.Config;
using System.Diagnostics;

namespace CoreBackup.ViewModels
{
    class TasksViewModel : ViewModelBase
    {

        // https://reactiveui.net/docs/handbook/collections/

        private readonly ReadOnlyObservableCollection<FileInformation> _derivedLocalFiles;
        public ReadOnlyObservableCollection<FileInformation> DerivedLocalFiles => _derivedLocalFiles;

        private readonly ReadOnlyObservableCollection<FileInformation> _derivedRemoteFiles;
        public ReadOnlyObservableCollection<FileInformation> DerivedRemoteFiles => _derivedRemoteFiles;

        public ObservableCollectionExtended<FileInformation> LocalFiles { get; }
        public ObservableCollectionExtended<FileInformation> RemoteFiles { get; }


        public ReactiveCommand<Unit, Unit> SyncToLocalCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncToLocalOverrideCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncMirrorCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncToRemoteOverrideCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncToRemoteCommand { get; }


        private SyncActions syncActions;

        public TasksViewModel()
        {
            SyncToLocalCommand = ReactiveCommand.Create(SyncToLocal);
            SyncToLocalOverrideCommand = ReactiveCommand.Create(SyncToLocalOverride);
            SyncMirrorCommand = ReactiveCommand.Create(SyncMirror);
            SyncToRemoteCommand = ReactiveCommand.Create(SyncToRemote);
            SyncToRemoteOverrideCommand = ReactiveCommand.Create(SyncToRemoteOverride);

            LocalFiles = new ObservableCollectionExtended<FileInformation>();
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

            syncActions = new SyncActions();
            /*
            BasicIO bIO = new BasicIO();
            LocalFiles.Where(l => l.Size != 0).ToList().All(i => LocalFiles.Remove(i));
            foreach (FileInfo f in bIO.getFilesInDirectory("C:\\"))
            {
                //LocalFiles.Add(f.ToString());
                FileInformation fi = new FileInformation();
                fi.Filename = f.Name;
                fi.Size = f.Length;
                RemoteFiles.Add(fi);
                LocalFiles.Add(fi);
            }*/
        }


        private void SyncMirror()
        {
            BasicIO bIO = new BasicIO();
            LocalFiles.Where(l => l.Size != 0).ToList().All(i => LocalFiles.Remove(i)); //Clear LocalFiles
            RemoteFiles.Where(l => l.Size != 0).ToList().All(i => RemoteFiles.Remove(i));
            foreach (FileInfo f in bIO.getFilesInDirectory("C:\\"))
            {
                Debug.WriteLine(f.ToString());
                //LocalFiles.Add(f.ToString());
                FileInformation fi = new FileInformation();
                fi.Filename = f.Name;
                fi.Size = f.Length;
                RemoteFiles.Add(fi);
                LocalFiles.Add(fi);
            }
        }

        private void SyncToLocal()
        {

        }
        private void SyncToLocalOverride()
        {

        }
        private void SyncToRemote()
        {
        }
        private void SyncToRemoteOverride()
        {
        }
    }
}
