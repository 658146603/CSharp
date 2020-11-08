using System;
using SQLite;
using SQLitePCL;

namespace BalanceYourIO
{
    public class BillType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DataProvider.IoType IoType { get; set; }
        public string Icon { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {IoType}, {Icon}";
        }
    }
}