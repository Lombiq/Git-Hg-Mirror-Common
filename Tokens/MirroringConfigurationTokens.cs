using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Localization;
using Orchard.Tokens;

namespace GitHgMirror.Common.Tokens
{
    public class MirroringConfigurationTokens : ITokenProvider
    {
        public Localizer T { get; set; }


        public MirroringConfigurationTokens()
        {
            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context) =>
            context.For(TokenNames.MirroringConfiguration, T("Mirroring Configuration"), T("Tokens for Mirroring Configuration."))
                .Token("Content", T("Content"), T("Chaining to Content tokens."), "Content")
                .Token("Direction", T("Mirroring direction"), T("The direction of the sync."))
                .Token("FailedSyncCounter", T("Failed sync counter"), T("The number of failed synchronization attempts."))
                .Token("GitCloneUrl", T("Git clone URL"), T("The full URL of the git repo."))
                .Token("HgCloneUrl", T("Hg clone URL"), T("The full URL of the hg repo."));

        public void Evaluate(EvaluateContext context) =>
            context.For<IContent>(TokenNames.MirroringConfiguration, () => null)
                .Chain("Content", "Content", item => item.ContentItem)
                .Token("Direction", content => content.As<MirroringConfigurationPart>().Direction)
                .Token("FailedSyncCounter", content => content.As<MirroringConfigurationPart>().FailedSyncCounter)
                .Token("GitCloneUrl", content => content.As<MirroringConfigurationPart>().GitCloneUrl)
                .Token("HgCloneUrl", content => content.As<MirroringConfigurationPart>().HgCloneUrl);
    }
}