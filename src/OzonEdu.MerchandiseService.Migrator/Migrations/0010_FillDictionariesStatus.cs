using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(10)]
    public class FillDictionariesStatus : Migration
    {
        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableStatus))
            {
                Insert.IntoTable(CommonConstants.NameTableStatus)
                    .Row(new { id = 1, name = "New" })
                    .Row(new { id = 2, name = "InQueue" })
                    .Row(new { id = 3, name = "Done" })
                    .Row(new { id = 4, name = "Notified" })
                    .Row(new { id = 5, name = "Decline" });
            }
        }
        public override void Down()
        {
            if (TableExists(CommonConstants.NameTableStatus))
            {
                Delete.FromTable(CommonConstants.NameTableStatus)
                    .Row(new { id = 1 })
                    .Row(new { id = 2 })
                    .Row(new { id = 3 })
                    .Row(new { id = 4 })
                    .Row(new { id = 5 });
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}