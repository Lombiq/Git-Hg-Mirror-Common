using GitHgMirror.Common.Constants;
using GitHgMirror.Common.Models;
using GitHgMirror.Common.Models.ViewModels;
using GitHgMirror.CommonTypes;
using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Common.Models;
using Orchard.Localization;
using Orchard.Security;
using Orchard.Themes;
using Orchard.UI.Notify;
using System;
using System.Linq;
using System.Web.Mvc;
using Orchard.Core.Title.Models;

namespace GitHgMirror.Common.Controllers
{
    [Themed]
    [Authorize]
    public class MirroringConfigurationController : Controller, IUpdateModel
    {
        private readonly IOrchardServices _orchardServices;
        private readonly WorkContext _workContext;
        private readonly IContentManager _contentManager;
        private readonly IAuthorizer _authorizer;


        public Localizer T { get; set; }


        public MirroringConfigurationController(IOrchardServices orchardServices)
        {
            _orchardServices = orchardServices;
            _workContext = orchardServices.WorkContext;
            _contentManager = orchardServices.ContentManager;
            _authorizer = orchardServices.Authorizer;

            T = NullLocalizer.Instance;
        }


        public ActionResult Index()
        {
            var currentUser = _workContext.CurrentUser;

            var ownMirroringConfigurations = _contentManager
                .Query(ContentTypes.MirroringConfiguration)
                .Where<CommonPartRecord>(record => record.OwnerId == currentUser.Id)
                .Join<TitlePartRecord>()
                .OrderBy<TitlePartRecord>(record => record.Title)
                .Slice(250); // Hard limit for safety on how many configs you can have. We could have paging instead.

            if (ownMirroringConfigurations.Any() && !_authorizer.Authorize(Orchard.Core.Contents.Permissions.ViewContent, ownMirroringConfigurations.First(), T("You are not allowed to view users' Mirroring Configurations."))) return new HttpUnauthorizedResult();

            return View(new MirroringConfigurationsViewModel
            {
                OwnMirroringConfigurations = ownMirroringConfigurations
            });
        }

        public ActionResult Create()
        {
            var mirroringConfiguration = _contentManager.New(ContentTypes.MirroringConfiguration);

            if (!IsAuthorizedToEditMirroringConfiguration(mirroringConfiguration)) return new HttpUnauthorizedResult();

            return View(_contentManager.BuildEditor(mirroringConfiguration));
        }

        [HttpPost, ActionName("Create")]
        public ActionResult CreatePost()
        {
            var mirroringConfiguration = _contentManager.New(ContentTypes.MirroringConfiguration);

            if (!IsAuthorizedToEditMirroringConfiguration(mirroringConfiguration)) return new HttpUnauthorizedResult();

            _contentManager.UpdateEditor(mirroringConfiguration, this);

            _contentManager.Create(mirroringConfiguration);

            if (!ModelState.IsValid)
            {
                _orchardServices.TransactionManager.Cancel();

                return Create();
            }

            _orchardServices.Notifier.Information(T("Mirroring Configuration successfully saved."));

            return RedirectToAction("Edit", new { id = mirroringConfiguration.Id });
        }

        public ActionResult Edit(int id)
        {
            var mirroringConfiguration = GetMirroringConfiguration(id);

            if (mirroringConfiguration == null) return HttpNotFound();

            if (!IsAuthorizedToEditMirroringConfiguration(mirroringConfiguration)) return new HttpUnauthorizedResult();

            var shape = _contentManager.BuildEditor(mirroringConfiguration);

            var mirroringConfigurationPart = mirroringConfiguration.As<MirroringConfigurationPart>();
            var mirroringStatus = string.IsNullOrEmpty(mirroringConfigurationPart.Status) ? MirroringStatus.New : (MirroringStatus)Enum.Parse(typeof(MirroringStatus), mirroringConfigurationPart.Status);
            var statusViewModel = new ConfigurationStatusViewModel
            {
                Status = mirroringStatus,
                StatusCode = mirroringConfigurationPart.StatusCode,
                StatusMessage = mirroringConfigurationPart.StatusMessage
            };
            _orchardServices.WorkContext.Layout.AfterContent.Add(_orchardServices.New.GitHgMirror_Common_ConfigurationStatus(StatusViewModel: statusViewModel));

            return View(shape);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int id)
        {
            var mirroringConfiguration = GetMirroringConfiguration(id);

            if (mirroringConfiguration == null) return HttpNotFound();

            if (!IsAuthorizedToEditMirroringConfiguration(mirroringConfiguration)) return new HttpUnauthorizedResult();

            _contentManager.UpdateEditor(mirroringConfiguration, this);

            if (!ModelState.IsValid)
            {
                _orchardServices.TransactionManager.Cancel();

                _orchardServices.Notifier.Warning(T("Couldn't save the Mirroring Configuration. Please check the data you entered."));

                return Edit(mirroringConfiguration.Id);
            }

            _contentManager.Publish(mirroringConfiguration);

            var mirroringConfigurationPart = mirroringConfiguration.As<MirroringConfigurationPart>();

            mirroringConfigurationPart.FailedSyncCounter = 0;
            mirroringConfigurationPart.Status = MirroringStatus.New.ToString();

            _orchardServices.Notifier.Information(T("Mirroring Configuration successfully saved."));

            return RedirectToAction("Edit", new { Id = mirroringConfiguration.Id });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var mirroringConfiguration = GetMirroringConfiguration(id);

            if (mirroringConfiguration == null) return HttpNotFound();

            if (mirroringConfiguration.As<CommonPart>().Owner != _workContext.CurrentUser) return new HttpUnauthorizedResult(T("You are not allowed to delete other users' Mirroring Configuration.").Text);

            if (!_authorizer.Authorize(Orchard.Core.Contents.Permissions.DeleteContent, mirroringConfiguration, T("You are not allowed to delete this Mirroring Configuration."))) return new HttpUnauthorizedResult();

            _contentManager.Remove(mirroringConfiguration);

            _orchardServices.Notifier.Information(T("Mirroring Configuration was successfully deleted."));

            return RedirectToAction("Index");
        }


        private bool IsAuthorizedToEditMirroringConfiguration(IContent mirroringConfiguration)
        {
            return _authorizer.Authorize(Orchard.Core.Contents.Permissions.EditContent, mirroringConfiguration, T("You are not allowed to edit this Mirroring Configuration."));
        }

        private ContentItem GetMirroringConfiguration(int id)
        {
            var mirroringConfiguration = id == 0 ? _contentManager.New(ContentTypes.MirroringConfiguration) : _contentManager.Get(id);
            return mirroringConfiguration == null || mirroringConfiguration.ContentType != ContentTypes.MirroringConfiguration ? null : mirroringConfiguration;
        }


        #region IUpdateModel Members

        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }

        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }

        #endregion
    }
}