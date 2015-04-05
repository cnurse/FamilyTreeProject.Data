#region Copyright

// Copyright 2011 - Charles Nurse

#endregion

using System;
using System.Data.Common;
using System.Data.Entity;

namespace FamilyTreeProject.Data.Entity
{
    internal class FamilyTreeContext : DbContext
    {
        internal FamilyTreeContext(string connectionString) : base(connectionString)
        {
        }
    }
}