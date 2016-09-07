using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.DisplayManagement.Implementation;
using Orchard.Localization;

namespace GitHgMirror.Common.Services
{
    public class LogOnTitleSetter : IShapeDisplayEvents
    {
        public Localizer T { get; set; }


        public LogOnTitleSetter()
        {
            T = NullLocalizer.Instance;
        }


        public void Displaying(ShapeDisplayingContext context)
        {
            if (context.ShapeMetadata.Type != "LogOn") return;

            context.Shape.Title = T("Log in here!").Text;
        }

        public void Displayed(ShapeDisplayedContext context)
        {
        }
    }
}