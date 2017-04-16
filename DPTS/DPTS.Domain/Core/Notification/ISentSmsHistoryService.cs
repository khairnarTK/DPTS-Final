using System.Collections.Generic;

namespace DPTS.Domain.Core.Notification
{
    /// <summary>
    /// SentEmailHistory service interface
    /// </summary>
    public   interface ISentEmailHistoryService
    {
        /// <summary>
        /// Deletes a SentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistory">SentEmailHistory</param>
        void DeleteSentEmailHistory(Entities.Notification.SentEmailHistory sentEmailHistory);

        /// <summary>
        /// Gets all GetAllSentEmailHistory
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>GetAllSentEmailHistory</returns>
        IList<Entities.Notification.SentEmailHistory> GetAllSentEmailHistory(bool showHidden = false);

        /// <summary>
        /// Gets a SentEmailHistory
        /// </summary>
        /// <param name="historyId">SentEmailHistory identifier</param>
        /// <returns>SentEmailHistory</returns>
        Entities.Notification.SentEmailHistory GetSentEmailHistoryById(int historyId);

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="historyIds">SentEmailHistory identifiers</param>
        /// <returns>Countries</returns>
        IList<Entities.Notification.SentEmailHistory> GetSentEmailHistoryByIds(int[] historyIds);
 

        /// <summary>
        /// Inserts a SentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistory">SentEmailHistory</param>
        void InsertSentEmailHistory(Entities.Notification.SentEmailHistory sentEmailHistory);

        /// <summary>
        /// Updates the SentEmailHistory
        /// </summary>
        /// <param name="sentEmailHistory">SentEmailHistory</param>
        void UpdateSentEmailHistory(Entities.Notification.SentEmailHistory sentEmailHistory);
    }
}