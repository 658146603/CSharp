using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static BalanceYourIO.DataProvider;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddBillPage : ContentPage
    {
        private BillRecord BillRecord { get; set; } = new BillRecord
            {Amount = 4.00f, Remark = "", Time = DateTime.Now, BillType = BillTypes[0]};

        public AddBillPage()
        {
            InitializeComponent();

            DateLabel.Text = DateConverter.ToFriendDateTimeString(BillRecord.Time);

            ChooseDate.Clicked += async (sender, args) =>
            {
                var page = new ChooseDatePopup(SetTime);
                await PopupNavigation.Instance.PushAsync(page);
            };
        }

        void SetTime(DateTime dateTime)
        {
            BillRecord.Time = dateTime;
            DateLabel.Text = DateConverter.ToFriendDateTimeString(dateTime);
        }
    }
}