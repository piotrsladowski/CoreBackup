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
        }


        public void OnOpenedTasksView(object o, EventArgs e)
        {
            Debug.WriteLine("OnOpenedTasksView event successfully raised");
            GetAllFiles();
        }

        private void GetAllFiles()
        {
            List<FileInformation> allLeftFiles = new List<FileInformation>();
            List<FileInformation> allRightFiles = new List<FileInformation>();
            LeftFiles.Where(l => l.Size != 0).ToList().All(i => LeftFiles.Remove(i));
            // Clear LeftFiles.
            RightFiles.Where(l => l.Size != 0).ToList().All(i => RightFiles.Remove(i));

            foreach (KeyValuePair<string, ConfigHub> entry in CoreTask.tasksList)
            {

                foreach (Configuration leftConf in entry.Value.LeftSources)
                {
                    allLeftFiles.AddRange(leftConf.GetFiles());
                    allLeftFiles.All(c => { c.IsChecked = true; return true; });
                }

                foreach (Configuration rightConf in entry.Value.RightSources)
                {
                    allRightFiles.AddRange(rightConf.GetFiles());
                    allLeftFiles.All(c => { c.IsChecked = false; return true; });
                }
            }
            LeftFiles.AddRange(allLeftFiles);
            RightFiles.AddRange(allRightFiles);
        }

        private void SyncMirror()
        {

        }

        private void SyncToLeft()
        {

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
