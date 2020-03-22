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

namespace CoreBackup.ViewModels
{
    class ActionsViewModel : ViewModelBase
    {

        // https://reactiveui.net/docs/handbook/collections/

        private readonly ReadOnlyObservableCollection<string> _derived;
        public ReadOnlyObservableCollection<string> Derived => _derived;

        public ObservableCollectionExtended<string> LocalFiles { get; }

        public ReactiveCommand<Unit, Unit> SendToRemoteCommand { get; }
        public ReactiveCommand<Unit, Unit> PauseCommand { get; }

        public ActionsViewModel()
        {
            SendToRemoteCommand = ReactiveCommand.Create(SendToRemote);
            PauseCommand = ReactiveCommand.Create(Pause);
            LocalFiles = new ObservableCollectionExtended<string>();
            LocalFiles.ToObservableChangeSet()
            .Transform(value => value)
            // No need to use the .ObserveOn() operator here, as
            // ObservableCollectionExtended is single-threaded.
            .Bind(out _derived)
            .Subscribe();

            // Update the source collection and the derived
            // collection will update as well.
        }


        private void SendToRemote()
        {
            BasicIO bIO = new BasicIO();
            LocalFiles.Where(l => l.Length != 0).ToList().All(i => LocalFiles.Remove(i));
            foreach (string f in bIO.getFilesInDirectory("E:\\"))
            {
                LocalFiles.Add(f);
            }
        }

        private void Pause()
        {
            LocalFiles.Add("test");
        }
    }
}
