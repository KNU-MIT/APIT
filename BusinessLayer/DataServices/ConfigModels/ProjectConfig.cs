﻿namespace BusinessLayer.DataServices.ConfigModels
{
    public class ProjectConfig
    {
        public FeedbackConfig Feedback { get; set; }
        public MailboxConfig MailboxDefaults { get; set; }
        public ContentConfig Content { get; set; }


        public class FeedbackConfig
        {
            public string PhoneNumber { get; set; }
            public string ShortPhone { get; set; }
            public string Email { get; set; }
        }


        public class MailboxConfig
        {
            public string RealEmail { get; set; }
            public string RealEmailPassword { get; set; }
            public string AddressEmail { get; set; }
            public string AddressName { get; set; }

            public string ServiceHost { get; set; }
            public int ServicePort { get; set; }

            public MailSubjectsConfig MailSubjects { get; set; }


            public class MailSubjectsConfig
            {
                public string ConfirmEmailSubject { get; set; }
                public string ResetPasswordSubject { get; set; }
            }
        }


        public class ContentConfig
        {
            public ContentDataConfig Article { get; set; }
            public ContentDataConfig Conference { get; set; }

            public UniqueAddressConfig UniqueAddress { get; set; }
        }


        public class UniqueAddressConfig
        {
            public int MaxSize { get; set; }
            public int MinSize { get; set; }

            public int ArticleAddressSize { get; set; }
            public int UserAddressSize { get; set; }
        }

        public class ContentDataConfig
        {
            public int TitleMaxLength { get; set; }
            public int TopicMaxLength { get; set; }
            public int ShortDescriptionMaxLength { get; set; }
        }
    }
}