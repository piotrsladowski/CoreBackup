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

        #region Strings
        private string keyStatus;
        public string KeyStatus
        {
            get => keyStatus;
            set => this.RaiseAndSetIfChanged(ref keyStatus, value);
        }

        private string ivStatus;
        public string IVStatus
        {
            get => ivStatus;
            set => this.RaiseAndSetIfChanged(ref ivStatus, value);
        }

        private string pathStatus;
        public string PathStatus
        {
            get => pathStatus;
            set => this.RaiseAndSetIfChanged(ref pathStatus, value);
        }
        #endregion

        public CryptographyViewModel()
        {
            GenerateKeyCommand = ReactiveCommand.Create(GenerateKey);
            GenerateIVCommand = ReactiveCommand.Create(GenerateIV);

            SaveKeyAndIVCommand = ReactiveCommand.Create(SaveKeyAndIV);
            LoadKeyAndIVCommand = ReactiveCommand.Create(LoadKeyAndIV);

            CheckKeyAndIVStatus();
        }

        private void GenerateKey()
        { 
            Encryption.CreateAESKey();
            CheckKeyAndIVStatus();
        }

        private void GenerateIV()
        {
            Encryption.CreateAESIV();
            CheckKeyAndIVStatus();
        }

        private void SaveKeyAndIV()
        {
            Encryption.SaveAES_KeyIV_ToFile();
            CheckKeyAndIVStatus();
        }

        private void LoadKeyAndIV()
        {
            Encryption.LoadAES_KeyIV_FromFile();
            CheckKeyAndIVStatus();
        }

        private void CheckKeyAndIVStatus()
        {
            if (Encryption.isKeySet)
            {
                KeyStatus = "Generated, ";
            }
            else
            {
                KeyStatus = "Not generated, ";
            }

            if (Encryption.isIVSet)
            {
                IVStatus = "Generated, ";
            }
            else
            {
                IVStatus = "Not generated, ";
            }

            if (Encryption.isPathRemembered)
            {
                PathStatus = "path remembered";
            }
            else
            {
                PathStatus = "path not remembered";
            }
        }
    }
}
