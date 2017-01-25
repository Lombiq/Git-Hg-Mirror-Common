using GitHgMirror.Common.Constants;
using Orchard.Localization;
using Orchard.Workflows.Models;
using Orchard.Workflows.Services;
using System.Collections.Generic;

namespace GitHgMirror.Common.Activities
{
    public class SendSyncFailedEmailActivity : Event
    {
        public Localizer T { get; set; }

        public override string Name => ActivityNames.SendSyncFailedEmail;

        public override LocalizedString Category => T("GitHgMirror");

        public override LocalizedString Description => T("Send sync failed number exceeded the allowed number email.");

        public override bool CanExecute(WorkflowContext workflowContext, ActivityContext activityContext) => true;

        public override IEnumerable<LocalizedString> GetPossibleOutcomes(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return T("Done");
        }

        public override IEnumerable<LocalizedString> Execute(WorkflowContext workflowContext, ActivityContext activityContext)
        {
            yield return T("Done");
        }

        public override bool CanStartWorkflow => true;
    }
}