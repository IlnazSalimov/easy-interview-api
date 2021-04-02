using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyInterview.API.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_interviews_users_OwnerId1",
                table: "interviews");

            migrationBuilder.DropIndex(
                name: "IX_interviews_OwnerId1",
                table: "interviews");

            migrationBuilder.DropColumn(
                name: "OwnerId1",
                table: "interviews");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "interviews",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_interviews_OwnerId",
                table: "interviews",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_interviews_users_OwnerId",
                table: "interviews",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_interviews_users_OwnerId",
                table: "interviews");

            migrationBuilder.DropIndex(
                name: "IX_interviews_OwnerId",
                table: "interviews");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "interviews",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId1",
                table: "interviews",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_interviews_OwnerId1",
                table: "interviews",
                column: "OwnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_interviews_users_OwnerId1",
                table: "interviews",
                column: "OwnerId1",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
