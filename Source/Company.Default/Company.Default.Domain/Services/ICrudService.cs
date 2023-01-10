namespace Company.Default.Domain.Services
{
    public interface ICrudService<T> where T : class
    {
        T Create(T entity);
        T Get(int id);
        void Update(T entity);
        bool Delete(int id);
    }
}
