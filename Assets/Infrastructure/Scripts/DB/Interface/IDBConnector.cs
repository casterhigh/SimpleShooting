using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Infrastructure.DB.Interface
{
    public interface IDBConnector
    {
        bool DBExist();

        List<T> Find<T>(Expression<Func<T, bool>> predicate);

        List<T> FindAll<T>();

        T FindOne<T>(Expression<Func<T, bool>> predicate);

        T FindById<T>(long id);

        void Upsert<T>(T val);

        void Upsert<T>(List<T> list);
    }
}
