using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Notification;

namespace DPTS.Domain.Notification
{
    /// <summary>
    /// sentSmsHistory service
    /// </summary>
    public class SentSmsHistoryService : ISentSmsHistoryService
    {
        #region Fields

        private readonly IRepository<Domain.Entities.Notification.SentSmsHistory> _sentSmsHistoryRepository;

        #endregion

        #region Ctor

        public SentSmsHistoryService(IRepository<Domain.Entities.Notification.SentSmsHistory> sentSmsHistoryRepository)
        {
            _sentSmsHistoryRepository = sentSmsHistoryRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a sentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistory">sentSmsHistory</param>
        public virtual void DeleteSentSmsHistory(Domain.Entities.Notification.SentSmsHistory sentSmsHistory)
        {
            if (sentSmsHistory == null)
                throw new ArgumentNullException("sentSmsHistory");

            _sentSmsHistoryRepository.Delete(sentSmsHistory);

        }


        /// <summary>
        /// Gets all countries
        /// </summary> 
        public virtual IList<Domain.Entities.Notification.SentSmsHistory> GetAllSentSmsHistory(bool showHidden = false)
        {
            var query = _sentSmsHistoryRepository.Table;

            query = query.OrderBy(c => c.DateCreated).ThenBy(c => c.DateCreated);

            var countries = query.ToList();
            return countries;
        }
        /// <summary>
        /// Gets a sentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistoryId">sentSmsHistory identifier</param>
        /// <returns>sentSmsHistory</returns>
        public Domain.Entities.Notification.SentSmsHistory GetSentSmsHistoryById(int sentSmsHistoryId)
        {
            if (sentSmsHistoryId == 0)
                return null;

            return _sentSmsHistoryRepository.GetById(sentSmsHistoryId);
        }



        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="sentSmsHistoryIds">sentSmsHistory identifiers</param>
        /// <returns>Countries</returns>
        public IList<Domain.Entities.Notification.SentSmsHistory> GetSentSmsHistoryByIds(int[] sentSmsHistoryIds)
        {
            if (sentSmsHistoryIds == null || sentSmsHistoryIds.Length == 0)
                return new List<Domain.Entities.Notification.SentSmsHistory>();

            var query = from c in _sentSmsHistoryRepository.Table
                        where sentSmsHistoryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Domain.Entities.Notification.SentSmsHistory>();
            foreach (int id in sentSmsHistoryIds)
            {
                var sentSmsHistory = countries.Find(x => x.Id == id);
                if (sentSmsHistory != null)
                    sortedCountries.Add(sentSmsHistory);
            }
            return sortedCountries;
        }




        /// <summary>
        /// Inserts a sentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistory">sentSmsHistory</param>
        public virtual void InsertSentSmsHistory(Domain.Entities.Notification.SentSmsHistory sentSmsHistory)
        {
            if (sentSmsHistory == null)
                throw new ArgumentNullException("sentSmsHistory");

            _sentSmsHistoryRepository.Insert(sentSmsHistory);
        }

        /// <summary>
        /// Updates the sentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistory">sentSmsHistory</param>
        public virtual void UpdateSentSmsHistory(Domain.Entities.Notification.SentSmsHistory sentSmsHistory)
        {
            if (sentSmsHistory == null)
                throw new ArgumentNullException("sentSmsHistory");

            _sentSmsHistoryRepository.Update(sentSmsHistory);
        }
        #endregion
    }
}
