﻿namespace Apit.Service
{
    public class ProjectConfig
    {
        public FeedbackConfig Feedback { get; set; }
        public MailboxConfig Mailbox { get; set; }
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
        }


        public class ContentConfig
        {
            public ContentDataConfig Article { get; set; }
            public ContentDataConfig Conference { get; set; }
        }


        public class ContentDataConfig
        {
            public int TitleMaxLength { get; set; }
            public int TopicMaxLength { get; set; }
            public int ShortDescriptionMaxLength { get; set; }
        }
    }
}