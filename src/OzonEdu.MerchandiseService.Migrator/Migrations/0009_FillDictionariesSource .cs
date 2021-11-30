using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(9)]
    public class FillDictionariesSource : Migration
    {
        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableSource))
            {
                Insert.IntoTable(CommonConstants.NameTableSource)
                    .Row(new { id = 1, name = "External" })
                    .Row(new { id = 2, name = "Internal" });
            }
        }
        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableSource))
            {
                Delete.FromTable(CommonConstants.NameTableSource)
                    .Row(new { id = 1 })
                    .Row(new { id = 2 });
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}