using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using GitHgMirror.CommonTypes;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using Orchard.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GitHgMirror.Common.Controllers.Api
{
    public class MirroringsController : ApiController
    {
        private readonly IContentManager _contentManager;
        private readonly IAppConfigurationAccessor _appConfigurationAccessor;


        public MirroringsController(IContentManager contentManager, IAppConfigurationAccessor appConfigurationAccessor)
        {
            _contentManager = contentManager;
            _appConfigurationAccessor = appConfigurationAccessor;
        }


        public IEnumerable<MirroringConfiguration> Get(string password, int skip, int take)
        {
            ThrowIfPasswordInvalid(password);

            return _contentManager
                .Query(ContentTypes.MirroringConfiguration)
                .Slice(skip, take)
                .Select(item =>
                    {
                        var configurationPart = item.As<MirroringConfigurationPart>();

                        return new MirroringConfiguration
                        {
                            Id = item.Id,
                            Direction = (MirroringDirection)Enum.Parse(typeof(MirroringDirection), configurationPart.Direction),
                            HgCloneUri = new Uri(configurationPart.HgCloneUrl),
                            GitCloneUri = new Uri(configurationPart.GitCloneUrl),
                            GitUrlIsHgUrl = configurationPart.GitUrlIsHgUrl
                        };
                    });
        }

        public int GetCount(string password)
        {
            ThrowIfPasswordInvalid(password);

            return _contentManager
                .Query(ContentTypes.MirroringConfiguration)
                .Count();
        }

        public void PostReport(string password, MirroringStatusReport report)
        {
            ThrowIfPasswordInvalid(password);

            var mirroringConfiguration = _contentManager.Get(report.ConfigurationId);
            if (mirroringConfiguration == null && mirroringConfiguration.ContentType != ContentTypes.MirroringConfiguration) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var mirroringConfigurationPart = mirroringConfiguration.As<MirroringConfigurationPart>();
            mirroringConfigurationPart.Status = report.Status.ToString();
            mirroringConfigurationPart.StatusCode = report.Code;
            mirroringConfigurationPart.StatusMessage = report.Message;
        }

        private void ThrowIfPasswordInvalid(string password)
        {
            if (!PasswordIsValid(password)) throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }

        private bool PasswordIsValid(string password)
        {
            var configuredPassword = _appConfigurationAccessor.GetConfiguration(AppConfigurationKeys.ApiPassword);

            Argument.ThrowIfNullOrEmpty(configuredPassword, AppConfigurationKeys.ApiPassword);

            return password == configuredPassword;
        }
    }
}