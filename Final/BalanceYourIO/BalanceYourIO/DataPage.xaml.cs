using System;
using System.Collections.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
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

        //跳转新增记录页面
        private async void NewBillRecord_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddBillPage());
        }

        //每次页面在栈顶时进行数据更新
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RecordList.ItemsSource = null;

            var data = App.Database.GetBillRecordsAsync().Result;
            RecordDayGroup.Clear();

            //获得每天的分组
            var dateGroup =
                from billRecord in data
                group billRecord by billRecord.Time.Date
                into g
                orderby g.Key descending
                select g;

            //对分组进行处理
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

            //设置数据源
            RecordList.ItemsSource = RecordDayGroup;
        }

        //点击记录弹出详细窗口
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