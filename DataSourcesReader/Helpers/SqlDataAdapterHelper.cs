using System.Data;
using System.Data.SqlClient;

namespace DataSourcesReaders.Helpers
{
    public static class SqlDataAdapterHelper
    {
        public static DataTable GetAsDataTable(this SqlDataAdapter adapter)
        {
            var dataTable = new DataTable();
            adapter.Fill(dataTable);

            return dataTable;
        }
    }
}
