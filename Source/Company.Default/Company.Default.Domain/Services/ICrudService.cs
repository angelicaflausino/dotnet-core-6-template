namespace Company.Default.Domain.Services
{
    public interface ICrudService<T, TKey> where T : class
    {
        T Create(T entity);
        T Get(TKey id);
        void Update(T entity);
        bool Delete(TKey id);
    }
}
