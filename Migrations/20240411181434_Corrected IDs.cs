using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace aeternaCaptions.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedIDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubtitleFiles_Transcripts_TranscriptInternalId",
                table: "SubtitleFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transcripts",
                table: "Transcripts");

            migrationBuilder.DropIndex(
                name: "IX_SubtitleFiles_TranscriptInternalId",
                table: "SubtitleFiles");

            migrationBuilder.DropColumn(
                name: "InternalId",
                table: "Transcripts");

            migrationBuilder.DropColumn(
                name: "TranscriptInternalId",
                table: "SubtitleFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Transcripts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Transcripts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Transcripts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "SubtitleFiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TranscriptId",
                table: "SubtitleFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transcripts",
                table: "Transcripts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SubtitleFiles_TranscriptId",
                table: "SubtitleFiles",
                column: "TranscriptId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubtitleFiles_Transcripts_TranscriptId",
                table: "SubtitleFiles",
                column: "TranscriptId",
                principalTable: "Transcripts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubtitleFiles_Transcripts_TranscriptId",
                table: "SubtitleFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transcripts",
                table: "Transcripts");

            migrationBuilder.DropIndex(
                name: "IX_SubtitleFiles_TranscriptId",
                table: "SubtitleFiles");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Transcripts");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Transcripts");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "SubtitleFiles");

            migrationBuilder.DropColumn(
                name: "TranscriptId",
                table: "SubtitleFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Transcripts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "InternalId",
                table: "Transcripts",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "TranscriptInternalId",
                table: "SubtitleFiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transcripts",
                table: "Transcripts",
                column: "InternalId");

            migrationBuilder.CreateIndex(
                name: "IX_SubtitleFiles_TranscriptInternalId",
                table: "SubtitleFiles",
                column: "TranscriptInternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubtitleFiles_Transcripts_TranscriptInternalId",
                table: "SubtitleFiles",
                column: "TranscriptInternalId",
                principalTable: "Transcripts",
                principalColumn: "InternalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
