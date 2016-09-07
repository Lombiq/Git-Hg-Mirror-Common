using Orchard.ContentManagement;
using System.Collections.Generic;

namespace GitHgMirror.Common.Models.ViewModels
{
    public class MirroringConfigurationsViewModel
    {
        public IEnumerable<ContentItem> OwnMirroringConfigurations { get; set; }
    }
}