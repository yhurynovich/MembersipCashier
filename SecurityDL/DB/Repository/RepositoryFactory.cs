using SecurityDL.DB.Repository.CustomRepository;
using SecurityUnified.Interfaces;

namespace SecurityDL.DB.Repository
{
    public static class RepositoryFactory
    {
        public static UserProfileRepositoryEntitySet GetUserProfile()
        {
            return new UserProfileRepositoryEntitySet();
        }

        public static RepositoryEntitySet<webpages_Role, IRole> GetRole()
        {
            return new RepositoryEntitySet<webpages_Role, IRole>();
        }

        public static RepositoryEntitySet<webpages_Membership, IWebPagesMembership> GetWebPagesMembership()
        {
            return new RepositoryEntitySet<webpages_Membership, IWebPagesMembership>();
        }
    }
}
