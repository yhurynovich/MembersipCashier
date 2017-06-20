using MembershipCashierUnified.Interfaces;

namespace MembershipCashierDL.DB
{
    public partial class Owner : IOwner
    {

        //int IOwner.OwnerUserId
        //{
        //    get
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //    set
        //    {
        //        throw new System.NotImplementedException();
        //    }
        //}

        /// <summary>
        /// Mimics OwnerUserId
        /// </summary>
        public int UserId
        {
            get
            {
                return OwnerUserId ?? 0;
            }
            set
            {
                OwnerUserId = value;
            }
        }
    }
}
