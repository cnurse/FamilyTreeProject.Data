//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Naif.Core.Caching;
using Naif.Core.Collections;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.Data.EntityFramework
{
    public class EFRepository<T> : RepositoryBase<T> where T : class
    {
        private readonly FamilyTreeContext _db;

        /// <summary>
        ///   Gets the entity set provided by the type T and returns for querying
        /// </summary>
        private DbSet<T> EntitySet
        {
            // ReSharper disable once ConvertPropertyToExpressionBody
            get { return _db.Set<T>(); }
        }

        public EFRepository(FamilyTreeContext db, ICacheProvider cache) : base(cache)
        {
            Requires.NotNull(db);

            _db = db;
        }

        public override IEnumerable<T> Find(string sqlCondition, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override IPagedList<T> Find(int pageIndex, int pageSize, string sqlCondition, params object[] args)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public override IPagedList<T> Find(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        protected override void AddInternal(T item)
        {
            EntitySet.Add(item);
        }

        protected override void DeleteInternal(T item)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<T> GetAllInternal()
        {
            throw new NotImplementedException();
        }

        protected override T GetByIdInternal(object id)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<T> GetByScopeInternal(object scopeValue)
        {
            throw new NotImplementedException();
        }

        protected override IPagedList<T> GetPageByScopeInternal(object scopeValue, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        protected override IPagedList<T> GetPageInternal(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateInternal(T item)
        {
            if (_db.Entry(item).State == EntityState.Detached)
            {
                EntitySet.Attach(item);
            }
            _db.Entry(item).State = EntityState.Modified;
        }

        protected override IEnumerable<T> GetByPropertyInternal<TProperty>(string propertyName, TProperty propertyValue)
        {
            throw new NotImplementedException();
        }
    }
}