using System;
using System.Collections.Generic;
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
        //构造收入，支出，其他类型列表
        private static readonly List<BillType> OutcomeBillType =
            BillTypes.FindAll(type => type.IoType == IoType.Outcome);
        private static readonly List<BillType> IncomeBillType = BillTypes.FindAll(type => type.IoType == IoType.Income);
        private static readonly List<BillType> OtherBillType = BillTypes.FindAll(type => type.IoType == IoType.Other);

        private readonly List<StackLayout> _billTypeCell = new List<StackLayout>();

        //判断是更新还是新建记录
        private bool _update = false;

        //存储当前记账数据的BillRecord对象
        private BillRecord BillRecord { get; set; } = new BillRecord
            {Amount = 0.00f, Remark = "", Time = DateTime.Now, BillType = BillTypes[0].Id};

        //类型选中和未选中的颜色
        private readonly Color _selected = new Color(77 / 255f, 182 / 255f, 172 / 255f, 75 / 100f);
        private readonly Color _unselected = Color.Transparent;

        //默认选中的项为第一项
        private BillType _billType = OutcomeBillType[0];

        private BillType BillType
        {
            get => _billType;
            set
            {
                _billType = value;

                //设置类型的同时将选中的类型设置颜色标记
                var index = BillTypes.FindIndex(type => type.Id == _billType.Id);
                foreach (var t in _billTypeCell)
                {
                    t.BackgroundColor = _unselected;
                }

                _billTypeCell[index].BackgroundColor = _selected;
            }
        }


        public AddBillPage()
        {
            InitializeComponent();
            DateLabel.Text = DateConverter.ToFriendDateTimeString(BillRecord.Time);
            
            //点击日历按钮跳转日期选择
            ChooseDate.Clicked += async (sender, args) =>
            {
                var page = new ChooseDatePopup(SetTime);
                await PopupNavigation.Instance.PushAsync(page);
            };

            for (int i = 0; i < OutcomeBillType.Count; i++)
            {
                var cell = new StackLayout
                {
                    Orientation = StackOrientation.Vertical, Margin = 12, Padding = 4,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = -1
                };

                var button = new ImageButton
                {
                    Source = OutcomeBillType[i].Icon,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = -1,
                    BackgroundColor = Color.Transparent
                };
                var index = i;
                button.Clicked += delegate { BillType = OutcomeBillType[index]; };
                cell.BackgroundColor = _unselected;

                cell.Children.Add(button);
                cell.Children.Add(new Label {Text = OutcomeBillType[i].Name, HorizontalOptions = LayoutOptions.Center});

                BillTypesOutcomeGrid.Children.Add(cell, i % 5, i / 5);

                var cellSize = cell.Measure(-1, -1);

                BillTypesOutcomeGrid.RowDefinitions.ForEach(definition =>
                    definition.Height = new GridLength(cellSize.Request.Height * 8));

                _billTypeCell.Add(cell);
            }

            for (int i = 0; i < IncomeBillType.Count; i++)
            {
                var cell = new StackLayout
                {
                    Orientation = StackOrientation.Vertical, Margin = 12, Padding = 4,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = -1
                };

                var button = new ImageButton
                {
                    Source = IncomeBillType[i].Icon,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = -1,
                    BackgroundColor = Color.Transparent
                };
                var index = i;
                button.Clicked += delegate { BillType = IncomeBillType[index]; };
                cell.BackgroundColor = _unselected;

                cell.Children.Add(button);
                cell.Children.Add(new Label {Text = IncomeBillType[i].Name, HorizontalOptions = LayoutOptions.Center});

                BillTypesIncomeGrid.Children.Add(cell, i % 5, i / 5);

                var cellSize = cell.Measure(-1, -1);

                BillTypesIncomeGrid.RowDefinitions.ForEach(definition =>
                    definition.Height = new GridLength(cellSize.Request.Height * 8));

                _billTypeCell.Add(cell);
            }

            for (int i = 0; i < OtherBillType.Count; i++)
            {
                var cell = new StackLayout
                {
                    Orientation = StackOrientation.Vertical, Margin = 12, Padding = 4,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = -1
                };

                var button = new ImageButton
                {
                    Source = OtherBillType[i].Icon,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.Start,
                    HeightRequest = -1,
                    BackgroundColor = Color.Transparent
                };
                var index = i;
                button.Clicked += delegate { BillType = OtherBillType[index]; };
                cell.BackgroundColor = _unselected;

                cell.Children.Add(button);
                cell.Children.Add(new Label {Text = OtherBillType[i].Name, HorizontalOptions = LayoutOptions.Center});

                BillTypesOtherGrid.Children.Add(cell, i % 5, i / 5);

                var cellSize = cell.Measure(-1, -1);

                BillTypesOtherGrid.RowDefinitions.ForEach(definition =>
                    definition.Height = new GridLength(cellSize.Request.Height * 8));

                _billTypeCell.Add(cell);
            }

            BillType = BillTypes[0];
        }
        
        public AddBillPage(BillRecord billRecord) : this()
        {
            _update = true;
            BillRecord = billRecord;
            BillType = BillTypes.Find(type => type.Id == BillRecord.BillType) ?? BillTypes[0];
            Amount.Text = $"{BillRecord.Amount}";
            Remark.Text = BillRecord.Remark;
            DateLabel.Text = DateConverter.ToFriendDateTimeString(BillRecord.Time);
        }

        //日期选择的回调
        private void SetTime(DateTime dateTime)
        {
            BillRecord.Time = dateTime;
            DateLabel.Text = DateConverter.ToFriendDateTimeString(dateTime);
        }

        // DONE 改进记账金额合理性逻辑
        private void Amount_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // if (e.NewTextValue.EndsWith(".") || e.NewTextValue.StartsWith("-"))
            // {
            //     Amount.TextColor = Color.Red;
            // }
            // else if (double.TryParse(e.NewTextValue, out var amount))
            // {
            //     Amount.TextColor = Color.Black;
            //
            //     Amount.Text = amount.ToString("#.##");
            // }
            // else
            // {
            //     Amount.TextColor = Color.Red;
            // }

            var amount = IsAmountValid(Amount.Text);

            if (amount <= 0)
            {
                Amount.TextColor = Color.Red;
            }
            else
            {
                Amount.TextColor = Color.Black;
            }
        }

        private async void Submit_OnClicked(object sender, EventArgs e)
        {
            // if (Amount.Text == null || Amount.Text.EndsWith("."))
            // {
            //     Amount.PlaceholderColor = Color.Red;
            //     return;
            // }
            //
            // if (Amount.Text.StartsWith("-"))
            // {
            //     Amount.PlaceholderColor = Color.Red;
            //     return;
            // }

            // if (double.TryParse(Amount.Text, out var amount))
            // {
            //     BillRecord.Amount = amount;
            //     BillRecord.BillType = BillType.Id;
            //     BillRecord.Remark = Remark.Text;
            
            var amount = IsAmountValid(Amount.Text);
            if (amount <= 0)
            {
                Amount.TextColor = Color.Red;
                return;
            }

            BillRecord.Amount = (double) amount;
            BillRecord.BillType = BillType.Id;
            BillRecord.Remark = Remark.Text;

            if (_update)
            {
                await App.Database.UpdateBillRecordAsync(BillRecord);
                Log.Warning(ClassId, $"UPDATE: {BillRecord}");
            }
            else
            {
                await App.Database.SaveBillRecordAsync(BillRecord);
                Log.Warning(ClassId, $"SAVE: {BillRecord}");
            }

            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync(true);
            }

            // }
            // else
            // {
            //     Amount.PlaceholderColor = Color.Red;
            // }
        }


        //判断金额合理性
        private decimal IsAmountValid(string text)
        {
            if (decimal.TryParse(Amount.Text, out var amount))
            {
                Log.Warning(ClassId, $"Before {text}, After {Math.Round(amount, 2)}");
                
                return amount >= 0 ? Math.Round(amount, 2): -1;
            }
            else
            {
                return -1;
            }
        }
    }
}