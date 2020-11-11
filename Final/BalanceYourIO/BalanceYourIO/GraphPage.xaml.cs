using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcharts;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BalanceYourIO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphPage : ContentPage
    {
        private ChartEntry[] _allOutcomeViewLineChartEntry;

        private ChartEntry[] _allIncomeViewLineChartEntry;


        private ChartEntry[] AllOutcomeViewLineChartEntry
        {
            get => _allOutcomeViewLineChartEntry;
            set
            {
                _allOutcomeViewLineChartEntry = value;
                OutcomeLineChart.Chart.Entries = _allOutcomeViewLineChartEntry;
            }
        }

        private ChartEntry[] AllIncomeViewLineChartEntry
        {
            get => _allIncomeViewLineChartEntry;
            set
            {
                _allIncomeViewLineChartEntry = value;
                IncomeLineChart.Chart.Entries = _allIncomeViewLineChartEntry;
            }
        }


        private ChartEntry[] SubOutcomeViewBarChartEntry { get; set; }

        private ChartEntry[] SubIncomeViewBarChartEntry { get; set; }

        private IEnumerable<BillRecordDetail> BillRecordDetails { get; set; } = new List<BillRecordDetail>();

        public GraphPage()
        {
            InitializeComponent();

            OutcomeLineChart.Chart = new LineChart()
            {
                LineMode = LineMode.Spline,
                LineSize = 2,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.Circle,
                PointSize = 6,
                IsAnimated = false
            };

            IncomeLineChart.Chart = new LineChart()
            {
                LineMode = LineMode.Spline,
                LineSize = 2,
                ValueLabelOrientation = Orientation.Horizontal,
                LabelOrientation = Orientation.Horizontal,
                PointMode = PointMode.Circle,
                PointSize = 6,
                IsAnimated = false
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var data = App.Database.GetBillRecordsAsync().Result;
            BillRecordDetails = from billRecord in data select new BillRecordDetail(billRecord);

            YearDataEntry();
        }

        // TODO 间断年份的处理
        private void YearDataEntry()
        {
            IOrderedEnumerable<IGrouping<int, BillRecordDetail>> yearGroups =
                from detail in BillRecordDetails
                group detail by detail.Date.Year
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

            AllIncomeViewLineChartEntry = incomeList.ToArray();
            AllOutcomeViewLineChartEntry = outcomeList.ToArray();
        }
    }
}