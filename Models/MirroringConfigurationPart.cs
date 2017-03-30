using GitHgMirror.CommonTypes;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using System.ComponentModel.DataAnnotations;

namespace GitHgMirror.Common.Models
{
    public class MirroringConfigurationPart : ContentPart<MirroringConfigurationPartRecord>
    {
        [Required(ErrorMessage = "We'll need a git URL to get this done!")]
        public string GitCloneUrl
        {
            get { return this.Retrieve(x => x.GitCloneUrl); }
            set { this.Store(x => x.GitCloneUrl, value); }
        }

        [Required(ErrorMessage = "We'll need an hg URL to get this done!")]
        public string HgCloneUrl
        {
            get { return this.Retrieve(x => x.HgCloneUrl); }
            set { this.Store(x => x.HgCloneUrl, value); }
        }

        [Required(ErrorMessage = "We can't just guess the direction of syncing, right?")]
        public MirroringDirection Direction
        {
            get { return this.Retrieve(x => x.Direction); }
            set { this.Store(x => x.Direction, value); }
        }

        public string Status
        {
            get { return this.Retrieve(x => x.Status); }
            set { this.Store(x => x.Status, value); }
        }

        public int StatusCode
        {
            get { return this.Retrieve(x => x.StatusCode); }
            set { this.Store(x => x.StatusCode, value); }
        }

        public string StatusMessage
        {
            get { return this.Retrieve(x => x.StatusMessage); }
            set { this.Store(x => x.StatusMessage, value); }
        }

        public bool GitUrlIsHgUrl
        {
            get { return this.Retrieve(x => x.GitUrlIsHgUrl); }
            set { this.Store(x => x.GitUrlIsHgUrl, value); }
        }

        public int FailedSyncCounter
        {
            get { return this.Retrieve(x => x.FailedSyncCounter); }
            set { this.Store(x => x.FailedSyncCounter, value); }
        }
    }


    public class MirroringConfigurationPartRecord : ContentPartRecord
    {
        public virtual string Status { get; set; }
    }
}