using GitHgMirror.CommonTypes;

namespace GitHgMirror.Common.Models.ViewModels
{
    public class ConfigurationStatusViewModel
    {
        public MirroringStatus Status { get; set; }
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
    }
}