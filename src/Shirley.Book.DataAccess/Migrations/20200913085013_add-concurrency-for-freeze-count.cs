using Microsoft.EntityFrameworkCore.Migrations;

namespace Shirley.Book.DataAccess.Migrations
{
    public partial class addconcurrencyforfreezecount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreezeStock",
                table: "BookStocks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FreezeStock",
                table: "BookStocks");
        }
    }
}
