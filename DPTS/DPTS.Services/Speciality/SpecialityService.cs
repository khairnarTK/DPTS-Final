using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Data.Context;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Speciality;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Speciality
{
    public class SpecialityService : ISpecialityService
    {
        #region Fields
        private readonly IRepository<Domain.Entities.Speciality> _specialityRepository;
        private readonly IRepository<SpecialityMapping> _specalityMappingRepos;
        private readonly DPTSDbContext _context;
        #endregion

        #region Constructor
        public SpecialityService(IRepository<Domain.Entities.Speciality> specialityRepository, IRepository<SpecialityMapping> specalityMappingRepos)
        {
            _specialityRepository = specialityRepository;
            _specalityMappingRepos = specalityMappingRepos;
            _context = new DPTSDbContext();
        }
        #endregion

        public void AddSpeciality(Domain.Entities.Speciality speciality)
        {
            if (speciality == null)
                throw new ArgumentNullException("speciality");

            _specialityRepository.Insert(speciality);

        }

        public void DeleteSpeciality(Domain.Entities.Speciality speciality)
        {
            if (speciality == null)
                throw new ArgumentNullException("speciality");

            _specialityRepository.Delete(speciality);

        }

        public Domain.Entities.Speciality GetSpecialitybyId(int Id)
        {
            if (Id == 0)
                return null;

            return _specialityRepository.GetById(Id);
        }

        public IList<Domain.Entities.Speciality> GetAllSpeciality(bool showhidden, bool enableTracking = false)
        {
            try
            {
                var query = _specialityRepository.Table;
                if (!showhidden)
                    query = query.Where(c => c.IsActive);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Title);

                var Specilities = query.ToList();
                return Specilities;
            }
            catch(Exception ex) { return null; }
            
        }

        public void UpdateSpeciality(Domain.Entities.Speciality data)
        {
            if (data == null)
                throw new ArgumentNullException("data");


            _specialityRepository.Update(data);
        }

        public void AddSpecialityByDoctor(SpecialityMapping doctorSpeciality)
        {
            if (doctorSpeciality == null)
                throw new ArgumentNullException("doctorSpeciality");

            doctorSpeciality.DateCreated = DateTime.UtcNow;
            doctorSpeciality.DateUpdated = DateTime.UtcNow;
            _specalityMappingRepos.Insert(doctorSpeciality);

        }
        public bool IsDoctorSpecialityExists(SpecialityMapping doctorSpeciality)
        {
            if (doctorSpeciality == null)
                throw new ArgumentNullException("doctorSpeciality");
            try
            {

                var data = _specalityMappingRepos.Table.FirstOrDefault(c => c.Doctor_Id == doctorSpeciality.Doctor_Id && c.Speciality_Id == doctorSpeciality.Speciality_Id);
                if (data != null)
                    return true;
            }
            catch { return false; }
            return false;

        }
        public IList<Domain.Entities.Speciality> GetDoctorSpecilities(string doctorId)
        {
            if (!string.IsNullOrWhiteSpace(doctorId))
            {
                var query = from s in _context.Specialities
                            join m in _context.SpecialityMapping on s.Id equals m.Speciality_Id
                            where m.Doctor_Id == doctorId
                            select s;

                return query.ToList();
            }
            return null;
        }

    }
}

