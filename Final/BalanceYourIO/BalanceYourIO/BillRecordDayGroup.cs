using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace BalanceYourIO
{
    public class BillRecordDayGroup : ObservableCollection<BillRecordDetail>
    {
        public string Date => DateConverter.ToFriendDateString(_date);

        public DateTime _date;

        public double Income { get; set; } = 0f;

        public double Outcome { get; set; } = 0f;

        public BillRecordDayGroup(List<BillRecord> billRecords)
        {
            foreach (var record in billRecords)
            {
                var detail = new BillRecordDetail(record);
                Add(detail);
                if (detail.Type.IoType == DataProvider.IoType.Income)
                {
                    Income += detail.Amount;
                }
                else if (detail.Type.IoType == DataProvider.IoType.Outcome)
                {
                    Outcome += detail.Amount;
                }
            }
        }

        public static IEnumerable<BillRecordDayGroup> ConvertAll(List<BillRecord> billRecords)
        {
            var dateGroup =
                from billRecord in billRecords
                group billRecord by billRecord.Time.Date
                into g
                orderby g.Key descending
                select g;
            
            var groups =
                from item in dateGroup
                select new BillRecordDayGroup(item.OrderByDescending(detail => detail.Time.Hour)
                    .ThenByDescending(detail => detail.Id).ToList()) {_date = item.Key};

            return groups;
            
            
            // List<BillRecordDayGroup> collection = new List<BillRecordDayGroup>();
            //
            // billRecords.GroupToDictionary(record => record.Time.Date).ForEach(pair =>
            // {
            //     var list = from record in pair.Value
            //         orderby record.Time.Hour descending, record.Id descending
            //         select record;
            //     var group = new BillRecordDayGroup(list.ToList()) {_date = pair.Key};
            //     collection.Add(group);
            // });
            //
            // return new ObservableCollection<BillRecordDayGroup>(collection.OrderByDescending(group => @group._date));
        }
    }
}