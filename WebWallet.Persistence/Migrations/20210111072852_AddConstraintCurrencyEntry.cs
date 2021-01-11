using Microsoft.EntityFrameworkCore.Migrations;

namespace WebWallet.Persistence.Migrations
{
    public partial class AddConstraintCurrencyEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CurrencyEntries_CurrencyId",
                table: "CurrencyEntries");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyEntries_CurrencyId_WalletId",
                table: "CurrencyEntries",
                columns: new[] { "CurrencyId", "WalletId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CurrencyEntries_CurrencyId_WalletId",
                table: "CurrencyEntries");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyEntries_CurrencyId",
                table: "CurrencyEntries",
                column: "CurrencyId");
        }
    }
}
