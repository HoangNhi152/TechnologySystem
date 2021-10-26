namespace TechnologySystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAssignCourseTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssignCourses",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CourseId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.CourseId })
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.CourseId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AssignCourses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AssignCourses", "CourseId", "dbo.Courses");
            DropIndex("dbo.AssignCourses", new[] { "CourseId" });
            DropIndex("dbo.AssignCourses", new[] { "UserId" });
            DropTable("dbo.AssignCourses");
        }
    }
}
