using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace teamseven.PhyGen.Repository.Migrations
{
    /// <inheritdoc />
    public partial class MinusChangesSolution : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Explanation",
                table: "Solutions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Explanation",
                table: "Solutions");
        }
    }
}
