using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(10)]
    public class FillDictionariesStatus : Migration
    {
        private readonly string NameTable = "status";
        public override void Up()
        {
            if (TableExists(NameTable))
            {
                Insert.IntoTable(NameTable)
                    .Row(new { id = 1, name = "New" })
                    .Row(new { id = 2, name = "InQueue" })
                    .Row(new { id = 3, name = "Done" })
                    .Row(new { id = 4, name = "Notified" });
            }
        }
        public override void Down()
        {
            if (TableExists(NameTable))
            {
                Delete.FromTable(NameTable)
                    .Row(new { id = 1 })
                    .Row(new { id = 2 })
                    .Row(new { id = 3 })
                    .Row(new { id = 4 });
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}