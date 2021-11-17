using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(4)]
    public class SourceTable : Migration
    {
        private readonly string NameTable = "source";
        public override void Up()
        {
            if (!TableExists(NameTable))
            {
                Create.Table(NameTable)
                    .WithColumn("id").AsInt64().Identity().PrimaryKey()
                    .WithColumn("name").AsString().NotNullable();
            }
        }

        public override void Down()
        {
            if (TableExists(NameTable))
            {
                Delete.Table(NameTable);
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}