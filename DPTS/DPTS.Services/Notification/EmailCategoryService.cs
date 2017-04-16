using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Notification;

namespace DPTS.Domain.Notification
{
    /// <summary>
    /// emailCategory service
    /// </summary>
    public partial class EmailCategoryService : IEmailCategoryService
    {
        #region Fields

        private readonly IRepository<Domain.Entities.Notification.EmailCategory> _emailCategoryRepository;

        #endregion

        #region Ctor

        public EmailCategoryService(IRepository<Domain.Entities.Notification.EmailCategory> emailCategoryRepository)
        {
            _emailCategoryRepository = emailCategoryRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a emailCategory
        /// </summary>
        /// <param name="emailCategory">emailCategory</param>
        public virtual void DeleteEmailCategory(Domain.Entities.Notification.EmailCategory emailCategory)
        {
            if (emailCategory == null)
                throw new ArgumentNullException("emailCategory");

            _emailCategoryRepository.Delete(emailCategory);

        }
        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Domain.Entities.Notification.EmailCategory> GetAllEmailCategories(bool showHidden = false)
        {
                var query = _emailCategoryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

                var countries = query.ToList();
                return countries;
        }

        /// <summary>
        /// Gets a emailCategory
        /// </summary>
        /// <param name="emailCategoryId">emailCategory identifier</param>
        /// <returns>emailCategory</returns>
        public virtual Domain.Entities.Notification.EmailCategory GetEmailCategoryById(int emailCategoryId)
        {
            if (emailCategoryId == 0)
                return null;

            return _emailCategoryRepository.GetById(emailCategoryId);
        }

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="emailCategoryIds">emailCategory identifiers</param>
        /// <returns>Countries</returns>
        public virtual IList<Domain.Entities.Notification.EmailCategory> GetEmailCategoriesByIds(int[] emailCategoryIds)
        {
            if (emailCategoryIds == null || emailCategoryIds.Length == 0)
                return new List<Domain.Entities.Notification.EmailCategory>();

            var query = from c in _emailCategoryRepository.Table
                        where emailCategoryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Domain.Entities.Notification.EmailCategory>();
            foreach (int id in emailCategoryIds)
            {
                var emailCategory = countries.Find(x => x.Id == id);
                if (emailCategory != null)
                    sortedCountries.Add(emailCategory);
            }
            return sortedCountries;
        }

      
       

        /// <summary>
        /// Inserts a emailCategory
        /// </summary>
        /// <param name="emailCategory">emailCategory</param>
        public virtual void InsertEmailCategory(Domain.Entities.Notification.EmailCategory emailCategory)
        {
            if (emailCategory == null)
                throw new ArgumentNullException("emailCategory");

            _emailCategoryRepository.Insert(emailCategory);
        }

        /// <summary>
        /// Updates the emailCategory
        /// </summary>
        /// <param name="emailCategory">emailCategory</param>
        public virtual void UpdateEmailCategory(Domain.Entities.Notification.EmailCategory emailCategory)
        {
            if (emailCategory == null)
                throw new ArgumentNullException("emailCategory");

            _emailCategoryRepository.Update(emailCategory);
        } 
        #endregion
    }
}
