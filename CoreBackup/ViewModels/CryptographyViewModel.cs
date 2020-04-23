using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using CoreBackup.Models.Crypto;
using ReactiveUI;

namespace CoreBackup.ViewModels
{
    class CryptographyViewModel : ViewModelBase
    {
        private ReactiveCommand<Unit, Unit> GenerateKeyCommand { get; }
        private ReactiveCommand<Unit, Unit> GenerateIVCommand { get; }
        
        private ReactiveCommand<Unit, Unit> SaveKeyAndIVCommand { get; }
        private ReactiveCommand<Unit, Unit> LoadKeyAndIVCommand { get; }

        public CryptographyViewModel()
        {
            GenerateKeyCommand = ReactiveCommand.Create(GenerateKey);
            GenerateIVCommand = ReactiveCommand.Create(GenerateIV);

            SaveKeyAndIVCommand = ReactiveCommand.Create(SaveKeyAndIV);
            LoadKeyAndIVCommand = ReactiveCommand.Create(LoadKeyAndIV);
        }

        private void GenerateKey()
        { 
            Encryption.CreateAESKey();
        }

        private void GenerateIV()
        {
            Encryption.CreateAESIV();
        }

        private void SaveKeyAndIV()
        {
            Encryption.SaveAES_KeyIV_ToFile();
        }

        private void LoadKeyAndIV()
        {
            Encryption.LoadAES_KeyIV_FromFile();
        }
    }
}
