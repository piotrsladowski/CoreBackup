using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using ReactiveUI;

namespace CoreBackup.ViewModels
{
    class ContainerViewModel : ViewModelBase
    {
        //ViewModelBase tasksScreen;
        TasksViewModel tasksScreen;
        ConfigurationViewModel configScreen;
        HomeViewModel homeScreen;
        FileExplorerViewModel fileExplorerScreen;
        EventLogViewModel eventLogScreen;
        ViewModelBase cryptoScreen;

        #region Constructor
        ViewModelBase screen;
        NavbarViewModel navbar;
        InfoPanelViewModel infoPanel;
        public ViewModelBase Screen
        {
            get => screen;
            private set => this.RaiseAndSetIfChanged(ref screen, value);
        }

        public NavbarViewModel Navbar
        {
            get => navbar;
            private set => this.RaiseAndSetIfChanged(ref navbar, value);
        }

        public InfoPanelViewModel InfoPanel
        {
            get => infoPanel;
            private set => this.RaiseAndSetIfChanged(ref infoPanel, value);
        }

        public ContainerViewModel()
        {     
            // Base ViewModels
            Screen = new HomeViewModel();
            Navbar = new NavbarViewModel();
            InfoPanel = new InfoPanelViewModel();

            // Mutable ViewModels
            homeScreen = new HomeViewModel();
            tasksScreen = new TasksViewModel();
            configScreen = new ConfigurationViewModel();
            fileExplorerScreen = new FileExplorerViewModel();
            eventLogScreen = new EventLogViewModel();
            cryptoScreen = new CryptographyViewModel();

            OpenedTasksView += tasksScreen.OnOpenedTasksView;
            configScreen.SavedConfiguration += OnSavedConfiguration;
            SavedConfigurationNotified += InfoPanel.OnSavedConfigurationNotified;

            // Internet Connection Check - Timer
            SetupTimer();

            EventLogViewModel.AddNewRegistry("Welcome to CoreBackup! Enjoy and Keep your data safe!",
                DateTime.Now, "CoreBackup", "LOW");
        }
        #endregion

        #region ViewModel tasks
        public TasksViewModel TasksScreen
        {
            get => tasksScreen;
            private set => this.RaiseAndSetIfChanged(ref tasksScreen, value);
        }


        public ConfigurationViewModel ConfigScreen
        {
            get => configScreen;
            private set => this.RaiseAndSetIfChanged(ref configScreen, value);
        }

        public ViewModelBase CryptoScreen
        {
            get => cryptoScreen;
            private set => this.RaiseAndSetIfChanged(ref cryptoScreen, value);
        }

        public HomeViewModel HomeScreen
        {
            get => homeScreen;
            private set => this.RaiseAndSetIfChanged(ref homeScreen, value);
        }

        public FileExplorerViewModel FileExplorerScreen
        {
            get => fileExplorerScreen;
            private set => this.RaiseAndSetIfChanged(ref fileExplorerScreen, value);
        }

        public EventLogViewModel EventLogScreen
        {
            get => eventLogScreen;
            private set => this.RaiseAndSetIfChanged(ref eventLogScreen, value);
        }
        #endregion

        #region Events
        //public delegate void OpenedTasksViewEventHandler(object o, EventArgs e);
        //public event OpenedTasksViewEventHandler OpenedTasksView;
        public event EventHandler<EventArgs> OpenedTasksView;
        public event EventHandler<EventArgs> SavedConfigurationNotified;

        protected virtual void OnOpenedTasksViewEvent()
        {
            OpenedTasksView(this, EventArgs.Empty);
        }

        protected virtual void OnSavedConfigurationNotified()
        {
            SavedConfigurationNotified(this, EventArgs.Empty);
        }

        public void OnSavedConfiguration(object o, EventArgs e)
        {
            Debug.WriteLine("OnSavedConfiguration event successfully raised");
            OnSavedConfigurationNotified();
        }

        #endregion

        #region ChangeScreen on Click Actions
        public void ChangeScreenHome()
        {
            Screen = homeScreen;
        }

        public void ChangeScreenConfiguration()
        {           
            Screen = configScreen;
        }

        public void ChangeScreenEventLog()
        {          
            Screen = eventLogScreen;           
        }

        public void ChangeScreenTasks()
        {
            Screen = tasksScreen;
            OnOpenedTasksViewEvent();
        }
        public void ChangeScreenFileExplorer()
        {          
            Screen = fileExplorerScreen;
        }

        public void ChangeScreenCryptography()
        {
            Screen = cryptoScreen;
        }
        #endregion

        #region InternetConnection
        private string connectionStatus;

        public string ConnectionStatus
        {
            get => connectionStatus;
            set => this.RaiseAndSetIfChanged(ref connectionStatus, value);
        }

        private Timer StatusTimer = new Timer();
        private void SetupTimer()
        {
            StatusTimer.Elapsed += new ElapsedEventHandler(ConnectionStatusChecker);
            StatusTimer.Interval = 1000;
            StatusTimer.Enabled = true;
            StatusTimer.Start();
        }
        private void ConnectionStatusChecker(object source, ElapsedEventArgs e)
        {

            Ping myPing = new Ping();
            String host = "www.google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            try
            {
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                if (reply.Status == IPStatus.Success)
                {
                    string delay = reply.RoundtripTime.ToString();
                    ConnectionStatus = "Connected   Delay: " +  delay + " ms";
                }
                    
                else
                    ConnectionStatus = "Disconnected";
            }
            catch (PingException)
            {
                ConnectionStatus = "Disconnected";
            }
        }
        #endregion


    }
}
