using System;
using SkiaSharp;
using Xamarin.Forms;

namespace BalanceYourIO
{
    public class BillType : IComparable<BillType>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DataProvider.IoType IoType { get; set; }
        public string Icon { get; set; }

        public string EnName { get; set; }

        public SKColor Color { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {IoType}, {Icon}";
        }

        public int CompareTo(BillType other)
        {
            return other.Id == Id ? 0 : 1;
        }
    }
}