using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPage : ContentPage
    {
        public ObservableCollection<BillRecordDayGroup> RecordDayGroup { get; set; } = new ObservableCollection<BillRecordDayGroup>();

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
            foreach (var billRecordDayGroup in BillRecordDayGroup.ConvertAll(data))
            {
                RecordDayGroup.Add(billRecordDayGroup);
            }

            RecordList.ItemsSource = RecordDayGroup;
        }
    }
}