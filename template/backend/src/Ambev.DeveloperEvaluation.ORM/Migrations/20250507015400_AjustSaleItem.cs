using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class AjustSaleItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "SalesItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "SalesItems");
        }
    }
}
