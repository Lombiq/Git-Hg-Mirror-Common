﻿using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using GitHgMirror.CommonTypes;
using Orchard.ContentManagement;
using Orchard.Environment.Configuration;
using Orchard.Validation;
using Orchard.Workflows.Services;
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
        private readonly IWorkflowManager _workflowManager;


        public MirroringsController(
            IContentManager contentManager,
            IAppConfigurationAccessor appConfigurationAccessor,
            IWorkflowManager workflowManager)
        {
            _contentManager = contentManager;
            _appConfigurationAccessor = appConfigurationAccessor;
            _workflowManager = workflowManager;
        }


        public IEnumerable<MirroringConfiguration> Get(string password, int skip, int take)
        {
            ThrowIfPasswordInvalid(password);

            return _contentManager
                .Query(ContentTypes.MirroringConfiguration)
                .Where<MirroringConfigurationPartRecord>(record => record.Status != MirroringStatus.Disabled.ToString())
                .OrderBy<MirroringConfigurationPartRecord>(record => record.Id)
                .Slice(skip, take)
                .Select(item =>
                    {
                        var configurationPart = item.As<MirroringConfigurationPart>();

                        return new MirroringConfiguration
                        {
                            Id = item.Id,
                            Direction = configurationPart.Direction,
                            GitPushRefsRegex = configurationPart.GitPushRefsRegex,
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
                .Where<MirroringConfigurationPartRecord>(record =>
                    record.Status != MirroringStatus.Disabled.ToString())
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

            if (report.Status == MirroringStatus.Failed)
            {
                mirroringConfigurationPart.FailedSyncCounter++;

                if (mirroringConfigurationPart.FailedSyncCounter >= Constants.Configuration.MaximumNumberOfFailedSyncs)
                {
                    mirroringConfigurationPart.Status = MirroringStatus.Disabled.ToString();
                    _workflowManager.TriggerEvent(ActivityNames.MirroringGotDisabled, null,
                        () => new Dictionary<string, object>
                        {
                            { TokenNames.MirroringConfiguration, mirroringConfiguration }
                        });
                }
            }
            else if (report.Status == MirroringStatus.Syncing)
            {
                mirroringConfigurationPart.FailedSyncCounter = 0;
            }
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