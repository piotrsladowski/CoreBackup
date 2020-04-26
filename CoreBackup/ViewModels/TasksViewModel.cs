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

        private readonly ReadOnlyObservableCollection<FileInformation> _derivedLeftFiles;
        public ReadOnlyObservableCollection<FileInformation> DerivedLeftFiles => _derivedLeftFiles;

        private readonly ReadOnlyObservableCollection<FileInformation> _derivedRightFiles;
        public ReadOnlyObservableCollection<FileInformation> DerivedRightFiles => _derivedRightFiles;

        public ObservableCollectionExtended<FileInformation> LeftFiles { get; }
        public ObservableCollectionExtended<FileInformation> RightFiles { get; }


        public ReactiveCommand<Unit, Unit> SyncToLeftCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncToLeftOverrideCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncMirrorCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncToRightOverrideCommand { get; }
        public ReactiveCommand<Unit, Unit> SyncToRightCommand { get; }



        public TasksViewModel()
        {
            SyncToLeftCommand = ReactiveCommand.Create(SyncToLeft);
            SyncToLeftOverrideCommand = ReactiveCommand.Create(SyncToLeftOverride);
            SyncMirrorCommand = ReactiveCommand.Create(SyncMirror);
            SyncToRightCommand = ReactiveCommand.Create(SyncToRight);
            SyncToRightOverrideCommand = ReactiveCommand.Create(SyncToRightOverride);

            LeftFiles = new ObservableCollectionExtended<FileInformation>();
            LeftFiles.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _derivedLeftFiles)
            .Subscribe();
            // Update the source collection and the derived
            // collection will update as well.

            RightFiles = new ObservableCollectionExtended<FileInformation>();
            RightFiles.ToObservableChangeSet()
           .Transform(value => value)
           // No need to use the .ObserveOn() operator here, as
           // ObservableCollectionExtended is single-threaded.
           .Bind(out _derivedRightFiles)
           .Subscribe();
            /*
            BasicIO bIO = new BasicIO();
            LeftFiles.Where(l => l.Size != 0).ToList().All(i => LeftFiles.Remove(i));
            foreach (FileInfo f in bIO.getFilesInDirectory("C:\\"))
            {
                //LeftFiles.Add(f.ToString());
                FileInformation fi = new FileInformation();
                fi.Filename = f.Name;
                fi.Size = f.Length;
                RightFiles.Add(fi);
                LeftFiles.Add(fi);
            }*/
        }


        public void OnOpenedTasksView(object o, EventArgs e)
        {
            Debug.WriteLine("OnOpenedTasksView event successfully raised");
            GetAllFiles();
        }
    
        private void GetAllFiles()
        {

        }

        private void SyncMirror()
        {
            BasicIO bIO = new BasicIO();
            LeftFiles.Where(l => l.Size != 0).ToList().All(i => LeftFiles.Remove(i)); //Clear LeftFiles
            RightFiles.Where(l => l.Size != 0).ToList().All(i => RightFiles.Remove(i));
            foreach (FileInfo f in bIO.getFilesInDirectory("C:\\"))
            {
                Debug.WriteLine(f.ToString());
                //LeftFiles.Add(f.ToString());
                FileInformation fi = new FileInformation();
                fi.Filename = f.Name;
                fi.Size = f.Length;
                RightFiles.Add(fi);
                LeftFiles.Add(fi);
            }
        }

        private void SyncToLeft()
        {
            /*
            Debug.WriteLine("test");
            FTPConfig ftpconf = new FTPConfig();
            ftpconf.provideCredentials("user", "pass", "urll");
            ftpconf.dataSource = DataSource.FTP;    
            FTPConfig ftpconf2 = new FTPConfig();
            ftpconf2.provideCredentials("user2", "pass2", "urll2");
            ConfigHub confHub = new ConfigHub();
            confHub.AddConfigHubEntry(ftpconf, ftpconf2);
            confHub.isActive = true;
            CoreTask.AddTaskEntry("zad2", confHub);
            SyncActions.GetFilesList();
            */
            // to wyżej da się w await
            // następnie pobierze się kolekcje i wleci update
            // Compare()

        }
        private void SyncToLeftOverride()
        {

        }
        private void SyncToRight()
        {
        }
        private void SyncToRightOverride()
        {
        }
    }
}
