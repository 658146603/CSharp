using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.Support.V4.App;
using SQLite;

namespace BalanceYourIO
{
    public static class DataProvider
    {
        public enum IoType
        {
            Unset = -1,
            Outcome = 0,
            Income = 1,
            Other = 2
        }

        public enum TimeMode
        {
            Day = 0,
            Week = 1,
            Month = 2,
            Year = 3
        }

        public static readonly List<BillType> BillTypes = new List<BillType>
        {
            new BillType {Id = "b4b5fe2d-e48f-4a90-9ba3-5c4905158a22", Name = "餐饮", IoType = IoType.Outcome, Icon = "drawable/type_food.png"},
            new BillType {Id = "bb944e95-cdde-4af3-8073-affa1fcbce60", Name = "娱乐", IoType = IoType.Outcome, Icon = "drawable/type_game.png"},
            new BillType {Id = "6fae281a-b3ec-4cd4-819b-24562831afef", Name = "水果", IoType = IoType.Outcome, Icon = "drawable/type_fruit.png"},
            new BillType {Id = "cbee01df-396c-41c0-ae37-49df8a9b435c", Name = "购物", IoType = IoType.Outcome, Icon = "drawable/type_shopping.png"},
            new BillType {Id = "21b7be15-727d-44d8-9d8c-9536e9a5397d", Name = "交通", IoType = IoType.Outcome, Icon = "drawable/type_transportation.png"},
            new BillType {Id = "cf53db5e-788f-4cdb-83d3-11249e57ec26", Name = "教育", IoType = IoType.Outcome, Icon = "drawable/type_education.png"},
            new BillType {Id = "465a0b40-992d-44ec-a50d-5f8cad4c2c0d", Name = "投资", IoType = IoType.Outcome, Icon = "drawable/type_investment.png"},
            new BillType {Id = "7f9bded4-09c3-4949-8075-982f91cdced6", Name = "其他", IoType = IoType.Outcome, Icon = "drawable/type_others.png"},
            new BillType {Id = "6984b735-f6f5-4cdd-8c9d-c6050e91f6fc", Name = "兼职", IoType = IoType.Income, Icon = "drawable/type_part_time.png"},
            new BillType {Id = "e5e18f6e-556e-47ea-9144-13d407dc491e", Name = "工资", IoType = IoType.Income, Icon = "drawable/type_wage.png"},
            new BillType {Id = "6bd0c873-952b-4a2b-bb48-45d21028318e", Name = "利息", IoType = IoType.Income, Icon = "drawable/type_interest.png"},
            new BillType {Id = "09f7a7ad-71d5-4554-b113-8568225c2993", Name = "其他", IoType = IoType.Income, Icon = "drawable/type_others.png"},
            new BillType {Id = "960e16ef-56d8-4a2c-a41e-a13534bd5bcf", Name = "还款", IoType = IoType.Other, Icon = "drawable/type_repayment.png"},
            new BillType {Id = "cb50fae9-ff8a-4203-8e9f-9901044e3695", Name = "充值", IoType = IoType.Other, Icon = "drawable/type_repayment.png"},
            new BillType {Id = "dbebdac2-5ed1-4a22-9ef3-6d0cc522ad79", Name = "其他", IoType = IoType.Other, Icon = "drawable/type_others.png"}
        };

        public class Database
        {
            readonly SQLiteAsyncConnection _database;

            public Database(string dbPath)
            {
                _database = new SQLiteAsyncConnection(dbPath);
                _database.CreateTableAsync<BillRecord>().Wait();
            }

            public Task<List<BillRecord>> GetBillRecordsAsync()
            {
                return _database.Table<BillRecord>().ToListAsync();
            }

            public Task<int> SaveBillRecordAsync(BillRecord billRecord)
            {
                return _database.InsertAsync(billRecord);
            }
        }
    }
}