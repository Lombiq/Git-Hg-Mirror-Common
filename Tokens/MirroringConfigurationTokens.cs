using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Localization;
using Orchard.Security;
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
                .Token("Direction", T("Mirroring direction"), T("The content item this comment was created on."))
                .Token("FailedSyncCounter", T("Failed sync counter"), T("The number of failed synchronization attempts."))
                .Token("GitCloneUrl", T("Git clone URL"), T("The full url of the git repo."))
                .Token("HgCloneUrl", T("Hg clone URL"), T("The full url of the hg repo."))
                .Token("OwnerUserName", T("Mirroring Configuration owner's username"), T("The username of the owner of the Mirroring Configuration."))
                .Token("OwnerEmail", T("Mirroring Configuration owner's email address"), T("The email address of the owner of the Mirroring Configuration."));

        public void Evaluate(EvaluateContext context) =>
            context.For<IContent>(TokenNames.MirroringConfiguration, () => null)
                .Chain("Content", "Content", item => item.ContentItem)
                .Token("Direction", content => content.As<MirroringConfigurationPart>().Direction)
                .Token("FailedSyncCounter", content => content.As<MirroringConfigurationPart>().FailedSyncCounter)
                .Token("GitCloneUrl", content => content.As<MirroringConfigurationPart>().GitCloneUrl)
                .Token("HgCloneUrl", content => content.As<MirroringConfigurationPart>().HgCloneUrl)
                .Token("OwnerUserName", MirroringConfigurationAuthorUserName)
                .Token("OwnerEmail", MirroringConfigurationAuthorEmailAddress);


        private static IUser MirroringConfigurationAuthor(IContent mirroringConfiguration) =>
            mirroringConfiguration.As<CommonPart>().Owner;

        private static string MirroringConfigurationAuthorEmailAddress(IContent mirroringConfiguration) =>
            MirroringConfigurationAuthor(mirroringConfiguration).Email;

        private static string MirroringConfigurationAuthorUserName(IContent mirroringConfiguration) =>
            MirroringConfigurationAuthor(mirroringConfiguration).UserName;
    }
}