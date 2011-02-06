﻿namespace Gymnastika.Data.Migration.Commands
{
    public class AddIndexCommand : TableCommand
    {
        public string IndexName { get; set; }

        public AddIndexCommand(string tableName, string indexName, params string[] columnNames)
            : base(tableName)
        {
            ColumnNames = columnNames;
            IndexName = indexName;
        }

        public string[] ColumnNames { get; private set; }
    }
}
