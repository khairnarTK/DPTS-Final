using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Core.Speciality
{
    public partial interface ISpecialityService
    {
        /// <summary>
        /// Inserts an Speciality
        /// </summary>
        void AddSpeciality(Entities.Speciality speciality);

        /// <summary>
        /// Get Speciality by Id
        /// </summary>
        Entities.Speciality GetSpecialitybyId(int Id);

        /// <summary>
        /// Delete Specialityr by Id
        /// </summary>
        void DeleteSpeciality(Entities.Speciality speciality);

        /// <summary>
        /// update Speciality
        /// </summary>
        void UpdateSpeciality(Entities.Speciality data);

        /// <summary>
        /// get list of Speciality
        /// </summary>
        IList<Entities.Speciality> GetAllSpeciality(bool showhidden, bool enableTracking = false);

        void AddSpecialityByDoctor(SpecialityMapping doctorSpecilities);

        bool IsDoctorSpecialityExists(SpecialityMapping doctorSpecilities);
        /// <summary>
        /// Get specilities by doctor
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        IList<Entities.Speciality> GetDoctorSpecilities(string doctorId);
    }
}
