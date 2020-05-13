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
using System.Threading.Tasks;

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

        private long summaryFileSize;
        private Dictionary<string, FileInformation> leftFilesDictionary;
        private Dictionary<string, FileInformation> rightFilesDictionary;

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

            leftFilesDictionary = new Dictionary<string, FileInformation>();
            rightFilesDictionary = new Dictionary<string, FileInformation>();
        }


        public void OnOpenedTasksView(object o, EventArgs e)
        {
            Debug.WriteLine("OnOpenedTasksView event successfully raised");
            RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            GetAllFiles();
            CreateFilesDictionary();
            GetSummaryFileInfo();
            CompareFiles();
        }

        private void GetAllFiles()
        {
            // To Avoid re-listing files when Button Tasks from Submenu Clicked.
            LeftFiles.Clear();
            RightFiles.Clear();

            List<FileInformation> allLeftFiles = new List<FileInformation>();
            List<FileInformation> allRightFiles = new List<FileInformation>();
            LeftFiles.Where(l => l.Size != 0).ToList().All(i => LeftFiles.Remove(i));
            // Clear LeftFiles.
            RightFiles.Where(l => l.Size != 0).ToList().All(i => RightFiles.Remove(i));

            foreach (KeyValuePair<string, ConfigHub> entry in CoreTask.tasksList)
            {
                foreach (Configuration leftConf in entry.Value.LeftSources)
                {
                    var files = leftConf.GetFiles();
                    files.All(c => { c.ConfigurationName = entry.Key; return true; });
                    allLeftFiles.AddRange(files);
                }

                foreach (Configuration rightConf in entry.Value.RightSources)
                {
                    var files = rightConf.GetFiles();
                    files.All(c => { c.ConfigurationName = entry.Key; return true; });
                    allRightFiles.AddRange(files);
                }
            }
            allLeftFiles.All(c => { c.IsChecked = true; return true; });
            allRightFiles.All(c => { c.IsChecked = true; return true; });
            LeftFiles.AddRange(allLeftFiles);
            RightFiles.AddRange(allRightFiles);
        }

        private void CreateFilesDictionary()
        {
            leftFilesDictionary = new Dictionary<string, FileInformation>();
            rightFilesDictionary = new Dictionary<string, FileInformation>();
            foreach (var item in LeftFiles)
            {
                leftFilesDictionary.Add(item.RelativePath, item);
            }
            foreach (var item in RightFiles)
            {
                rightFilesDictionary.Add(item.RelativePath, item);
            }
        }

        private void GetSummaryFileInfo()
        {
            summaryFileSize = 0;
            foreach (var item in LeftFiles)
            {
                summaryFileSize += item.Size;
            }
            foreach (var item in RightFiles)
            {
                summaryFileSize += item.Size;
            }
            Debug.WriteLine(summaryFileSize);
        }

        private void CompareFiles()
        {
            // Lookup foreach entry in LeftFilesDict in RightFilesDict
            foreach (KeyValuePair<string, FileInformation> entry in leftFilesDictionary)
            {
                if (rightFilesDictionary.TryGetValue(entry.Key, out FileInformation outFileInformation))
                {
                    if (entry.Value.ModificationTime > outFileInformation.ModificationTime)
                    {
                        var item = LeftFiles.FirstOrDefault(i => i.RelativePath == entry.Key);
                        if (item != null)
                        {
                            item.FileVersion = FileVersion.Newer;
                        }
                    }
                    else if (entry.Value.ModificationTime == outFileInformation.ModificationTime)
                    {
                        var item = LeftFiles.FirstOrDefault(i => i.RelativePath == entry.Key);
                        if (item != null)
                        {
                            item.FileVersion = FileVersion.Equal;
                        }
                    }
                    else
                    {
                        var item = LeftFiles.FirstOrDefault(i => i.RelativePath == entry.Key);
                        if (item != null)
                        {
                            item.FileVersion = FileVersion.Older;
                        }
                    }
                }
                else
                {
                    var item = LeftFiles.First(i => i.RelativePath == entry.Key);
                    if (item != null)
                    {
                        item.FileVersion = FileVersion.Newer;
                    }
                }

            }

            // Similarly for the second set

            foreach (KeyValuePair<string, FileInformation> entry in rightFilesDictionary)
            {
                if (leftFilesDictionary.TryGetValue(entry.Key, out FileInformation outFileInformation))
                {
                    if (entry.Value.ModificationTime > outFileInformation.ModificationTime)
                    {
                        var item = RightFiles.FirstOrDefault(i => i.RelativePath == entry.Key);
                        if (item != null)
                        {
                            item.FileVersion = FileVersion.Newer;
                        }
                    }
                    else if (entry.Value.ModificationTime == outFileInformation.ModificationTime)
                    {
                        var item = RightFiles.FirstOrDefault(i => i.RelativePath == entry.Key);
                        if (item != null)
                        {
                            item.FileVersion = FileVersion.Equal;
                        }
                    }
                    else
                    {
                        var item = RightFiles.FirstOrDefault(i => i.RelativePath == entry.Key);
                        if (item != null)
                        {
                            item.FileVersion = FileVersion.Older;
                        }
                    }
                }
                else
                {
                    var item = RightFiles.First(i => i.RelativePath == entry.Key);
                    if (item != null)
                    {
                        item.FileVersion = FileVersion.Newer;
                    }
                }

            }
        }

        private void SyncMirror()
        {
            SyncActions syncActions = new SyncActions(LeftFiles, RightFiles);
            syncActions.SyncMirror();
            EventLogViewModel.AddNewRegistry("Synchronization completed",
DateTime.Now, this.GetType().Name, "HIGH");
            RefreshDataGrid();
        }

        private void SyncToLeft()
        {
            SyncActions syncActions = new SyncActions(LeftFiles, RightFiles);
            syncActions.SyncToLeft();
            EventLogViewModel.AddNewRegistry("Synchronization completed",
DateTime.Now, this.GetType().Name, "HIGH");
            RefreshDataGrid();
        }
        private void SyncToLeftOverride()
        {
            SyncActions syncActions = new SyncActions(LeftFiles, RightFiles);
            syncActions.SyncToLeftOverride();
            EventLogViewModel.AddNewRegistry("Synchronization completed",
DateTime.Now, this.GetType().Name, "HIGH");
            RefreshDataGrid();
        }
        private void SyncToRight()
        {
            SyncActions syncActions = new SyncActions(LeftFiles, RightFiles);
            syncActions.SyncToRight();
            EventLogViewModel.AddNewRegistry("Synchronization completed",
DateTime.Now, this.GetType().Name, "HIGH");
            RefreshDataGrid();
        }
        private void SyncToRightOverride()
        {
            SyncActions syncActions = new SyncActions(LeftFiles, RightFiles);
            syncActions.SyncToRightOverride();
            EventLogViewModel.AddNewRegistry("Synchronization completed",
    DateTime.Now, this.GetType().Name, "HIGH");
            RefreshDataGrid();
        }
    }
}
