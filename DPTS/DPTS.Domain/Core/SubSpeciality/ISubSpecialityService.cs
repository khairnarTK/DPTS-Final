using System.Collections.Generic;

namespace DPTS.Domain.Core.SubSpeciality
{
    /// <summary>
    /// Sub Speciality Service
    /// </summary>
    public interface ISubSpecialityService
    {
        /// <summary>
        /// Inserts an sub Speciality
        /// </summary>
        void AddSubSpeciality(Entities.SubSpeciality subSpeciality);

        /// <summary>
        /// Get sub Speciality by Id
        /// </summary>
        Entities.SubSpeciality GetSubSpecialitybyId(int Id);

        /// <summary>
        /// Delete sub Speciality by Id
        /// </summary>
        void DeleteSubSpeciality(Entities.SubSpeciality subSpeciality);

        /// <summary>
        /// update sub Speciality
        /// </summary>
        void UpdateSubSpeciality(Entities.SubSpeciality subSpeciality);

        /// <summary>
        /// get list of sub Speciality
        /// </summary>
        IList<Entities.SubSpeciality> GetAllSubSpeciality(bool showhidden, bool enableTracking = false);

        IList<Domain.Entities.SubSpeciality> GetSubSpecBySpecId(int specId, bool showHidden = false);

    }
}
