namespace $safeprojectname$.Contracts.Repositories
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
        IPersonRepository Person { get; }
    }
}
