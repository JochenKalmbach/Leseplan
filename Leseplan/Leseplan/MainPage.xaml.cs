using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Leseplan
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();


            // Load Leseplan...

            LeseplanVM lp = null;

            // Display names of embedded resources
            var assembly = typeof(MainPage).Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                // Read the first json
                if (string.Equals(Path.GetExtension(res), ".json", StringComparison.OrdinalIgnoreCase))
                {
                    var s = assembly.GetManifestResourceStream(res);
                    var sr = new StreamReader(s);
                    var lpd = LeseplanData.Load(sr.ReadToEnd());

                    lp = new LeseplanVM(lpd);
                }
            }

            if (lp == null)
            {
                lp = new LeseplanVM();
            }

            this.BindingContext = lp;
        }
    }


    public class LeseplanVM : ViewModelBase
    {
        public LeseplanVM()
        {
            Commands.Add("PrevDay", new Command(PrevDay));
            Commands.Add("NextDay", new Command(NextDay));
            Commands.Add("Options", new Command(Options));
            Commands.Add("Today", new Command(Today));
        }
        public LeseplanVM(LeseplanData lpd) : this()
        {
            _LeseplanData = lpd;
            LoadUserData();
            AssignData(DateTime.Now.Date);
        }

        private LeseplanData _LeseplanData;

        void AssignData(DateTime actDate)
        {
            Entries.Clear();
            var dt = UserData.StartDate;
            int day = 0;
            Bibel b = _Optionen.Data.GetFromShort(UserData.Translation);
            foreach (var lpde in _LeseplanData.Entries)
            {
                Entries.Add(new EntryVM(lpde, b, day, ItemVMCheckedChanged, GetChecked) { Date = dt.AddDays(day) });
                day++;
            }

            Date = actDate;
        }


        #region UserData
        private void LoadUserData()
        {
            var fn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Id}_ud.json");
            if (File.Exists(fn))
            {
                UserData = LeseplanUserData.Load(File.ReadAllText(fn));
            }
            else
            {
                UserData = new LeseplanUserData();
                SaveUserData();
            }
        }

        private void SaveUserData()
        {
            var fn = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{Id}_ud.json");
            File.WriteAllText(fn, UserData.ToJson());
        }

        private void ItemVMCheckedChanged(ItemVM item, bool chked)
        {
            if (UserData == null)
            {
                UserData = new LeseplanUserData();
            }
            var key = item.Ref;
            if (!UserData.ReadItems.ContainsKey(key))
            {
                UserData.ReadItems[key] = new ItemReadData { Ref = item.Ref };
            }
            UserData.ReadItems[key].Checked = chked;
            SaveUserData();
        }

        private bool GetChecked(ItemVM item)
        {
            var key = item.Ref;
            if (UserData != null && UserData.ReadItems.ContainsKey(key))
            {
                return UserData.ReadItems[key].Checked;
            }
            return false;
        }

        internal void PrevDay()
        {
            Date = Date.Date.AddDays(-1);
        }

        internal void NextDay()
        {
            Date = Date.Date.AddDays(1);
        }

        internal void Today()
        {
            Date = DateTime.Now.Date;
        }

        #endregion

        LeseplanOptions _Optionen = new LeseplanOptions();
        private async void Options()
        {
            var nav = App.Current.MainPage as NavigationPage;
            if (nav != null)
            {
                _Optionen.Data.StartDate = UserData.StartDate;
                _Optionen.Data.SetTranslation(UserData.Translation);
                if (_Optionen.Data.OnSave == null)
                {
                    _Optionen.Data.OnSave = async opt =>
                    {
                        // Save only if changed
                        if (UserData.StartDate.Date != opt.StartDate.Date || !string.Equals(UserData.Translation, opt.Translation.Short, StringComparison.OrdinalIgnoreCase))
                        {
                            UserData.StartDate = opt.StartDate.Date;
                            UserData.Translation = opt.Translation.Short;
                            SaveUserData();
                            // Re-Apply all data
                            AssignData(Date);
                        }
                        //await nav.PopAsync();
                    };
                }

                await nav.PushAsync(_Optionen);
            }
        }

        private string Id { get; set; }

        private LeseplanUserData UserData { get; set; }

        private DateTime _Date;
        public DateTime Date {
            get { return _Date; }
            set {
                _Date = value;
                OnPropertyChanged(nameof(Date));
                var ed = Entries.FirstOrDefault(p => p.Date.Date == _Date.Date);
                if (ed != null)
                {
                    CurrentEntry = ed;
                }
                else
                {
                    CurrentEntry = null;
                }
            }
        }

        EntryVM _CurrentEntry;
        public EntryVM CurrentEntry
        {
            get
            {
                return _CurrentEntry;
            }
            set
            {
                if (!object.ReferenceEquals(_CurrentEntry, value))
                {
                    _CurrentEntry = value;
                    OnPropertyChanged(nameof(CurrentEntry));
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        public List<ItemVM> Items
        {
            get
            {
                if (_CurrentEntry == null) return null;
                return _CurrentEntry.Items;
            }
        }

        public List<EntryVM> Entries { get; set; } = new List<EntryVM>();
    }

    public class EntryVM
    {
        public EntryVM() { }
        public EntryVM(EntryData ed, Bibel trans, int no, Action<ItemVM, bool> checkedHandler, Func<ItemVM, bool> getChecked)
        {
            Text = $"Tag {no+1}";
            foreach (var id in ed.Items)
            {
                Items.Add(new ItemVM(id, trans, checkedHandler, getChecked));
            }
        }
        public DateTime Date { get; set; } = DateTime.Now.Date;

        public string Text { get; set; } = "Heute";

        public List<ItemVM> Items { get; set; } = new List<ItemVM>();
    }

    public class ItemVM : ViewModelBase
    {
        public ItemVM()
        {
            Commands.Add("Click", new Command(OnClick));
        }
        public ItemVM(ItemData id, Bibel trans, Action<ItemVM, bool> checkedHandler, Func<ItemVM, bool> getChecked) : this()
        {
            this.Ref = id.Ref;
            this.Text = id.Ref;
            Translation = trans;
            CheckedHandler = checkedHandler;
            GetChecked = getChecked;
        }

        public string Ref { get; private set; }

        private Action<ItemVM, bool> CheckedHandler;
        private Func<ItemVM, bool> GetChecked;
        private Bibel Translation;
        public string Text { get; set; } = "TEXT";

        public bool Checked
        {
            get
            {
                return GetChecked(this);
            }
            set
            {
                CheckedHandler?.Invoke(this, value);
                OnPropertyChanged(nameof(Checked));
            }
        }

        internal void OnClick(object o)
        {
            Device.OpenUri(Translation.ToUrl(Ref));
        }
    }

    /// <summary>
    /// See: https://stackoverflow.com/questions/32667408/how-to-implement-inotifypropertychanged-in-xamarin-forms
    /// </summary>
    public class ObservableProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class ViewModelBase : ObservableProperty
    {
        public Dictionary<string, ICommand> Commands { get; protected set; }

        public ViewModelBase()
        {
            Commands = new Dictionary<string, ICommand>();
        }
    }
}
