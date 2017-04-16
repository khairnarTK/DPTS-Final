using System.Collections.Generic;

namespace DPTS.Domain.Core.Notification
{
    /// <summary>
    /// EmailCategory service interface
    /// </summary>
    public partial interface IEmailCategoryService
    {
        /// <summary>
        /// Deletes a EmailCategory
        /// </summary>
        /// <param name="emailCategory">EmailCategory</param>
        void DeleteEmailCategory(Entities.Notification.EmailCategory emailCategory);

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        IList<Entities.Notification.EmailCategory> GetAllEmailCategories(bool showHidden = false);

        /// <summary>
        /// Gets a EmailCategory
        /// </summary>
        /// <param name="emailCategoryId">EmailCategory identifier</param>
        /// <returns>EmailCategory</returns>
        Entities.Notification.EmailCategory GetEmailCategoryById(int emailCategoryId);

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="emailCategoryIds">EmailCategory identifiers</param>
        /// <returns>Countries</returns>
        IList<Entities.Notification.EmailCategory> GetEmailCategoriesByIds(int[] emailCategoryIds);
 

        /// <summary>
        /// Inserts a EmailCategory
        /// </summary>
        /// <param name="emailCategory">EmailCategory</param>
        void InsertEmailCategory(Entities.Notification.EmailCategory emailCategory);

        /// <summary>
        /// Updates the EmailCategory
        /// </summary>
        /// <param name="emailCategory">EmailCategory</param>
        void UpdateEmailCategory(Entities.Notification.EmailCategory emailCategory);
    }
}