using System;
using System.Collections.Generic;
using System.Linq;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPage : ContentPage
    {
        //历年收支状况折线图数据源
        private ChartEntry[] _yearsOutcomeViewLineChartEntry;
        private ChartEntry[] _yearsIncomeViewLineChartEntry;

        //年内，月内收支状况数据源
        private ChartEntry[] _yearTypeViewPieChartEntry;
        private ChartEntry[] _monthTypeViewPieChartEntry;


        private int _year = DateTime.Now.Year;
        private int _month = DateTime.Now.Month;

        //当前选中的年份
        private int Year
        {
            get => _year;
            set
            {
                _year = value;
                YearLabel.Text = $"{_year}年";
                Month = Month;
                YearTypeViewPieChartEntry = TypeYearEntry(Year);
            }
        }

        //当前选中的月份
        private int Month
        {
            get => _month;
            set
            {
                _month = value;
                MonthLabel.Text = $"{_month}月";
                MonthTypeViewPieChartEntry = TypeMonthEntry(Year, Month);
            }
        }

        private ChartEntry[] YearsOutcomeViewLineChartEntry
        {
            get => _yearsOutcomeViewLineChartEntry;
            set
            {
                _yearsOutcomeViewLineChartEntry = value;
                YearsOutcomeLineChart.Chart.Entries = _yearsOutcomeViewLineChartEntry;
            }
        }

        private ChartEntry[] YearsIncomeViewLineChartEntry
        {
            get => _yearsIncomeViewLineChartEntry;
            set
            {
                _yearsIncomeViewLineChartEntry = value;
                YearsIncomeLineChart.Chart.Entries = _yearsIncomeViewLineChartEntry;
            }
        }

        private ChartEntry[] YearTypeViewPieChartEntry
        {
            get => _yearTypeViewPieChartEntry;
            set
            {
                _yearTypeViewPieChartEntry = value;
                YearPieChart.Chart.Entries = _yearTypeViewPieChartEntry;
            }
        }

        private ChartEntry[] MonthTypeViewPieChartEntry
        {
            get => _monthTypeViewPieChartEntry;
            set
            {
                _monthTypeViewPieChartEntry = value;
                MonthPieChart.Chart.Entries = _monthTypeViewPieChartEntry;
            }
        }

        // private ChartEntry[] SubOutcomeViewBarChartEntry { get; set; }
        // private ChartEntry[] SubIncomeViewBarChartEntry { get; set; }

        private IEnumerable<BillRecordDetail> BillRecordDetails { get; set; } = new List<BillRecordDetail>();

        public GraphPage()
        {
            InitializeComponent();

            //初始化和配置图表
            YearsOutcomeLineChart.Chart = new LineChart()
            {
                LineMode = LineMode.Spline,
                LineSize = 2,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.Circle,
                PointSize = 6,
                IsAnimated = false
            };

            YearsIncomeLineChart.Chart = new LineChart()
            {
                LineMode = LineMode.Spline,
                LineSize = 2,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.Circle,
                PointSize = 6,
                IsAnimated = false
            };

            YearPieChart.Chart = new PieChart()
            {
                IsAnimated = false,
            };

            MonthPieChart.Chart = new PieChart()
            {
                IsAnimated = false,
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var data = App.Database.GetBillRecordsAsync().Result;
            BillRecordDetails = from billRecord in data select new BillRecordDetail(billRecord);

            var (incomeList, outcomeList) = IncomeOutcomeYearsEntry();

            YearsIncomeViewLineChartEntry = incomeList;
            YearsOutcomeViewLineChartEntry = outcomeList;

            Year = DateTime.Now.Year;
            Month = DateTime.Now.Month;
        }

        //获取数据源
        private (ChartEntry[] incomeList, ChartEntry[] outcomeList) IncomeOutcomeYearsEntry()
        {
            var yearGroups =
                from detail in BillRecordDetails
                group detail by detail.Time.Year
                into yearRecord
                orderby yearRecord.Key
                select yearRecord;

            var outcomeList = new List<ChartEntry>();
            var incomeList = new List<ChartEntry>();

            foreach (var yearGroup in yearGroups)
            {
                var year = yearGroup.Key;
                var yearIncome = yearGroup.ToList().Sum(detail =>
                    detail.Type.IoType == DataProvider.IoType.Income ? detail.Amount : 0);

                var yearOutcome = yearGroup.ToList().Sum(detail =>
                    detail.Type.IoType == DataProvider.IoType.Outcome ? detail.Amount : 0);

                outcomeList.Add(new ChartEntry((float) Math.Log(yearOutcome + 1))
                {
                    Label = $"{year}",
                    ValueLabel = $"{yearOutcome:F2}",
                    TextColor = SKColors.Black,
                    Color = SKColors.Orange
                });
                incomeList.Add(new ChartEntry((float) Math.Log(yearIncome + 1))
                {
                    Label = $"{year}",
                    ValueLabel = $"{yearIncome:F2}",
                    TextColor = SKColors.Black,
                    Color = SKColors.Green
                });
            }

            return (incomeList.ToArray(), outcomeList.ToArray());
        }

        private (List<ChartEntry> incomeList, List<ChartEntry> outcomeList) IncomeOutcomeMonthEntry(int selectedYear)
        {
            IOrderedEnumerable<IGrouping<int, BillRecordDetail>> monthGroups =
                from detail in BillRecordDetails
                where detail.Date.Year == selectedYear
                group detail by detail.Date.Month
                into monthRecord
                orderby monthRecord.Key
                select monthRecord;

            var outcomeList = new List<ChartEntry>();
            var incomeList = new List<ChartEntry>();

            foreach (var monthGroup in monthGroups)
            {
                var month = monthGroup.Key;
                var monthIncome = monthGroup.ToList().Sum(detail =>
                    detail.Type.IoType == DataProvider.IoType.Income ? detail.Amount : 0);

                var monthOutcome = monthGroup.ToList().Sum(detail =>
                    detail.Type.IoType == DataProvider.IoType.Outcome ? detail.Amount : 0);

                outcomeList.Add(new ChartEntry((float) Math.Log(monthOutcome + 1))
                {
                    Label = $"{month}",
                    ValueLabel = $"{monthOutcome:F2}",
                    TextColor = SKColors.Black,
                    Color = SKColors.Orange
                });
                incomeList.Add(new ChartEntry((float) Math.Log(monthIncome + 1))
                {
                    Label = $"{month}",
                    ValueLabel = $"{monthIncome:F2}",
                    TextColor = SKColors.Black,
                    Color = SKColors.Green
                });
            }

            return (incomeList, outcomeList);
        }

        private ChartEntry[] TypeMonthEntry(int year, int month)
        {
            var dateGroups =
                from detail in BillRecordDetails
                where (detail.Time.Month == month && detail.Time.Year == year)
                group detail by detail.Type
                into monthTypeRecord
                select monthTypeRecord;

            var typedDict = dateGroups.ToDictionary(
                details => details.Key,
                details => details.Sum(detail => detail.Amount));

            var typeMonthList = new List<ChartEntry>();

            foreach (var group in typedDict)
            {
                var chartEntry = new ChartEntry((float) Math.Log(group.Value + 1))
                {
                    ValueLabel = $"{group.Value:F2}",
                    Label = group.Key.EnName,
                    Color = group.Key.Color,
                    ValueLabelColor = group.Key.Color,
                    TextColor = group.Key.Color
                };

                typeMonthList.Add(chartEntry);
            }

            return typeMonthList.ToArray();
        }

        private ChartEntry[] TypeYearEntry(int year)
        {
            var typeGroups =
                from detail in BillRecordDetails
                where detail.Time.Year == year
                group detail by detail.Type
                into typeRecord
                orderby typeRecord.Key
                select typeRecord;

            var typedDict = typeGroups.ToDictionary(
                details => details.Key,
                details => details.Sum(detail => detail.Amount));
            var typeYearList = new List<ChartEntry>();

            foreach (var group in typedDict)
            {
                var chartEntry = new ChartEntry((float) Math.Log(group.Value + 1))
                {
                    ValueLabel = $"{group.Value:F2}",
                    Label = group.Key.EnName,
                    Color = group.Key.Color,
                    ValueLabelColor = group.Key.Color,
                    TextColor = group.Key.Color
                };

                typeYearList.Add(chartEntry);
            }

            return typeYearList.ToArray();
        }

        private void YearInc_OnClicked(object sender, EventArgs e)
        {
            Year++;
        }

        private void YearDec_OnClicked(object sender, EventArgs e)
        {
            Year--;
        }

        private void MonthDec_OnClicked(object sender, EventArgs e)
        {
            if (Month <= 1)
            {
                Month = 1;
            }
            else
            {
                Month--;
            }
        }

        private void MonthInc_OnClicked(object sender, EventArgs e)
        {
            if (Month >= 12)
            {
                Month = 12;
            }
            else
            {
                Month++;
            }
        }
    }
}