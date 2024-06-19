using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aeternaCaptions.Migrations
{
    /// <inheritdoc />
    public partial class Correctedlanguagesprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LanguageCode",
                table: "Transcripts",
                newName: "NativeLanguageCode");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "SubtitleFiles",
                newName: "SubtitleLanguage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NativeLanguageCode",
                table: "Transcripts",
                newName: "LanguageCode");

            migrationBuilder.RenameColumn(
                name: "SubtitleLanguage",
                table: "SubtitleFiles",
                newName: "Language");
        }
    }
}
