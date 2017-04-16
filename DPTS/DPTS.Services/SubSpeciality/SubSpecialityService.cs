using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.SubSpeciality;

namespace DPTS.Domain.SubSpeciality
{
    /// <summary>
    /// Sub Speci service
    /// </summary>
    public class SubSpecialityService : ISubSpecialityService
    {
        #region Fields
        private readonly IRepository<Domain.Entities.SubSpeciality> _subSpecialityRepository;
        #endregion

        #region Constructor
        public SubSpecialityService(IRepository<Domain.Entities.SubSpeciality> subSpecialityRepository)
        {
            _subSpecialityRepository = subSpecialityRepository;
        }
        #endregion
        public void AddSubSpeciality(Domain.Entities.SubSpeciality subSpeciality)
        {
            if (subSpeciality == null)
                throw new ArgumentNullException("subSpeciality");

            _subSpecialityRepository.Insert(subSpeciality);
        }

        public void DeleteSubSpeciality(Domain.Entities.SubSpeciality subSpeciality)
        {
            if (subSpeciality == null)
                throw new ArgumentNullException("subSpeciality");

            _subSpecialityRepository.Delete(subSpeciality);
        }

        public Domain.Entities.SubSpeciality GetSubSpecialitybyId(int Id)
        {
            if (Id == 0)
                return null;

            return _subSpecialityRepository.GetById(Id);
        }

        public void UpdateSubSpeciality(Domain.Entities.SubSpeciality subSpeciality)
        {
            if (subSpeciality == null)
                throw new ArgumentNullException("subSpeciality");


            _subSpecialityRepository.Update(subSpeciality);
        }

        public IList<Domain.Entities.SubSpeciality> GetAllSubSpeciality(bool showhidden, bool enableTracking = false)
        {
            var query = _subSpecialityRepository.Table;
            if (!showhidden)
                query = query.Where(c => c.IsActive);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

            var subSpecilities = query.ToList();
            return subSpecilities;
        }
        public IList<Domain.Entities.SubSpeciality> GetSubSpecBySpecId(int specId, bool showHidden = false)
        {
            var query = from sp in _subSpecialityRepository.Table
                        orderby sp.DisplayOrder, sp.Name
                        where sp.SpecialityId == specId &&
                        (showHidden || sp.IsActive)
                        select sp;

            return query.ToList();
        }
    }
}
