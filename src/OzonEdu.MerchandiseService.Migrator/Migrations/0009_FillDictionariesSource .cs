using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(9)]
    public class FillDictionariesSource : Migration
    {
        private readonly string NameTable = "source";
        public override void Up()
        {
            if (TableExists(NameTable))
            {
                Insert.IntoTable(NameTable)
                    .Row(new { id = 1, name = "External" })
                    .Row(new { id = 2, name = "Internal" });
            }
        }
        public override void Down()
        {
            if (TableExists(NameTable))
            {
                Delete.FromTable(NameTable)
                    .Row(new { id = 1 })
                    .Row(new { id = 2 });
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}