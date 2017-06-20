using MembershipCashierUnified.Interfaces;
using System;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;

namespace MembershipCashierW.Controllers.ControllerBase
{
    public abstract class BlobSearchUserProfileControllerBase : UserProfileControllerBase
    {
        protected Expression<Func<IUserProfile2, bool>> GetBlobFilter(string blob)
        {
            if (string.IsNullOrWhiteSpace(blob))
                throw new Exception("blob is not supplied");

            blob = blob.Trim();

            Expression<Func<IUserProfile2, bool>> filter = x =>
                   SqlMethods.Like(x.EmailId, "%" + blob + "%")
                   || SqlMethods.Like(blob, "%" + x.EmailId + "%")
                   || SqlMethods.Like(x.LastName + " " + x.FirstName + " " + x.LastName, "%" + blob + "%")
                   || SqlMethods.Like(x.Phone, "%" + blob);

            return filter;
        }
    }
}