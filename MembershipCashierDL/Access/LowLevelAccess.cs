using MembershipCashierDL.DB.Repository;
using MembershipCashierDL.MixedContracts;
using MembershipCashierDL.Properties;
using MembershipCashierUnified;
using MembershipCashierUnified.Contracts;
using SecurityUnified;
using SecurityUnified.Contracts;
using SecurityUnified.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;

namespace MembershipCashierDL.Access
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "LowLevelAccess" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)] 
    public class LowLevelAccess : ILowLevelAccess
    {
        private static DB.MembershipCashierEntitiesDataContext mdb;
        private object _sync_lock = new object();
        public int DEFAULT_MAX_RECORDS = 100;

        /// <summary>
        /// Please avoid using this property in favour of RepositoryEntitySet
        /// RepositoryEntitySet is lightweight ObjectRelationProvider for a single table
        /// </summary>
        DB.MembershipCashierEntitiesDataContext MDB
        {
            get
            {
                if (mdb == null)
                {
                    lock (_sync_lock)
                    {
                        if (mdb == null)
                        {
                            mdb = new DB.MembershipCashierEntitiesDataContext();
                            //mdb.ObjectTrackingEnabled
                        }
                    }
                }

                //switch (mdb.Database.Connection.State)
                //{ 
                //    //case System.Data.ConnectionState.Closed:
                //    //    mdb.Database.Connection.Open();
                //    //    break;
                //    case System.Data.ConnectionState.Broken:
                //        mdb.Database.Connection.Close();
                //        mdb.Database.Connection.Open();
                //        break;
                //    case System.Data.ConnectionState.Connecting:
                //        System.Threading.Thread.Sleep(10);
                //        break;
                //}

                return mdb;
            }
        }

        #region Find

        public AddressContract[] FindAddress(AddressDiscriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetAddress())
                {
                    records.Discriminator = d;
                    return records.OrderByDescending(x => x.ValidityLevel).Select(r => new AddressContract() { Address = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }
        public CreditTransactionContract[] FindCreditTransaction(CreditTransactionDiscriminator d)
        {
            try
            {
                var records = RepositoryFactory.GetCreditTransaction();
                if (d != null)
                {
                    records.Discriminator = d;
                    return records.Skip(d.Skip).Take(d.Take).Select(r => new CreditTransactionContract() { CreditTransaction = r }).ToArray();
                }
                else
                    return records.Take(DEFAULT_MAX_RECORDS).Select(r => new CreditTransactionContract() { CreditTransaction = r }).ToArray();

            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public LocationContract[] FindLocation(LocationDiscriminator d, OwnerVsLocationDiscriminator o, UserProfileDiscriminator u)
        {
            try
            {
                using (var records = RepositoryFactory.GetLocation())
                {
                    records.Discriminator = d;
                    records.OwnerVsLocationFilter = o;
                    records.UserProfileFilter = u;
                    return records.Select(r => new LocationContract() { Location = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public OwnerVsLocationContract[] FindOwnerVsLocation(OwnerVsLocationDiscriminator o, LocationDiscriminator l, UserProfileDiscriminator p)
        {
            try
            {
                using (var records = RepositoryFactory.GetOwnerVsLocation())
                {
                    records.Discriminator = o;
                    records.LocationFilter = l;
                    return records.Select(r => new OwnerVsLocationContract() { OwnerVsLocation = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public OwnerContract[] FindOwner(OwnerDiscriminator d, LocationDiscriminator l)
        {
            try
            {
                using (var records = RepositoryFactory.GetOwner())
                {
                    records.Discriminator = d;
                    records.LocationFilter = l;
                    return records.Select(r => new OwnerContract() {  Owner = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public ProductCreditContract[] FindProfileCredit(ProfileCreditDiscriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetProfileCredit())
                {
                    records.Discriminator = d;
                    return records.Select(r => new ProductCreditContract()
                    {
                        ProfileCredit = r,
                        Product = (r as DB.ProfileCredit).Product
                    }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public ProductCreditContract[] FindProfileCredit2(ProductDiscriminator p, ProfileCreditDiscriminator d, LocationDiscriminator l)
        {
            try
            {
                using (var records = RepositoryFactory.GetProfileCredit())
                {
                    records.Discriminator = d;
                    records.ProductFilter = p;
                    records.LocationFilter = l;
                    return records.Select(r => new ProductCreditContract()
                    {
                        ProfileCredit = r,
                        Product = (r as DB.ProfileCredit).Product
                    }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        /// <summary>
        /// Locates users by combining boths filters
        /// </summary>
        /// <param name="d"></param>
        /// <param name="d2"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public UserProfile2Contract[] FindUserProfile(UserProfile2Discriminator d, UserProfileVsLocationDiscriminator d2, RoleDiscriminator r)
        {
            try
            {
                using (var records = RepositoryFactory.GetUserProfile())
                {
                    records.Discriminator2 = d;
                    records.UserProfileVsLocationFilter = d2;
                    records.RoleFilter = r;
                    //Debug.WriteLine(records.Count());
                    return records.Select(x => new UserProfile2Contract() {
                         UserProfile2 = x as DB.UserProfile
                    }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public UserProfile2Contract[] FindUserProfile(UserProfile2Discriminator d)
        {
            try
            {
                using (var records = RepositoryFactory.GetUserProfile())
                {
                    records.Discriminator2 = d;

                    return records.OfType<DB.UserProfile>().Select(r => new UserProfile2Contract()
                    {
                        UserProfile2 = r,
                        UserRoles = (r as DB.UserProfile).webpages_UsersInRoles.Select(ur=>ur.webpages_Role)
                    }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="l"></param>
        /// <param name="c"></param>
        /// <param name="userId">User id that will be used to remove products known for the user</param>
        /// <returns></returns>
        public ProductContract[] FindProduct(ProductDiscriminator d, LocationDiscriminator l, ProfileCreditDiscriminator c, int userId)
        {
            try
            {
                using (var records = RepositoryFactory.GetProduct())
                {
                    records.Discriminator = d;
                    records.LocationFilter = l;
                    records.ProfileCreditFilter = c;
                    records.UserIdForExcludingAlreadyUsedProducts = userId;
                    return records.Select(r => new ProductContract() { Product = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public ProductPriceHistoryContract[] FindProductPriceHistory(ProductDiscriminator p, bool latestPriceOnly = true)
        {
            try
            {
                using (var records = RepositoryFactory.GetProductPriceHistory())
                {
                    //records.Discriminator = d;
                    records.ProductFilter = p;
                    records.LatestPriceOnly = latestPriceOnly;
                    return records.Select(r => new ProductPriceHistoryContract() { ProductPriceHistory = r }).ToArray();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        public UserAndProductsContarct[] FindLastUsedProducts(UserProfile2Discriminator u, UserProfileVsLocationDiscriminator pvl)
        {
            //TODO: it is not matching how it is implemented in the ProductRepository
            try
            {
                var userRecords = RepositoryFactory.GetUserProfile();
                userRecords.Discriminator2 = u;
                userRecords.UserProfileVsLocationFilter = pvl;

                return userRecords.OfType<DB.UserProfile>()
                    .Select(p=>new UserAndProductsContarct() {
                         UserProfile = p,
                         Products = p.ProfileCredits.OrderByDescending(pc=>pc.HasBallance).ThenByDescending(pc=>pc.CalculatedTime).ThenByDescending(pc=>pc.Product.CreditTransactions.Count).Take(Settings.Default.NumberOfLastProductsToShow)
                            .Select(pc=>new ProductCreditContract() {
                              Product = pc.Product,
                              ProfileCredit = pc
                         }).ToArray()
                    }).ToArray();
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
                return null;
            }
        }

        #endregion

        #region Insert
        public long[] InsertAddress(params AddressContract[] d)
        {
            lock (MDB)
            {
                try
                {
                    List<long> ret = new List<long>(d.Count());
                    foreach (var x in d)
                    {
                        x.Address.Uppercase();

                        var validation = x.Address.Validate();
                        if (validation.Any())
                            throw validation.ToException("Address validation failed");

                        var filter = MembershipCashierUnified.Extentions_ToLambda.ToLambda(x.Address);
                        var existingAddress = MDB.Addresses.Where(filter).OrderByDescending(a => a.ValidityLevel).FirstOrDefault();
                        if (existingAddress == null)
                        {
                            var xx = new DB.Address();
                            x.Address.CopyTo(xx);
                            MDB.Addresses.InsertOnSubmit(xx);
                            MDB.SubmitChanges();
                            ret.Add(xx.AddressId);
                        }
                        else
                        {
                            if (existingAddress.ValidityLevel < x.Address.ValidityLevel)
                            {
                                existingAddress.ValidityLevel = x.Address.ValidityLevel;
                            }
                            ret.Add(existingAddress.AddressId);
                        }
                    }

                    return ret.ToArray();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                    return new long[] { };
                }
            }
        }

        public void InsertProduct(Product_Location_Price_Contract[] d)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        var productId = x.Product.ProductId;
                        DB.Product product;
                        if(productId != default(int))
                            product = MDB.Products.FirstOrDefault(p=>p.ProductId == productId);
                        else
                        {
                            var locationId = x.Location.LocationId;
                            if (locationId == default(int))
                                throw new Xxception("LocationId is not specified");

                            //All owners of this location
                            var owners = MDB.OwnerVsLocations.Where(o=>o.LocationId == locationId).Select(o=>o.OwnerId);

                            //All locations that owners may own
                            var locationIds = MDB.OwnerVsLocations.Where(o=>owners.Contains(o.OwnerId)).Select(o=>o.LocationId).ToArray();

                            //Only reuse products from other locations of those owners
                            product = MDB.Products.FirstOrDefault(p => 
                                p.Description == x.Product.Description 
                                && (
                                      (!p.ProductVsLocations.Any() || p.ProductVsLocations.Any(l=> locationIds.Contains(l.LocationId)))
                                      &&  (!p.ProductPriceHistories.Any() || p.ProductPriceHistories.OrderByDescending(r=>r.ChangeDate).First().Price == x.Price)
                                   )
                            );

                            if (product == null)
                            {
                                product = new DB.Product();
                                x.Product.CopyTo(product, false);
                                MDB.Products.InsertOnSubmit(product);
                            }
                        }

                        var prodVsLoc = new DB.ProductVsLocation() { Product = product, LocationId = x.Location.LocationId };
                        MDB.ProductVsLocations.InsertOnSubmit(prodVsLoc);

                        var price = new DB.ProductPriceHistory() { Product = product, ChangeDate = DateTime.UtcNow, Price = x.Price };
                        MDB.ProductPriceHistories.InsertOnSubmit(price);
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public void InsertProductPriceHistory(ProductPriceHistoryContract[] d) {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        var productPrice = new DB.ProductPriceHistory();
                        x.ProductPriceHistory.CopyTo(productPrice, false);
                        MDB.ProductPriceHistories.InsertOnSubmit(productPrice);
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public ProfileCreditContract[] InsertCreditTransaction(CreditTransactionContract[] d)
        {
            lock (MDB)
            {
                try
                {
                    if (d != null && d.Any())
                    {
                        foreach (var x in d)
                        {
                            if (x.CreditTransaction.TransactionTime == default(DateTime))
                                x.CreditTransaction.TransactionTime = DateTime.UtcNow;

                            var ct = new DB.CreditTransaction();
                            x.CreditTransaction.CopyTo(ct, false);
                            MDB.CreditTransactions.InsertOnSubmit(ct);
                        }

                        //Recalculate ballance
                        var affectedBallances = new Code.BallanceManager().RecalcBallance(MDB, MDB.GetChangeSet().Inserts.OfType<DB.CreditTransaction>());

                        MDB.SubmitChanges();

                        return affectedBallances.Select(x => new ProfileCreditContract() { ProfileCredit = x }).ToArray();
                    }     
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }

                return new ProfileCreditContract[] { };
            }
        }

        public int[] InsertLocation(params LocationContract[] d)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        var xx = new DB.Location();
                        x.Location.CopyTo(xx);
                        MDB.Locations.InsertOnSubmit(xx);
                    }
                    var locations = MDB.GetChangeSet().Inserts.OfType<DB.Location>();
                    MDB.SubmitChanges();

                    var locationIds = locations.Select(l => l.LocationId).ToArray();
                    return locationIds;
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                    return new int[] { };
                }
            }
        }

        public void InsertOwner(params OwnerContract[] a)
        {
            lock (MDB)
            {
                try
                {
                    if (a != null)
                    {
                        foreach (var x in a)
                        {
                            var xx = new DB.Owner();
                            x.Owner.CopyTo(xx);
                            MDB.Owners.InsertOnSubmit(xx);
                        }
                    }

                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public void InsertOwnerVsLocation(params OwnerVsLocationContract[] a)
        {
            lock (MDB)
            {
                try
                {
                    if (a != null)
                    {
                        foreach (var x in a)
                        {
                            var xx = new DB.OwnerVsLocation();
                            x.OwnerVsLocation.CopyTo(xx);
                            MDB.OwnerVsLocations.InsertOnSubmit(xx);
                        }
                    }

                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public long[] InsertPayment(params PaymentContract[] d)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        var xx = new DB.Payment();
                        x.Payment.CopyTo(xx);
                        MDB.Payments.InsertOnSubmit(xx);
                    }
                    var payments = MDB.GetChangeSet().Inserts.OfType<DB.Payment>();
                    MDB.SubmitChanges();

                    var paymentsIds = payments.Select(l => l.PaymentId).ToArray();
                    return paymentsIds;
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                    return new long[] { };
                }
            }
        }
        #endregion

        #region Update

        public void UpdateAddress(AddressContract[] d, bool allowDefaultValues = true)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        x.Address.Uppercase();

                        var validation = x.Address.Validate();
                        if (validation.Any())
                            throw validation.ToException("Address validation failed");

                        var xx = MDB.Addresses.First(p => p.AddressId == x.Address.AddressId);
                        var originalValidity = xx.ValidityLevel;

                        if (originalValidity > x.Address.ValidityLevel) //3 is" complete validation" level
                            throw new Code.Exceptions.ExistingAddressException("Reducing address validity level is not allowed.");
                        if (xx.Country != x.Address.Country && !string.IsNullOrEmpty(xx.Country))
                            throw new Code.Exceptions.ExistingAddressException("Country code change is not allowed. Create new address instead.");
                        if (xx.Province != x.Address.Province && !string.IsNullOrEmpty(xx.Province))
                            throw new Code.Exceptions.ExistingAddressException("Province code change is not allowed. Create new address instead.");

                        x.Address.CopyTo(xx, allowDefaultValues);
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public void UpdateUserProfile(UserProfile2Contract[] d, int modifiedByUserId, bool allowDefaultValues = true)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var profile in d)
                    {
                        var dbProfile = MDB.UserProfiles.First(p => p.UserId == profile.UserProfile2.UserId);

                        //insert audit
                        var audit = new DB.UserProfileAudit()
                        {
                            Action = Convert.ToByte(SecurityUnified.Enumerations.AddRemoveOptions.PreviousVersion),
                            ModificationTime = DateTime.UtcNow,
                            ModifiedBy = modifiedByUserId
                        };
                        dbProfile.CopyTo(audit); //store previous profile data

                        MDB.UserProfileAudits.InsertOnSubmit(audit);

                        //update profile
                        profile.UserProfile2.CopyTo(dbProfile, allowDefaultValues);
                    }

                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public void UpdateProduct(ProductContract[] d, bool allowDefaultValues = true)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        var xx = MDB.Products.First(p => p.ProductId == x.Product.ProductId);
                        x.Product.CopyTo(xx, allowDefaultValues);
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        public void UpdateOwnerVsLocation(OwnerVsLocationContract[] d, bool allowDefaultValues = true)
        {
            lock (MDB)
            {
                try
                {
                    foreach (var x in d)
                    {
                        if (x.OwnerVsLocation.LocationId == default(int) && x.OwnerVsLocation.OwnerId == default(int))
                            throw new Xxception("Invalid Id");

                        var xx = MDB.OwnerVsLocations.First(p => p.LocationId == x.OwnerVsLocation.LocationId && p.OwnerId == x.OwnerVsLocation.OwnerId);
                        x.OwnerVsLocation.CopyTo(xx, allowDefaultValues);
                    }
                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }


        public void UpdateLocation(LocationContract[] d, bool allowDefaultValues = true)
        {
            lock (MDB)
            {
                try
                {
                    if (d.Any(l => l.Location.LocationId == default(int)))
                        throw new Xxception("LocationId expected");

                    foreach (var x in d)
                    {
                        var xx = MDB.Locations.First(p => p.LocationId == x.Location.LocationId);
                        x.Location.CopyTo(xx, allowDefaultValues);
                    }

                    MDB.SubmitChanges();
                }
                catch (Exception ex)
                {
                    HandleMyException(ex);
                }
            }
        }

        #endregion

        #region InsertOrUpdate

        public void InsertOrUpdateProfileCredit(ProfileCreditContract[] d, bool allowDefaultValues = true)
        {
            try
            {
                lock (MDB)
                {
                    foreach (var x in d)
                    {
                        var xx = MDB.ProfileCredits.FirstOrDefault(p => p.UserId == x.ProfileCredit.UserId && p.LocationId == x.ProfileCredit.LocationId && p.ProductId == x.ProfileCredit.ProductId);
                        if (xx == null)
                        {
                            xx = new DB.ProfileCredit();
                            MDB.ProfileCredits.InsertOnSubmit(xx);
                        }

                        x.ProfileCredit.CopyTo(xx, allowDefaultValues);
                    }
                    MDB.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                HandleMyException(ex);
            }
        }


        #endregion

        private void HandleMyException(Exception ex)
        {
            EventLog.WriteEntry("SecurityDL.LowLevelAccess"
                , ex.ConcatenateTrace()
                , EventLogEntryType.Error);

            Console.Error.WriteLine(ex);
            lock (_sync_lock)
            {
                mdb = null;
            }
#if !DEBUG
            throw ex;
#endif
        }
    }
}
