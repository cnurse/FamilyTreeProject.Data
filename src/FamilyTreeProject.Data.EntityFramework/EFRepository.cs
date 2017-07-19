//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Collections.Generic;
using System.Linq;
using FamilyTreeProject.Collections;
using FamilyTreeProject.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace FamilyTreeProject.Data.EntityFramework
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly FamilyTreeContext _db;

        /// <summary>
        ///   Gets the entity set provided by the type T and returns for querying
        /// </summary>
        private DbSet<T> EntitySet
        {
            get { return _db.Set<T>(); }
        }

        public EFRepository(FamilyTreeContext db)
        {
            Requires.NotNull(db);

            _db = db;
        }

        public void Add(T item)
        {
            Requires.NotNull(item);

            EntitySet.Add(item);
        }

        public void Delete(T item)
        {
            EntitySet.Remove(item);
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return EntitySet.Where(predicate);
        }

        public IPagedList<T> Find(int pageIndex, int pageSize, Func<T, bool> predicate)
        {
            return GetAll().Where(predicate).InPagesOf(pageSize).GetPage(pageIndex);
        }

        public IEnumerable<T> GetAll()
        {
            return EntitySet;
        }

        public IPagedList<T> GetPage(int pageIndex, int pageSize)
        {
            return GetAll().InPagesOf(pageSize).GetPage(pageIndex);
        }

        public void Update(T item)
        {
            Requires.NotNull(item);

            if (_db.Entry(item).State == EntityState.Detached)
            {
                EntitySet.Attach(item);
            }
            _db.Entry(item).State = EntityState.Modified;
        }
    }
}