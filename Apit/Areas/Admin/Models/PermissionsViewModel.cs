using System.Collections.Generic;
using DatabaseLayer.Entities;

namespace Apit.Areas.Admin.Models
{
    public class PermissionsViewModel
    {
        public IEnumerable<User> RootAdmins { get; set; }
        public IEnumerable<User> Managers { get; set; }
        
        
    }
}