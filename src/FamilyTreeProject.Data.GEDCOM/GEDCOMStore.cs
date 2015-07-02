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
using FamilyTreeProject.Common;
using FamilyTreeProject.GEDCOM;
using FamilyTreeProject.GEDCOM.Common;
using FamilyTreeProject.GEDCOM.Records;
using FamilyTreeProject.GEDCOM.Structures;
using Naif.Core.Contracts;
using PCLStorage;

// ReSharper disable UseStringInterpolation

namespace FamilyTreeProject.Data.GEDCOM
{
    public class GEDCOMStore : IGEDCOMStore
    {
        private GEDCOMDocument _document;
        private readonly string _path;
// ReSharper disable InconsistentNaming
        private const int DEFAULT_TREE_ID = 1;
// ReSharper restore InconsistentNaming

        public GEDCOMStore(string path)
        {
            Requires.NotNullOrEmpty("path", path);

            _path = path;

            Initialize();
        }

        public List<Family> Families { get;private set; }

        public List<Individual> Individuals { get;private set; }

        private void CreateNewFamily(Individual individual)
        {
            var newFamily = new Family();

            if (individual.FatherId > 0)
            {
                //New father
                newFamily.HusbandId = individual.FatherId;
            }
            if (individual.MotherId > 0)
            {
                //New mother
                newFamily.WifeId = individual.MotherId;
            }

            newFamily.Children.Add(individual);

            //Save Family
            AddFamily(newFamily);
        }

        private GEDCOMFamilyRecord GetFamilyRecord(Individual individual)
        {
            int fatherId = individual.FatherId.GetValueOrDefault();
            int motherId = individual.MotherId.GetValueOrDefault();

            return _document.SelectFamilyRecord(GEDCOMUtil.CreateId("I", fatherId), GEDCOMUtil.CreateId("I", motherId));
        }

        private void Initialize()
        {
            LoadDocument();

            ProcessIndividuals();
            ProcessFamilies();
        }

        private void LoadDocument()
        {
            _document = new GEDCOMDocument();

            var fileStorage = FileSystem.Current;
            var file = fileStorage.GetFileFromPathAsync(_path).Result;

            using (var stream = file.OpenAsync(FileAccess.Read).Result)
            {
                _document.Load(stream);
            }
        }

        private void ProcessFamilies()
        {
            Families = new List<Family>();

            foreach (var gedcomRecord in _document.FamilyRecords)
            {
                var familyRecord = (GEDCOMFamilyRecord) gedcomRecord;
                var family = new Family
                                    {
                                        Id = familyRecord.GetId(),
                                        HusbandId = GEDCOMUtil.GetId(familyRecord.Husband),
                                        WifeId = GEDCOMUtil.GetId(familyRecord.Wife),
                                        TreeId = DEFAULT_TREE_ID
                                    };

                foreach (string child in familyRecord.Children)
                {
                    var childId = GEDCOMUtil.GetId(child);
                    if (childId > -1)
                    {
                        var individual = Individuals.SingleOrDefault(ind => ind.Id == childId);
                        if (individual != null)
                        {
                            individual.MotherId = family.WifeId;
                            individual.FatherId = family.HusbandId;
                        }
                    }
                }
                Families.Add(family);
            }
        }

        private void ProcessIndividuals()
        {
            Individuals = new List<Individual>();

            foreach (var gedcomRecord in _document.IndividualRecords)
            {
                var individualRecord = (GEDCOMIndividualRecord) gedcomRecord;
                var individual = new Individual
                                        {
                                            Id = individualRecord.GetId(),
                                            FirstName = individualRecord.Name.GivenName,
                                            LastName = individualRecord.Name.LastName,
                                            Sex = individualRecord.Sex,
                                            TreeId = DEFAULT_TREE_ID
                                        };

                foreach (var eventStructure in individualRecord.Events)
                {
                    var newEvent = new IndividualEvent
                    {
                        Date = eventStructure.Date,
                        Place = (eventStructure.Place != null) ? eventStructure.Place.Data : string.Empty,
                        EventType = eventStructure.IndividualEventType
                    };
                    individual.Events.Add(newEvent);
                }

                foreach (var noteStructure in individualRecord.Notes)
                {
                    if (String.IsNullOrEmpty(noteStructure.XRefId))
                    {
                        individual.Notes.Add(new Note { Text = noteStructure.Text });
                    }
                    else
                    {
                        var noteRecord = _document.NoteRecords[noteStructure.XRefId] as GEDCOMNoteRecord;
                        if (noteRecord != null)
                        {
                            individual.Notes.Add(new Note { Text = noteRecord.Data });
                        }
                    }
                }

                Individuals.Add(individual);
            }
        }

        private static void RemoveIndividualFromFamilyRecord(Individual child, GEDCOMRecord familyRecord, GEDCOMTag tag)
        {
            var childRecord = (from GEDCOMRecord record in familyRecord.ChildRecords.GetLinesByTag(tag)
                               where record.XRefId == GEDCOMUtil.CreateId("I", child.Id)
                               select record).SingleOrDefault();

            if (childRecord != null)
            {
                familyRecord.ChildRecords.Remove(childRecord);
            }
        }

