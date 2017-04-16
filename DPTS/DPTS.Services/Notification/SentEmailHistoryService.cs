using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Notification;

namespace DPTS.Domain.Notification
{
    /// <summary>
    /// sentEmailHistory service
    /// </summary>
    public class SentEmailHistoryService : ISentEmailHistoryService
    {
        #region Fields

        private readonly IRepository<Domain.Entities.Notification.SentEmailHistory> _sentEmailHistoryRepository;

        #endregion

        #region Ctor

        public SentEmailHistoryService(IRepository<Domain.Entities.Notification.SentEmailHistory> sentEmailHistoryRepository)
        {
            _sentEmailHistoryRepository = sentEmailHistoryRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a sentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistory">sentEmailHistory</param>
        public virtual void DeleteSentEmailHistory(Domain.Entities.Notification.SentEmailHistory sentEmailHistory)
        {
            if (sentEmailHistory == null)
                throw new ArgumentNullException("sentEmailHistory");

            _sentEmailHistoryRepository.Delete(sentEmailHistory);

        }


        /// <summary>
        /// Gets all countries
        /// </summary> 
        public virtual IList<Domain.Entities.Notification.SentEmailHistory> GetAllSentEmailHistory(bool showHidden = false)
        {
            var query = _sentEmailHistoryRepository.Table;

            query = query.OrderBy(c => c.DateCreated).ThenBy(c => c.DateCreated);

            var countries = query.ToList();
            return countries;
        }
        /// <summary>
        /// Gets a sentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistoryId">sentEmailHistory identifier</param>
        /// <returns>sentEmailHistory</returns>
        public Domain.Entities.Notification.SentEmailHistory GetSentEmailHistoryById(int sentEmailHistoryId)
        {
            if (sentEmailHistoryId == 0)
                return null;

            return _sentEmailHistoryRepository.GetById(sentEmailHistoryId);
        }



        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="sentEmailHistoryIds">sentEmailHistory identifiers</param>
        /// <returns>Countries</returns>
        public IList<Domain.Entities.Notification.SentEmailHistory> GetSentEmailHistoryByIds(int[] sentEmailHistoryIds)
        {
            if (sentEmailHistoryIds == null || sentEmailHistoryIds.Length == 0)
                return new List<Domain.Entities.Notification.SentEmailHistory>();

            var query = from c in _sentEmailHistoryRepository.Table
                        where sentEmailHistoryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Domain.Entities.Notification.SentEmailHistory>();
            foreach (int id in sentEmailHistoryIds)
            {
                var sentEmailHistory = countries.Find(x => x.Id == id);
                if (sentEmailHistory != null)
                    sortedCountries.Add(sentEmailHistory);
            }
            return sortedCountries;
        }




        /// <summary>
        /// Inserts a sentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistory">sentEmailHistory</param>
        public virtual void InsertSentEmailHistory(Domain.Entities.Notification.SentEmailHistory sentEmailHistory)
        {
            if (sentEmailHistory == null)
                throw new ArgumentNullException("sentEmailHistory");

            _sentEmailHistoryRepository.Insert(sentEmailHistory);
        }

        /// <summary>
        /// Updates the sentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistory">sentEmailHistory</param>
        public virtual void UpdateSentEmailHistory(Domain.Entities.Notification.SentEmailHistory sentEmailHistory)
        {
            if (sentEmailHistory == null)
                throw new ArgumentNullException("sentEmailHistory");

            _sentEmailHistoryRepository.Update(sentEmailHistory);
        }
        #endregion
    }
}
