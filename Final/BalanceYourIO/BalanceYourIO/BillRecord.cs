using System;
using SQLite;

namespace BalanceYourIO
{
    public class BillRecord
    {
        [PrimaryKey, AutoIncrement] int Id { get; set; }
        DateTime Time { get; set; }
        BillType BillType { get; set; }
        private float Amount { get; set; }
        private string Remark { get; set; }
    }
}