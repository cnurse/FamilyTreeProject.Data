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
using FamilyTreeProject.Collections;
using FamilyTreeProject.Contracts;
using FamilyTreeProject.Core;

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

        public IEnumerable<Individual> Find(Func<Individual, bool> predicate)
        {
            return GetAll().Where(predicate);
        }

        public IPagedList<Individual> Find(int pageIndex, int pageSize, Func<Individual, bool> predicate)
        {
            return GetAll().Where(predicate).InPagesOf(pageSize).GetPage(pageIndex);
        }

        IEnumerable<Individual> IRepository<Individual>.GetAll()
        {
            return GetAll();
        }

        public IEnumerable<Individual> GetAll()
        {
            return _database.Individuals;
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
