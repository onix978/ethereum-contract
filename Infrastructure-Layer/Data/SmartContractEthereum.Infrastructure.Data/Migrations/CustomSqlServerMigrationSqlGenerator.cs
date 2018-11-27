using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Data.Entity.SqlServer;

namespace SmartContractEthereum.Infrastructure.Data.Migrations
{
    internal class CustomSqlServerMigrationSqlGenerator : SqlServerMigrationSqlGenerator
    {
        protected override void Generate(AddColumnOperation addColumnOperation)
        {
            SetCreatedUtcColumn(addColumnOperation.Column);

            base.Generate(addColumnOperation);
        }

        protected override void Generate(CreateTableOperation createTableOperation)
        {
            SetCreatedUtcColumn(createTableOperation.Columns);

            base.Generate(createTableOperation);
        }

        private static void SetCreatedUtcColumn(IEnumerable<ColumnModel> columns)
        {
            foreach (var columnModel in columns)
            {
                SetCreatedUtcColumn(columnModel);
                //SetHashColumn(columnModel); Commented because will not be used.
            }
        }

        private static void SetCreatedUtcColumn(PropertyModel column)
        {
            if (column.Name.Equals("Created"))
                column.DefaultValueSql = "GETDATE()";
        }

        private static void SetHashColumn(PropertyModel column)
        {
            if (column.Name.Equals("Hash"))
                column.DefaultValueSql = "newsequentialid()";
        }
    }
}