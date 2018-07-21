namespace DataSourcesReaders.Models
{
    public class TestSourceWrapper<TTable, TRow>
    {
        public TTable Table;
        public TRow Row;

        public TestSourceWrapper(TTable table, TRow row)
        {
            Table = table;
            Row = row;
        }
    }
}
