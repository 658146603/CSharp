using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Xamarin.Forms;

namespace BalanceYourIO
{
    public static class DateConverter
    {
        public static readonly List<int> Dawn = new List<int> {0, 1, 2, 3, 4, 5};

        public static readonly List<int> Morning = new List<int> {6, 7, 8, 9, 10};

        public static readonly List<int> Noon = new List<int> {11, 12};

        public static readonly List<int> Afternoon = new List<int> {13, 14, 15, 16, 17, 18};

        public static readonly List<int> Evening = new List<int> {19, 20, 21, 22, 23};

        private static readonly Dictionary<DayOfWeek, string> WeekName = new Dictionary<DayOfWeek, string>
        {
            {DayOfWeek.Sunday, "星期日"},
            {DayOfWeek.Monday, "星期一"},
            {DayOfWeek.Tuesday, "星期二"},
            {DayOfWeek.Wednesday, "星期三"},
            {DayOfWeek.Thursday, "星期四"},
            {DayOfWeek.Friday, "星期五"},
            {DayOfWeek.Saturday, "星期六"}
        };


        public static string ToFriendDateTimeString(DateTime dateTime)
        {
            var today = DateTime.Now;
            var dayDetail = ToDayDetail(dateTime.Hour);
            var week = WeekName[dateTime.DayOfWeek];
            var date = dateTime.ToString("MM月dd日");
            var year = dateTime.Year != today.Year ? $"{dateTime.Year}年" : "";

            string final;

            if (dateTime.Year == today.Year && dateTime.DayOfYear == today.DayOfYear)
            {
                final = $"{year}{date}(今天) {dayDetail}";
            }
            else
            {
                final = $"{year}{date} {dayDetail}";
            }

            return final;
        }

        public static string ToFriendDateString(DateTime dateTime)
        {
            var today = DateTime.Now;
            var week = WeekName[dateTime.DayOfWeek];
            var date = dateTime.ToString("MM月dd日");
            var year = dateTime.Year != today.Year ? $"{dateTime.Year}年" : "";

            string final;

            if (dateTime.Year == today.Year && dateTime.DayOfYear == today.DayOfYear)
            {
                final = $"{year}{date}(今天) {week}";
            }
            else
            {
                final = $"{year}{date} {week}";
            }

            return final;
        }

        private static string ToDayDetail(int hour)
        {
            if (Dawn.Contains(hour))
            {
                return "凌晨";
            }

            if (Morning.Contains(hour))
            {
                return "上午";
            }

            if (Noon.Contains(hour))
            {
                return "中午";
            }

            if (Afternoon.Contains(hour))
            {
                return "下午";
            }

            if (Evening.Contains(hour))
            {
                return "晚上";
            }

            return "未知";
        }
    }

    internal class DateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateConverter.ToFriendDateTimeString(value as DateTime? ?? DateTime.Now);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}