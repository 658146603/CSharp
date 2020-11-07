using System;
using SQLite;

namespace BalanceYourIO
{
    public class BillRecord
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public DateTime Time { get; set; }
        public BillType BillType { get; set; }
        public float Amount { get; set; }
        public string Remark { get; set; }
        
    }
}