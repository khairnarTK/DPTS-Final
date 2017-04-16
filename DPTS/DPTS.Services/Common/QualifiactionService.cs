using DPTS.Domain.Core.Common;
using System;
using System.Collections.Generic;
using DPTS.Domain.Entities;
using DPTS.Domain.Core;
using System.Linq;

namespace DPTS.Services.Common
{
    public partial class QualifiactionService : IQualifiactionService
    {

        private readonly IRepository<Qualifiaction> _qualifiactionRepository;

        public QualifiactionService(IRepository<Qualifiaction> qualifiactionRepository)
        {
            _qualifiactionRepository = qualifiactionRepository;
        }

        public void AddQualifiaction(Qualifiaction qualifiaction)
        {
            if (qualifiaction == null)
                throw new ArgumentNullException("qualifiaction");

            _qualifiactionRepository.Insert(qualifiaction);
        }

        public void DeleteQualifiaction(Qualifiaction qualifiaction)
        {
            if (qualifiaction == null)
                throw new ArgumentNullException("qualifiaction");

            _qualifiactionRepository.Delete(qualifiaction);
        }

        public IPagedList<Qualifiaction> GetAllQualifiaction(int pageIndex = 0,
            int pageSize = Int32.MaxValue, bool showHidden = false)
        {
            var query = _qualifiactionRepository.Table;
            //if (!showHidden)
            //    query = query.Where(m => m.IsActive);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

            query = query.OrderBy(c => c.DisplayOrder);
            return new PagedList<Qualifiaction>(query, pageIndex, pageSize);
        }

        public Qualifiaction GetQualifiactionbyId(int Id)
        {
            if (Id == 0)
                return null;

            return _qualifiactionRepository.GetById(Id);
        }

        public void UpdateQualifiaction(Qualifiaction qualifiaction)
        {
            if (qualifiaction == null)
                throw new ArgumentNullException("qualifiaction");


            _qualifiactionRepository.Update(qualifiaction);
        }
    }
}
