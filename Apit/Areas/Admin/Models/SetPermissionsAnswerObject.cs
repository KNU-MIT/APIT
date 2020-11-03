using System;

namespace Apit.Areas.Admin.Models
{
    [Serializable]
    public class SetPermissionsAnswerObject
    {
        public string Status { get; set; }
        public UserData User { get; set; }


        [Serializable]
        public class UserData
        {
            public string FullName { get; set; }
            public string ProfileAddress { get; set; }
        }
    }
}