using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(8)]
    public class MerchandiseOrderStatusIdx : ForwardOnlyMigration
    {
        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableMerchandiseOrder))
            {
                Create.Index("merchandise_order_status_idx")
                    .OnTable(CommonConstants.NameTableMerchandiseOrder)
                    .InSchema("public")
                    .OnColumn("status_id");
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}