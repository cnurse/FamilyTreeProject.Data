//******************************************
//  Copyright (C) 2014-2015 Charles Nurse  *
//                                         *
//  Licensed under MIT License             *
//  (see included LICENSE)                 *
//                                         *
// *****************************************

using System;
using System.Linq;
using System.Linq.Expressions;
using Naif.Core.Collections;
using Naif.Core.Contracts;
using Naif.Data;

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMIndividualRepository : ILinqRepository<Individual>
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

        public IQueryable<Individual> Find(Expression<Func<Individual, bool>> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IPagedList<Individual> Find(int pageIndex, int pageSize, Expression<Func<Individual, bool>> predicate)
        {
            return GetAll().Where(predicate).InPagesOf(pageSize).GetPage(pageIndex);
        }

        public IQueryable<Individual> GetAll()
        {
            return _database.Individuals.AsQueryable();
        }

        public IPagedList<Individual> GetPage(int pageIndex, int pageSize)
        {
            return GetAll().InPagesOf(pageSize).GetPage(pageIndex);
        }

        public void Update(Individual item)
        {
            Requires.NotNull(item);

            _database.UpdateIndividual(item);
        }
    }
}
