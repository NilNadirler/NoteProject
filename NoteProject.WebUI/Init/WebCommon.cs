using NoteProject.Common;
using NoteProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteProject.WebUI.Init
{
    public class WebCommon : ICommon
    {
        public string GetUsername()
        {
            if(HttpContext.Current.Session["login"] != null)
            {
                EvernoteUser user = HttpContext.Current.Session["login"] as EvernoteUser;

                return user.Username;
            }

            return "system";
        }
    }
}