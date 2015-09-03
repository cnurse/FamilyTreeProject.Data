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
using System.Linq.Expressions;
using Naif.Core.Collections;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMIndividualRepository : IRepository<Individual>
    {
        private readonly IGEDCOMStore _database;

        public GEDCOMIndividualRepository(IGEDCOMStore database)
        {
            Requires.NotNull(database);

            _database = database;
        }

        public void Add(Individual item)
        {
            Requires.NotNull(item);

            _database.AddIndividual(item);
        }

        public void Delete(Individual item)
        {
            Requires.NotNull(item);

            _database.DeleteIndividual(item);
        }

        public IEnumerable<Individual> Find(string sqlCondition, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Individual> Find(int pageIndex, int pageSize, string sqlCondition, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Individual> Find(Expression<Func<Individual, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IPagedList<Individual> Find(int pageIndex, int pageSize, Expression<Func<Individual, bool>> predicate)
        {
            return GetAll().Where(predicate).InPagesOf(pageSize).GetPage(pageIndex);
        }

        public IEnumerable<Individual> Get<TScopeType>(TScopeType scopeValue)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Individual> IRepository<Individual>.GetAll()
        {
            return GetAll();
        }

        public Individual GetById<TProperty>(TProperty id)
        {
            throw new NotImplementedException();
        }

        public Individual GetById<TProperty, TScopeType>(TProperty id, TScopeType scopeValue)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Individual> GetAll()
        {
            return _database.Individuals.AsQueryable();
        }

        public IPagedList<Individual> GetPage(int pageIndex, int pageSize)
        {
            return GetAll().InPagesOf(pageSize).GetPage(pageIndex);
        }

        public IPagedList<Individual> GetPage<TScopeType>(TScopeType scopeValue, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void Update(Individual item)
        {
            Requires.NotNull(item);

            _database.UpdateIndividual(item);
        }

        public IEnumerable<Individual> GetByProperty<TProperty>(string propertyName, TProperty propertyValue)
        {
            throw new NotImplementedException();
        }
    }
}
