using MazraeatiBackOffice.Core;
using System.Collections.Generic;

namespace MazraeatiBackOffice.Models
{
    public class ScreenManagementViewModel
    {
        //public List<ScreenPermissionViewModel> Screens { get; set; }
        //public Screen NewScreen { get; set; }
        public List<ScreenViewModel> Screens { get; set; } = new List<ScreenViewModel>();
        public Screen NewScreen { get; set; } = new Screen();
    }
}
