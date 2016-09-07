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
                    .WithPart(typeof(MirroringConfigurationPart).Name)
                    .WithPart(typeof(JavaScriptAntiSpamPart).Name)
                    .WithPart("TitlePart")
                    .WithPart("CommonPart",
                        part => part
                            .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "False")
                            .WithSetting("DateEditorSettings.ShowDateEditor", "False"))
                );

            return 1;
        }
    }
}