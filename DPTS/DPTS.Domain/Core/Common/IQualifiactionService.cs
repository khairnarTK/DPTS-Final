using DPTS.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DPTS.Domain.Core.Common
{
    public partial interface IQualifiactionService
    {
        /// <summary>
        /// Inserts an Qualifiaction
        /// </summary>
        void AddQualifiaction(Qualifiaction Qualifiaction);

        /// <summary>
        /// Get Qualifiaction by Id
        /// </summary>
        Qualifiaction GetQualifiactionbyId(int Id);

        /// <summary>
        /// Delete Qualifiactionr by Id
        /// </summary>
        void DeleteQualifiaction(Qualifiaction qualifiaction);

        /// <summary>
        /// update Qualifiaction
        /// </summary>
        void UpdateQualifiaction(Qualifiaction qualifiaction);

        /// <summary>
        /// get list of Qualifiaction
        /// </summary>
        IPagedList<Qualifiaction> GetAllQualifiaction(int pageIndex = 0,
            int pageSize = Int32.MaxValue, bool showHidden = false);
    }
}
