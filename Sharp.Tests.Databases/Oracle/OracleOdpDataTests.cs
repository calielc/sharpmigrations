using System;
using NUnit.Framework;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Schema;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
	[TestFixture]
	public class OracleOdpDataTests : DataClientDataTests {
        [SetUp]
        public virtual void SetUp() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.OracleOdp);
        }

		public override void Can_insert_dates_and_booleans() {
            _dataClient.AddTable("footable", null,
		                         Column.AutoIncrement("id"),
		                         Column.Date("colDate"),
		                         Column.Boolean("colBool"));


			DateTime now = DateTime.Now;
			_dataClient.Insert.Into("footable").Columns("colDate", "colBool").Values(now, true);
			ResultSet res = _dataClient.Select.Columns("colDate", "colBool").From("footable").AllRows();
			Assert.AreEqual(now.ToString(), res[0][0].ToString());
			Assert.AreEqual(1, res[0][1]);
		}
    }
}