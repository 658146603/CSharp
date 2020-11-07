using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseDatePopup : PopupPage
    {
        private DateTime DateTime { get; set; } = DateTime.Today;
        private int _hour = DateTime.Now.Hour;

        private readonly OnDateSelected _listener;

        public ChooseDatePopup(OnDateSelected listener)
        {
            InitializeComponent();
            _listener = listener;
            DatePicker.Date = DateTime;

            DayDawn.IsChecked = DateConverter.Dawn.Contains(_hour);
            DayMorning.IsChecked = DateConverter.Morning.Contains(_hour);
            DayNoon.IsChecked = DateConverter.Noon.Contains(_hour);
            DayAfternoon.IsChecked = DateConverter.Afternoon.Contains(_hour);
            DayEvening.IsChecked = DateConverter.Evening.Contains(_hour);
        }

        private async void Submit_OnClicked(object sender, EventArgs e)
        {
            if (DayDawn.IsChecked)
            {
                _hour = 3;
            }

            if (DayMorning.IsChecked)
            {
                _hour = 8;
            }

            if (DayNoon.IsChecked)
            {
                _hour = 11;
            }

            if (DayAfternoon.IsChecked)
            {
                _hour = 15;
            }

            if (DayEvening.IsChecked)
            {
                _hour = 21;
            }

            var final = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day, _hour, 0, 0);
            
            _listener.Invoke(final);
            await Navigation.PopPopupAsync();
        }

        private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime = DatePicker.Date;
        }
    }

    public delegate void OnDateSelected(DateTime dateTime);
}