using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;
using System.Web.Mvc;

namespace GitHgMirror.Common.Tokens
{
    public class MirroringConfigurationTokens : ITokenProvider
    {
        private readonly UrlHelper _urlHelper;

        public Localizer T { get; set; }


        public MirroringConfigurationTokens(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context) =>
            context.For(TokenNames.MirroringConfiguration, T("Mirroring Configuration"), T("Tokens for Mirroring Configuration."))
                .Token("Content", T("Content"), T("Chaining to Content tokens."), "Content")
                .Token("FailedSyncCounter", T("Failed sync counter"), T("The number of failed synchronization attempts."))
                .Token("EditUrl", T("The edit URL of the Mirroring configuration"), T("The edit URL of the Mirroring configuration"));

        public void Evaluate(EvaluateContext context) =>
            context.For<IContent>(TokenNames.MirroringConfiguration, () => null)
                .Chain("Content", "Content", item => item.ContentItem)
                .Token("FailedSyncCounter", content => content.As<MirroringConfigurationPart>().FailedSyncCounter)
                .Token("EditUrl", content =>  _urlHelper.Action("Edit", new { area = "GitHgMirror.Common", controller = "MirroringConfiguration" }));
    }
}