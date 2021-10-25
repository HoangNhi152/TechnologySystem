namespace TechnologySystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Data_Types_DateofBirth_of_AppplicationUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "DateofBirth", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "DateofBirth", c => c.String());
        }
    }
}
