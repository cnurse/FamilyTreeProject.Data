#region Copyright

// Copyright 2011 - Charles Nurse

#endregion

using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Keydance.Collections;
using Keydance.Contracts;
using Keydance.Data;

namespace FamilyTreeProject.Data.Entity
{
    public class EntityRepository<T> : IDisposable, IRepository<T> where T : class
    {
        private readonly FamilyTreeContext db;

        /// <summary>
        ///   Gets the entity set provided by the type T and returns for querying
        /// </summary>
        private DbSet<T> EntitySet
        {
            get { return db.Set<T>(); }
        }

        #region Constructors

        public EntityRepository(string connectionString)
        {
            db = new FamilyTreeContext(connectionString);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            db.Dispose();
        }

        #endregion

        #region IRepository<T> Members

        public void Add(T item)
        {
            Requires.NotNull("item", item);

            EntitySet.Add(item);
            db.SaveChanges();
        }

        public void Delete(T item)
        {
            Requires.NotNull("item", item);

            EntitySet.Remove(item);
            db.SaveChanges();
        }

        public void Delete(Expression<Func<T, bool>> expression)
        {
            foreach (T entity in Find(expression))
            {
                Delete(entity);
            }
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> expression)
        {
            return EntitySet.Where(expression);
        }

        public IQueryable<T> GetAll()
        {
            return EntitySet;
        }

        public PagedList<T> GetPaged(int pageIndex, int pageSize)
        {
            return new PagedList<T>(EntitySet, pageIndex, pageSize);
        }

        public void Update(T item)
        {
            Requires.NotNull("item", item);

            db.SaveChanges();
        }

        #endregion
    }
}