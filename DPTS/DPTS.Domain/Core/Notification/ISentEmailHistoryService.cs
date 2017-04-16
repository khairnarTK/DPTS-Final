using System.Collections.Generic;

namespace DPTS.Domain.Core.Notification
{
    /// <summary>
    /// SentSmsHistory service interface
    /// </summary>
    public partial interface ISentSmsHistoryService
    {
        /// <summary>
        /// Deletes a SentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistory">SentSmsHistory</param>
        void DeleteSentSmsHistory(Entities.Notification.SentSmsHistory sentSmsHistory);

        /// <summary>
        /// Gets all GetAllSentSmsHistory
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>GetAllSentSmsHistory</returns>
        IList<Entities.Notification.SentSmsHistory> GetAllSentSmsHistory(bool showHidden = false);

        /// <summary>
        /// Gets a SentSmsHistory
        /// </summary>
        /// <param name="historyId">SentSmsHistory identifier</param>
        /// <returns>SentSmsHistory</returns>
        Entities.Notification.SentSmsHistory GetSentSmsHistoryById(int historyId);

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="historyIds">SentSmsHistory identifiers</param>
        /// <returns>Countries</returns>
        IList<Entities.Notification.SentSmsHistory> GetSentSmsHistoryByIds(int[] historyIds);
 

        /// <summary>
        /// Inserts a SentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistory">SentSmsHistory</param>
        void InsertSentSmsHistory(Entities.Notification.SentSmsHistory sentSmsHistory);

        /// <summary>
        /// Updates the SentSmsHistory
        /// </summary>
        /// <param name="sentSmsHistory">SentSmsHistory</param>
        void UpdateSentSmsHistory(Entities.Notification.SentSmsHistory sentSmsHistory);
    }
}