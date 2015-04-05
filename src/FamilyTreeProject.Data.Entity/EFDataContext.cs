using System;
using System.Linq;

namespace FamilyTreeProject.Data.Entity
{
    public class EFDataContext : IDataContext, IDisposable
    {
        #region IDataContext Implementation

        public IQueryable<Family> Families
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Individual> Individuals
        {
            get { throw new NotImplementedException(); }
        }

        public IQueryable<Note> Notes
        {
            get { throw new NotImplementedException(); }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void AddIndividual(Individual individual)
        {
            throw new NotImplementedException();
        }

        public void DeleteIndividual(Individual individual)
        {
            throw new NotImplementedException();
        }

        public void UpdateIndividual(Individual individual)
        {
            throw new NotImplementedException();
        }

        public void AddFamily(Family family)
        {
            throw new NotImplementedException();
        }

        public void DeleteFamily(Family family)
        {
            throw new NotImplementedException();
        }

        public void UpdateFamily(Family family)
        {
            throw new NotImplementedException();
        }

        public void AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public void DeleteNote(Note note)
        {
            throw new NotImplementedException();
        }

        public void UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///   Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
