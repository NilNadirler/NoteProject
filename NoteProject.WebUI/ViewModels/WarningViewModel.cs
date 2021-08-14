using NoteProject.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteProject.WebUI.ViewModels
{
    public class WarningViewModel : NotifyViewModelBase<string>
    {
        public WarningViewModel()
        {
            Title = "Uyarı!";
        }
    }
}