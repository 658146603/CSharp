using System;
using SQLite;

namespace BalanceYourIO
{
    public class BillRecord
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public DateTime Time { get; set; }
        public string BillType { get; set; }
        public double Amount { get; set; }
        public string Remark { get; set; }

        public override string ToString()
        {
            return $"{Id}, {DateConverter.ToFriendDateTimeString(Time)}, {BillType}, {Amount}, {Remark}";
        }
    }
}