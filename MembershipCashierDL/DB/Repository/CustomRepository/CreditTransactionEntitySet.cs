using MembershipCashierDL.Properties;
using MembershipCashierUnified.Contracts;
using MembershipCashierUnified.Interfaces;
using SecurityDL.DB.Repository;
using SecurityUnified.Contracts;
using System.Linq;

namespace MembershipCashierDL.DB.Repository.CustomRepository
{
    internal class CreditTransactionEntitySet : RepositoryEntitySet<DB.CreditTransaction, ICreditTransaction>
    {
        public override IQueryable<ICreditTransaction> Result
        {
            get
            {
                if (result == null)
                {
                    IQueryable<DB.CreditTransaction> newResult = GetTable<DB.CreditTransaction>();

                    if (discriminator != null)
                    {
                        if (discriminator.Filter != null)
                            newResult = newResult.Where(discriminator.Filter).OfType<DB.CreditTransaction>();

                        if (discriminator.OrderBy != null)
                        {
                            foreach (var oby in discriminator.OrderBy)
                            {
                                newResult = OrderByHelper(newResult, oby.FieldName, oby.IsDescending);
                            }
                        }

                        newResult = newResult.Skip(discriminator.Skip).Take(discriminator.Take);
                    }
                    else
                    {
                        newResult = newResult.Take(DEFAULT_MAX_RECORDS);
                    }

                    result = newResult;
                }


                return result;
            }
        }
    }
}
