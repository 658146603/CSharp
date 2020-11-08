using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SQLite;
using Xamarin.Forms;
using static BalanceYourIO.DataProvider;

namespace BalanceYourIO
{
    public class BillRecordDetail
    {
        private static readonly Dictionary<string, BillType> BillTypeDict = DataProvider.BillTypes.ToDictionary(type =>
            type.Id, type => type);

        public int Id { get; set; }
        public BillType Type { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public double Amount { get; set; }
        public string Remark { get; set; }
        public Color Color { get; set; }

        public BillRecordDetail(BillRecord billRecord)
        {
            Id = billRecord.Id;
            Type = BillTypeDict[billRecord.BillType];
            Date = billRecord.Time.Date;
            Time = billRecord.Time;
            Amount = billRecord.Amount;
            Remark = billRecord.Remark;
            switch (Type.IoType)
            {
                case IoType.Income:
                    Color = Color.Green;
                    break;
                case IoType.Outcome:
                    Color = Color.Orange;
                    break;
                case IoType.Other:
                    Color = Color.Gray;
                    break;
            }
        }

        public override string ToString()
        {
            return $"{Id}, {DateConverter.ToFriendDateTimeString(Date)}, {Type}, {Amount}, {Remark}";
        }
        
        internal class BillRecordConverter: IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return BillRecordDayGroup.ConvertAll((List<BillRecord>) value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }
    }
}