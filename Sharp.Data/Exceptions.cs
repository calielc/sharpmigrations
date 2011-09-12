using System;
using System.Data;
using System.Linq;
using System.Data.Common;
using System.Text;
using log4net;
using System.Reflection;

namespace Sharp.Data {
    
    public class DataTypeNotAvailableException : Exception {
        public DataTypeNotAvailableException(string message) : base(message) { }
    }

    public class NotSupportedByDialect : Exception {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public string FunctionName { get; set; }
        public string DialectName { get; set; }

        public NotSupportedByDialect(string message, string functionName, string dialectName) : base(message) {
            FunctionName = functionName;
            DialectName = dialectName;
            Log.Error(String.Format("Dataclient error: operation {0} not supported by {1}", functionName, dialectName));
        }
    }

    public class ProviderNotFoundException : Exception {
        public ProviderNotFoundException(string message) : base(message) { }
    }

    public class DataClientFactoryNotFoundException : Exception {
        public static bool IsX64 {
            get {
                return IntPtr.Size == 8;
            }
        }

        private static string _allFactories;

        static DataClientFactoryNotFoundException() {
            DataRowCollection rows = DbProviderFactories.GetFactoryClasses().Rows;
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in rows) {
                sb.Append("|").Append(row["InvariantName"]);
            }
            sb.Append("|");
            _allFactories = sb.ToString();
        }

        public DataClientFactoryNotFoundException(string factoryName, Exception innerException)
            : base("Could not find the DbProviderFactory named [" + factoryName + "]. Available factories are: " + _allFactories + ". Check your machine.config for " + (IsX64 ? "64" : "32") + "bits", innerException) {
        }
    }

    public class DatabaseException : Exception {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public string SQL { get; set; }

        public DatabaseException(string message, Exception innerException, string sql)
            : base(message, innerException) {
            SQL = sql;
            Log.Error(ToString());
        }

        public override string ToString() {
            return String.Format("Error running SQL: {0}\r\n{1}", SQL, base.ToString());
        }
    }

}
