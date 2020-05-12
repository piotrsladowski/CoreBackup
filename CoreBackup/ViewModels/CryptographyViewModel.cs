using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Text;
using Avalonia.Native.Interop;
using Avalonia.Remote.Protocol.Input;
using CoreBackup.Models.Crypto;
using CoreBackup.Models.Tasks;
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
            EventLogViewModel.AddNewRegistry("AES Key has been generated",
                DateTime.Now, this.GetType().Name, "HIGH");
        }

        private void GenerateIV()
        {
            Encryption.CreateAESIV();
            CheckKeyAndIVStatus();
            EventLogViewModel.AddNewRegistry("AES IV has been generated",
                DateTime.Now, this.GetType().Name, "HIGH");
        }

        private void SaveKeyAndIV()
        {
            if (Encryption.IsKeySet && Encryption.IsIVSet)
            {
                Encryption.SaveAES_KeyIV_ToFile();
            }
            else
            {
                EventLogViewModel.AddNewRegistry("AES Key and IV are not generated thus can not be saved",
                    DateTime.Now, this.GetType().Name, "MEDIUM");
            }
        }

        private void LoadKeyAndIV()
        {
            Encryption.LoadAES_KeyIV_FromFile();
            CheckKeyAndIVStatus();
        }

        private void CheckKeyAndIVStatus()
        {
            if (Encryption.IsKeySet)
            {
                KeyStatus = "Generated ";
            }
            else
            {
                KeyStatus = "Not generated ";
            }

            if (Encryption.IsIVSet)
            {
                IVStatus = "Generated ";
            }
            else
            {
                IVStatus = "Not generated ";
            }

            if (Encryption.IsPathRemembered)
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