        private void UpdateFamilyDetails(Individual individual)
        {
            var familyRecord = _document.SelectChildsFamilyRecord(GEDCOMUtil.CreateId("I", individual.Id));

            if (familyRecord != null)
            {
                if (individual.FatherId != GEDCOMUtil.GetId(familyRecord.Husband) || individual.MotherId != GEDCOMUtil.GetId(familyRecord.Wife))
                {
                    //remove child from current family
                    RemoveIndividualFromFamilyRecord(individual, familyRecord, GEDCOMTag.CHIL);

                    familyRecord = GetFamilyRecord(individual);

                    if (familyRecord != null)
                    {
                        //Add Individual as Child
                        familyRecord.AddChild(GEDCOMUtil.CreateId("I", individual.Id));
                    }
                    else
                    {
                        //new Family
                        CreateNewFamily(individual);
                    }
                }
            }
            else
            {
                if (individual.FatherId > 0 || individual.MotherId > 0)
                {
                    familyRecord = GetFamilyRecord(individual);

                    if (familyRecord != null)
                    {
                        //Add Individual as Child
                        familyRecord.AddChild(GEDCOMUtil.CreateId("I", individual.Id));
                    }
                    else
                    {
                        //new Family
                        CreateNewFamily(individual);
                    }
                }
            }
        }

        public void AddFamily(Family family)
        {
            Requires.NotNull("family", family);

            family.Id = _document.Records.GetNextId(GEDCOMTag.FAM);

            var record = new GEDCOMFamilyRecord(family.Id);
            if (family.HusbandId.HasValue)
            {
                //Add HUSB
                record.AddHusband(GEDCOMUtil.CreateId("I", family.HusbandId.Value));
            }

            if (family.WifeId.HasValue)
            {
                //Add WIFE
                record.AddWife(GEDCOMUtil.CreateId("I", family.WifeId.Value));
            }

            foreach (Individual child in family.Children)
            {
                //Add CHIL
                record.AddChild(GEDCOMUtil.CreateId("I", child.Id));
            }

            _document.AddRecord(record);
        }

        public void AddIndividual(Individual individual)
        {
            Requires.NotNull("individual", individual);

            //Add to internal List
            Individuals.Add(individual);

            //Add underlying GEDCOM record
            individual.Id = _document.Records.GetNextId(GEDCOMTag.INDI);

            var record = new GEDCOMIndividualRecord(individual.Id);
            var name = new GEDCOMNameStructure(String.Format("{0} /{1}/", individual.FirstName, individual.LastName), record.Level + 1);

            record.Name = name;
            record.Sex = individual.Sex;
            _document.AddRecord(record);

            //Update Family Info
            UpdateFamilyDetails(individual);
        }

        public void DeleteFamily(Family family)
        {
            Requires.NotNull("family", family);

            GEDCOMFamilyRecord record = _document.SelectFamilyRecord(GEDCOMUtil.CreateId("F", family.Id));

            if (record == null)
            {
                //record not in repository
                throw new ArgumentOutOfRangeException();
            }

            _document.RemoveRecord(record);
        }

        public void DeleteIndividual(Individual individual)
        {
            Requires.NotNull("individual", individual);

            string individualId = GEDCOMUtil.CreateId("I", individual.Id);

            //Remove from internal List
            Individuals.Remove(individual);

            GEDCOMIndividualRecord record = _document.SelectIndividualRecord(individualId);

            if (record == null)
            {
                //record not in repository
                throw new ArgumentOutOfRangeException();
            }

            _document.RemoveRecord(record);

            //see if individual is a child in a family
            var familyRecord = _document.SelectChildsFamilyRecord(individualId);
            if (familyRecord != null)
            {
                //remove child from family
                RemoveIndividualFromFamilyRecord(individual, familyRecord, GEDCOMTag.CHIL);
            }

            if (individual.Sex == Sex.Male)
            {
                //see if individual is a husband in a family
                foreach (GEDCOMFamilyRecord fRecord in _document.SelectHusbandsFamilyRecords(individualId))
                {
                    //remove husband from family
                    RemoveIndividualFromFamilyRecord(individual, fRecord, GEDCOMTag.HUSB);
                }
            }
            else
            {
                //see if individual is a wife in a family
                foreach (GEDCOMFamilyRecord fRecord in _document.SelectWifesFamilyRecords(individualId))
                {
                    //remove wife from family
                    RemoveIndividualFromFamilyRecord(individual, fRecord, GEDCOMTag.WIFE);
                }
            }
        }

        public async void SaveChangesAsync()
        {
            var fileStorage = FileSystem.Current;
            var file = fileStorage.GetFileFromPathAsync(_path).Result;

            await file.WriteAllTextAsync(_document.SaveGEDCOM());
        }

        public void UpdateFamily(Family family)
        {
            Requires.NotNull("family", family);

            GEDCOMFamilyRecord record = _document.SelectFamilyRecord(GEDCOMUtil.CreateId("F", family.Id));
            if (record == null)
            {
                //record not in repository
                throw new ArgumentOutOfRangeException();
            }
        }

        public void UpdateIndividual(Individual individual)
        {
            Requires.NotNull("individual", individual);

            GEDCOMIndividualRecord record = _document.SelectIndividualRecord(GEDCOMUtil.CreateId("I", individual.Id));
            if (record == null)
            {
                //record not in repository
                throw new ArgumentOutOfRangeException();
            }

            record.Name = new GEDCOMNameStructure(String.Format("{0} /{1}/", individual.FirstName, individual.LastName), record.Level + 1);
            record.Sex = individual.Sex;

            //Update Family Info
            UpdateFamilyDetails(individual);
        }
    }
}
