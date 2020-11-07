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
        private static readonly List<BillType> OutcomeBillType =
            BillTypes.FindAll(type => type.IoType == IoType.Outcome);

        private static readonly List<BillType> IncomeBillType = BillTypes.FindAll(type => type.IoType == IoType.Income);
        private static readonly List<BillType> OtherBillType = BillTypes.FindAll(type => type.IoType == IoType.Other);

        private List<StackLayout> BillTypeCell = new List<StackLayout>();

        private BillRecord BillRecord { get; set; } = new BillRecord
            {Amount = 4.00f, Remark = "", Time = DateTime.Now, BillType = BillTypes[0]};

        private readonly Color _selected = new Color(77/255f, 182/255f, 172/255f, 75/100f);
        private readonly Color _unselected = Color.Transparent;

        private BillType _billType = OutcomeBillType[0];

        private BillType BillType
        {
            get => _billType;
            set
            {
                _billType = value;


                var index = BillTypes.FindIndex(type => type.Id == _billType.Id);
                foreach (var t in BillTypeCell)
                {
                    t.BackgroundColor = _unselected;
                }

                BillTypeCell[index].BackgroundColor = _selected;

            }
        }


        public AddBillPage()
        {
            InitializeComponent();
            DateLabel.Text = DateConverter.ToFriendDateTimeString(BillRecord.Time);
            ChooseDate.Clicked += async (sender, args) =>
            {
                var page = new ChooseDatePopup(SetTime);
                await PopupNavigation.Instance.PushAsync(page);
            };

            for (int i = 0; i < OutcomeBillType.Count; i++)
            {
                var cell = new StackLayout {Orientation = StackOrientation.Vertical, Margin = 12, Padding = 4};

                var button = new ImageButton {Source = OutcomeBillType[i].Icon};
                var index = i;
                button.Clicked += delegate { BillType = OutcomeBillType[index]; };
                button.BackgroundColor = _unselected;

                cell.Children.Add(button);
                cell.Children.Add(new Label {Text = OutcomeBillType[i].Name, HorizontalOptions = LayoutOptions.Center});

                BillTypesOutcomeGrid.Children.Add(cell, i % 5, i / 5);
                BillTypeCell.Add(cell);
            }

            for (int i = 0; i < IncomeBillType.Count; i++)
            {
                var cell = new StackLayout {Orientation = StackOrientation.Vertical, Margin = 16};

                var button = new ImageButton {Source = IncomeBillType[i].Icon};
                var index = i;
                button.Clicked += delegate { BillType = IncomeBillType[index]; };
                button.BackgroundColor = _unselected;

                cell.Children.Add(button);
                cell.Children.Add(new Label {Text = IncomeBillType[i].Name, HorizontalOptions = LayoutOptions.Center});

                BillTypesIncomeGrid.Children.Add(cell, i % 5, i / 5);
                BillTypeCell.Add(cell);
            }

            for (int i = 0; i < OtherBillType.Count; i++)
            {
                var cell = new StackLayout {Orientation = StackOrientation.Vertical, Margin = 16};

                var button = new ImageButton {Source = OtherBillType[i].Icon};
                var index = i;
                button.Clicked += delegate { BillType = OtherBillType[index]; };
                button.BackgroundColor = _unselected;

                cell.Children.Add(button);
                cell.Children.Add(new Label {Text = OtherBillType[i].Name, HorizontalOptions = LayoutOptions.Center});

                BillTypesOtherGrid.Children.Add(cell, i % 5, i / 5);
                BillTypeCell.Add(cell);
            }

            BillType = BillTypes[0];
        }

        private void SetTime(DateTime dateTime)
        {
            BillRecord.Time = dateTime;
            DateLabel.Text = DateConverter.ToFriendDateTimeString(dateTime);
        }
    }
}