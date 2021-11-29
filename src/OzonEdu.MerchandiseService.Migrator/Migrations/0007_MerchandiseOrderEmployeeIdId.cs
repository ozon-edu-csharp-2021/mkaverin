using FluentMigrator;

namespace OzonEdu.MerchandiseService.Migrator.Migrations
{
    [Migration(7)]
    public class MerchandiseOrderEmployeeEmailId : ForwardOnlyMigration
    {
        public override void Up()
        {
            if (TableExists(CommonConstants.NameTableMerchandiseOrder))
            {
                Create.Index("merchandise_order_employee_email_idx")
                    .OnTable(CommonConstants.NameTableMerchandiseOrder)
                    .InSchema("public")
                    .OnColumn("employee_email");
            }
        }
        private bool TableExists(string tableName, string tdmSchema = "public") =>
             Schema.Schema(tdmSchema).Table(tableName).Exists();
    }
}