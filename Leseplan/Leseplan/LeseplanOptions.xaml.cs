using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Leseplan
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LeseplanOptions : ContentPage
	{
		public LeseplanOptions ()
		{
			InitializeComponent ();
		}

        public LeseplanOptionsVM Data
        {
            get
            {
                var o = BindingContext as LeseplanOptionsVM;
                if (o == null)
                {
                    BindingContext = o = new LeseplanOptionsVM();
                }
                return o;
            }
            set
            {
                BindingContext = value;
            }
        }
    }

    public class LeseplanOptionsVM : ViewModelBase
    {
        private DateTime _startDate;

        public LeseplanOptionsVM()
        {
            Commands.Add("Save", new Command(Save));
        }
        public Action<LeseplanOptionsVM> OnSave { get; set; }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                var old = _startDate;
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                if (old.Date != _startDate.Date)
                { Save(); }
            }
        }

        void Save()
        {
            OnSave?.Invoke(this);
        }

        private Bibeln _Bibeln = new Bibeln();
        public List<Bibel> Translations
        {
            get
            {
                return _Bibeln.Translations;
            }
        }

        Bibel _Translation;
        public Bibel Translation
        {
            get
            {
                return _Translation;
            }
            set
            {
                var old = _Translation;
                _Translation = value;
                OnPropertyChanged(nameof(Translation));
                if (!object.ReferenceEquals(old, _Translation))
                { Save(); }
            }
        }

        public Bibel GetFromShort(string key)
        {
            var t = Translations.FirstOrDefault(p => string.Equals(p.Short, key, StringComparison.OrdinalIgnoreCase));
            if (t == null)
            {
                t = Translations.First();
            }
            return t;
        }


        public void SetTranslation(string key)
        {
            Translation = GetFromShort(key);
        }
    }
}