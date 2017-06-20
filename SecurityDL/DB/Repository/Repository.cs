using SecurityUnified.Interfaces;
using System;
using System.Threading;


namespace SecurityDL.DB.Repository
{
    //TODO: Dispose timer
    internal static class Repository
    {
        private const int DISPOSE_PERIOD = 36000;
        private static Timer timer = new Timer(DisposeUnusedRepositories, null, 3000, DISPOSE_PERIOD);

        public static RepositoryItem<UserProfile, IUserProfile> UserProfiles = new RepositoryItem<UserProfile, IUserProfile>();
        public static RepositoryItem<UserProfileAudit, IUserProfileAudit> UserProfileAudits = new RepositoryItem<UserProfileAudit, IUserProfileAudit>();

        private static void DisposeUnusedRepositories(object state)
        {
            if (DateTime.UtcNow.Subtract(UserProfiles.LastUsed).TotalMilliseconds >= DISPOSE_PERIOD)
                UserProfiles.Dispose();
        }
    }
}
