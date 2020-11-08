using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BalanceYourIO
{
    public class BillRecordDayGroup : List<BillRecord>
    {
        public DateTime Date { get; set; }

        public double Income { get; set; }

        public double Outcome { get; set; }

        private List<BillRecordDetail> Records { get; set; }

        public BillRecordDayGroup(List<BillRecord> billRecords)
        {
            Records = billRecords.ConvertAll(delegate(BillRecord input)
            {
                BillRecordDetail billRecordDetail = new BillRecordDetail(input);
                return billRecordDetail;
            });

            if (Records.Count > 0)
            {
                Income = Records.Sum(detail => detail.Type.IoType == DataProvider.IoType.Income ? detail.Amount : 0);
                Outcome = Records.Sum(detail => detail.Type.IoType == DataProvider.IoType.Outcome ? detail.Amount : 0);
            }
        }

        public static ObservableCollection<BillRecordDayGroup> ConvertAll(List<BillRecord> billRecords)
        {
            ObservableCollection<BillRecordDayGroup> collection = new ObservableCollection<BillRecordDayGroup>();
            billRecords.GroupToDictionary(record => record.Time.Date).ForEach(pair =>
            {
                collection.Add(new BillRecordDayGroup(pair.Value) {Date = pair.Key});
            });

            return collection;
        }

        public static ObservableCollection<BillRecordDayGroup> All
        {
            get => ConvertAll(App.Database.GetBillRecordsAsync().Result);
        }
    }
}