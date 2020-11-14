using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static BalanceYourIO.DataProvider;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseDatePopup : PopupPage
    {
        private DateTime DateTime { get; set; } = DateTime.Today;
        private int _hour = DateTime.Now.Hour;

        private readonly Color _selected = Color.Coral;
        private readonly Color _unselected = Color.Transparent;

        private readonly OnDateSelected _listener;

        private DayDetail _dayDetail;

        public ChooseDatePopup(OnDateSelected listener)
        {
            InitializeComponent();
            _listener = listener;
            DatePicker.Date = DateTime;

            if (DateConverter.Dawn.Contains(_hour))
            {
                DayDawn_Selected(null, null);
            }

            if (DateConverter.Morning.Contains(_hour))
            {
                DayMorning_Selected(null, null);
            }

            if (DateConverter.Noon.Contains(_hour))
            {
                DayNoon_Selected(null, null);
            }

            if (DateConverter.Afternoon.Contains(_hour))
            {
                DayAfternoon_Selected(null, null);
            }

            if (DateConverter.Evening.Contains(_hour))
            {
                DayEvening_Selected(null, null);
            }
        }

        private async void Submit_OnClicked(object sender, EventArgs e)
        {
            _hour = (int) _dayDetail;
            var final = new DateTime(DateTime.Year, DateTime.Month, DateTime.Day, _hour, 0, 0);

            _listener.Invoke(final);
            await Navigation.PopPopupAsync();
        }

        private void DatePicker_OnDateSelected(object sender, DateChangedEventArgs e)
        {
            DateTime = DatePicker.Date;
        }

        private void DayDawn_Selected(object sender, EventArgs e)
        {
            DayDawn.BackgroundColor = _selected;
            DayMorning.BackgroundColor = _unselected;
            DayNoon.BackgroundColor = _unselected;
            DayAfternoon.BackgroundColor = _unselected;
            DayEvening.BackgroundColor = _unselected;
            
            _dayDetail = DayDetail.Dawn;
        }

        private void DayMorning_Selected(object sender, EventArgs e)
        {
            DayDawn.BackgroundColor = _unselected;
            DayMorning.BackgroundColor = _selected;
            DayNoon.BackgroundColor = _unselected;
            DayAfternoon.BackgroundColor = _unselected;
            DayEvening.BackgroundColor = _unselected;
            
            _dayDetail = DayDetail.Morning;
        }

        private void DayNoon_Selected(object sender, EventArgs e)
        {
            DayDawn.BackgroundColor = _unselected;
            DayMorning.BackgroundColor = _unselected;
            DayNoon.BackgroundColor = _selected;
            DayAfternoon.BackgroundColor = _unselected;
            DayEvening.BackgroundColor = _unselected;
            
            _dayDetail = DayDetail.Noon;
        }

        private void DayAfternoon_Selected(object sender, EventArgs e)
        {
            DayDawn.BackgroundColor = _unselected;
            DayMorning.BackgroundColor = _unselected;
            DayNoon.BackgroundColor = _unselected;
            DayAfternoon.BackgroundColor = _selected;
            DayEvening.BackgroundColor = _unselected;
            
            _dayDetail = DayDetail.Afternoon;
        }

        private void DayEvening_Selected(object sender, EventArgs e)
        {
            DayDawn.BackgroundColor = _unselected;
            DayMorning.BackgroundColor = _unselected;
            DayNoon.BackgroundColor = _unselected;
            DayAfternoon.BackgroundColor = _unselected;
            DayEvening.BackgroundColor = _selected;
            
            _dayDetail = DayDetail.Evening;
        }
    }

    //日期选择器的委托
    public delegate void OnDateSelected(DateTime dateTime);
}