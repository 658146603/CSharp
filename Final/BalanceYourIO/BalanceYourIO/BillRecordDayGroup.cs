using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BalanceYourIO
{
    public class BillRecordDayGroup
    {
        public DateTime Date { get; set; }

        public double Income { get; set; }

        public double Outcome { get; set; }

        public List<BillRecordDetail> Details { get; set; }
        
        public BillRecordDayGroup(List<BillRecord> billRecords)
        {

            Details = billRecords.ConvertAll(delegate(BillRecord input)
            {
                BillRecordDetail billRecordDetail = new BillRecordDetail(input);
                return billRecordDetail;
            });
            
            if (Details.Count > 0)
            {
                Income = Details.Sum(detail => detail.Type.IoType == DataProvider.IoType.Income ? detail.Amount : 0);
                Outcome = Details.Sum(detail => detail.Type.IoType == DataProvider.IoType.Outcome ? detail.Amount : 0);
            }
        }

        public static List<BillRecordDayGroup> ConvertAll(List<BillRecord> billRecords)
        {
            List<BillRecordDayGroup> collection = new List<BillRecordDayGroup>();
            billRecords.GroupToDictionary(record => record.Time.Date).ForEach(pair =>
            {
                var list = from record in pair.Value orderby record.Time.Hour descending select record;
                collection.Add(new BillRecordDayGroup(list.ToList()) {Date = pair.Key});
            });

            return collection;
        }
    }
}