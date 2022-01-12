using GitHgMirror.Common.Controllers;
using GitHgMirror.Common.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;

namespace GitHgMirror.Common.Handlers
{
    public class MirroringConfigurationPartHandler : ContentHandler
    {
        public MirroringConfigurationPartHandler(IRepository<MirroringConfigurationPartRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            var mirroringConfigurationPart = context.ContentItem.As<MirroringConfigurationPart>();

            if (mirroringConfigurationPart == null) return;

            context.Metadata.DisplayRouteValues = context.Metadata.EditorRouteValues =
                MirroringConfigurationController.GetEditorRouteValues(mirroringConfigurationPart.Id);
        }
    }
}