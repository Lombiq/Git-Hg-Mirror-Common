using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Localization;
using Orchard.Tokens;
using System;
using System.Web.Mvc;

namespace GitHgMirror.Common.Tokens
{
    public class MirroringConfigurationTokens : ITokenProvider
    {
        private readonly UrlHelper _urlHelper;
        private readonly IWorkContextAccessor _wca;

        public Localizer T { get; set; }


        public MirroringConfigurationTokens(UrlHelper urlHelper, IWorkContextAccessor wca)
        {
            _urlHelper = urlHelper;
            _wca = wca;

            T = NullLocalizer.Instance;
        }


        public void Describe(DescribeContext context) =>
            context.For(TokenNames.MirroringConfiguration, T("Mirroring Configuration"), T("Tokens for Mirroring Configuration."))
                .Token("Content", T("Content"), T("Chaining to Content tokens."), "Content")
                .Token("Direction", T("Mirroring direction"), T("The direction of the sync."))
                .Token("FailedSyncCounter", T("Failed sync counter"), T("The number of failed synchronization attempts."))
                .Token("GitCloneUrl", T("Git clone URL"), T("The full URL of the git repo."))
                .Token("HgCloneUrl", T("Hg clone URL"), T("The full URL of the hg repo."))
                .Token("EditUrl", T("The edit URL of the Mirroring configuration"), T("The edit URL of the Mirroring configuration"));

        public void Evaluate(EvaluateContext context) =>
            context.For<IContent>(TokenNames.MirroringConfiguration, () => null)
                .Chain("Content", "Content", item => item.ContentItem)
                .Token("Direction", content => content.As<MirroringConfigurationPart>().Direction)
                .Token("FailedSyncCounter", content => content.As<MirroringConfigurationPart>().FailedSyncCounter)
                .Token("GitCloneUrl", content => content.As<MirroringConfigurationPart>().GitCloneUrl)
                .Token("HgCloneUrl", content => content.As<MirroringConfigurationPart>().HgCloneUrl)
                .Token("EditUrl", content => new Uri(new Uri(_wca.GetContext().CurrentSite.BaseUrl), _urlHelper.Action(nameof(Controllers.MirroringConfigurationController.Edit), new { area = "GitHgMirror.Common", controller = "MirroringConfiguration", id= content.Id })));
    }
}