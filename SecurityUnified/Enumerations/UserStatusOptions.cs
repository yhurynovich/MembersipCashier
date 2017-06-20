using System.ComponentModel;

namespace SecurityUnified.Enumerations
{
    public enum UserStatusOptions : int
    {
        [Description("Disabled")]
        //[Display(Description = "Disabled", Name = "Disabled", ResourceType = typeof(StringResource))]
        Disabled = 0,
        [Description("New")]
        //[Display(Description = "New", Name = "New", ResourceType = typeof(StringResource))]
        New = 1,
        [Description("Approved")]
        //[Display(Description = "Approved - Awaiting E-mail Notification", Name = "Approved", ResourceType = typeof(StringResource))]
        Approved = 2,
        [Description("Email Sent")]
        //[Display(Description = "Approved - Email Sent", Name = "EmailSent", ResourceType = typeof(StringResource))]
        EmailSent = 3,
        [Description("Active")]
        //[Display(Description = "Active", Name = "Active", ResourceType = typeof(StringResource))]
        Active = 4
    }
}
