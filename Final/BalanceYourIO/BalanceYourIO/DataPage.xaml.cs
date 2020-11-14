using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPage : ContentPage
    {
        private ObservableCollection<BillRecordDayGroup> RecordDayGroup { get; set; } =
            new ObservableCollection<BillRecordDayGroup>();

        public DataPage()
        {
            InitializeComponent();
        }

        private async void NewBillRecord_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddBillPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RecordList.ItemsSource = null;

            var data = App.Database.GetBillRecordsAsync().Result;
            RecordDayGroup.Clear();

            var dateGroup =
                from billRecord in data
                group billRecord by billRecord.Time.Date
                into g
                orderby g.Key descending
                select g;

            RecordDayGroup =
                new ObservableCollection<BillRecordDayGroup>(
                    from item in dateGroup
                    select new BillRecordDayGroup(item.OrderByDescending(detail => detail.Time.Hour)
                        .ThenByDescending(detail => detail.Id).ToList()) {_date = item.Key}
                );


            // foreach (var billRecordDayGroup in groups)
            // {
            //     RecordDayGroup.Add(billRecordDayGroup);
            // }

            RecordList.ItemsSource = RecordDayGroup;
        }

        private async void RecordList_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView list && e.SelectedItem is BillRecordDetail item)
            {
                list.SelectedItem = null;
                var page = new BillRecordDetailPopup(item, OnAppearing);
                await PopupNavigation.Instance.PushAsync(page);
            }
        }
    }
}