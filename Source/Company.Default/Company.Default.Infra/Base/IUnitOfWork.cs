using Company.Default.Infra.Repositories;

namespace Company.Default.Infra.Base
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Begin new transaction on Database
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit current Transaction
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Rollback current Transaction
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// Save Changes on Database
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Entity Person specialized repository
        /// </summary>
        PersonRepository Person { get; }
    }
}
