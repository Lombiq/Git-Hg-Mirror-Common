using GitHgMirror.Common.Models;
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
    }
}