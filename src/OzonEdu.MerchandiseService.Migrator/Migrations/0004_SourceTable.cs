using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(4)]
    public class SourceTable : Migration
    {
        public override void Up()
        {
            if (!TableExists(CommonConstants.NameTableSource))
            {
                Create.Table(CommonConstants.NameTableSource)
                    .WithColumn("id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("name").AsString().NotNullable();
            }
        }

        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableSource))
            {
                Delete.Table(CommonConstants.NameTableSource);
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}