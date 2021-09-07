using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogPost.Common.RepositoryBase
{
    public interface IRepository<T> where T : class
    {
        Task<T> Create(T obj);

        Task<T> Update(T obj);

        Task<bool> Delete(int id);

        Task<T> GetById(int id);

        Task<IEnumerable<T>> Get();

        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate);
    }
}
