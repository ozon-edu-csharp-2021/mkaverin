using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(6)]
    public class MerchandiseOrderStatusIdx : ForwardOnlyMigration
    {
        private readonly string NameTable = "merchandise_order";
        public override void Up()
        {
            if (TableExists(NameTable))
            {
                Create.Index("merchandise_order_status_idx")
                    .OnTable(NameTable)
                    .InSchema("public")
                    .OnColumn("status_id");
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}