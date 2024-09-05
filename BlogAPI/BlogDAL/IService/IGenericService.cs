using System.Linq.Expressions;

namespace BlogDAL.IService
{
    public interface IGenericService<T> where T : class
    {
        Task<ICollection<T>> GetAll();
        Task<ICollection<T>> GetAll(params Expression<Func<T, object>>[] includes);
        Task<T> GetById(Expression<Func<T, bool>> predicate);
        Task<TResult> GetById<TResult>(Expression<Func<T, bool>> predicate,
                                       Expression<Func<T, TResult>> selector);
        Task<T> Add(T entity);
        Task Delete(Expression<Func<T, bool>> predicate);
        void Update(T entity);
        Task<ICollection<T>> GetByPredicate(Expression<Func<T, bool>> predicate);
        Task<ICollection<TResult>> GetAllWithSelect<TResult>(Expression<Func<T, TResult>> selector,
                                                             params Expression<Func<T, object>>[] includes);


    }
}
