using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using Orchard.AntiSpam.Models;
using Orchard.ContentManagement.MetaData;
using Orchard.Data.Migration;

namespace GitHgMirror.Common.Migrations
{
    public class MirroringConfigurationMigrations : DataMigrationImpl
    {
        public int Create()
        {
            ContentDefinitionManager.AlterTypeDefinition(ContentTypes.MirroringConfiguration,
                cfg => cfg
                    .DisplayedAs("Mirroring Configuration")
                    .WithPart(nameof(MirroringConfigurationPart))
                    .WithPart(nameof(JavaScriptAntiSpamPart))
                    .WithPart("TitlePart")
                    .WithPart("CommonPart",
                        part => part
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False")
                            .WithSetting("DateEditorSettings.ShowDateEditor", "False"))
                    .WithPart("IdentityPart")
                );

            SchemaBuilder.CreateTable(nameof(MirroringConfigurationPartRecord),
                table => table
                    .ContentPartRecord()
                    .Column<string>(nameof(MirroringConfigurationPartRecord.Status))
            )
            .AlterTable(nameof(MirroringConfigurationPartRecord),
                table => table.CreateIndex(nameof(MirroringConfigurationPartRecord.Status), nameof(MirroringConfigurationPartRecord.Status)));

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.CreateTable(nameof(MirroringConfigurationPartRecord),
                table => table
                    .ContentPartRecord()
                    .Column<string>(nameof(MirroringConfigurationPartRecord.Status))
            )
            .AlterTable(nameof(MirroringConfigurationPartRecord),
                table => table.CreateIndex(nameof(MirroringConfigurationPartRecord.Status), nameof(MirroringConfigurationPartRecord.Status)));

            return 2;
        }

        public int UpdateFrom2()
        {
            ContentDefinitionManager.AlterTypeDefinition(ContentTypes.MirroringConfiguration,
                cfg => cfg
                    .WithPart("IdentityPart")
                );

            return 3;
        }
    }
}