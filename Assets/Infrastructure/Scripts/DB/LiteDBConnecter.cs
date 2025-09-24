using LiteDB;
using System.Linq.Expressions;
using Infrastructure.DB.Interface;
using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Infrastructure.DB
{
    public class LiteDBConnecter : IDBConnector
    {
        string path;

        public LiteDBConnecter(string path)
        {
            this.path = path;
        }

        public bool DBExist()
        {
            return File.Exists(path);
        }

        public List<T> Find<T>(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<T>();
                return col.Find(predicate).ToList();
            }
        }

        public List<T> FindAll<T>()
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<T>();
                return col.FindAll().ToList();
            }
        }

        public T FindOne<T>(Expression<Func<T, bool>> predicate)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<T>();
                return col.FindOne(predicate);
            }
        }

        public T FindById<T>(long id)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<T>();
                return col.FindById(id);
            }
        }

        public void Upsert<T>(T val)
        {
            if (val == null)
            {
                throw new ArgumentNullException(nameof(val));
            }

            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<T>();
                col.Upsert(val);
            }
        }

        public void Upsert<T>(List<T> list)
        {
            using (var db = new LiteDatabase(path))
            {
                var col = db.GetCollection<T>();
                col.Upsert(list);
            }
        }
    }
}
