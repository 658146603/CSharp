using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DataPage : ContentPage
    {
        public DataPage()
        {
            InitializeComponent();
        }

        private async void NewBillRecord_OnClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AddBillPage());
        }
    }
}