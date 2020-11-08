using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPage : ContentPage
    {
        public List<BillRecordDayGroup> RecordDayGroup { get; set; }

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
            var data = App.Database.GetBillRecordsAsync().Result;
            RecordDayGroup = BillRecordDayGroup.ConvertAll(data);
        }
    }
}