using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Notification;

namespace DPTS.Domain.Notification
{
    /// <summary>
    /// DefaultNotificationSettings Service
    /// </summary>
    public class DefaultNotificationSettingsService : IDefaultNotificationSettingsService
    {
        #region Fields

        private readonly IRepository<Domain.Entities.Notification.DefaultNotificationSettings> _defaultNotificationSettingsRepository;

        #endregion

        #region Ctor
        public DefaultNotificationSettingsService(IRepository<Domain.Entities.Notification.DefaultNotificationSettings> defaultNotificationSettingsRepository)
        {
            _defaultNotificationSettingsRepository = defaultNotificationSettingsRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// delete defaultNotificationSettings
        /// </summary>
        /// <param name="defaultNotificationSettings"></param>
        public void DeleteDefaultNotificationSettings(Domain.Entities.Notification.DefaultNotificationSettings defaultNotificationSettings)
        {
            if (defaultNotificationSettings == null)
                throw new ArgumentNullException("defaultNotificationSettings");

            _defaultNotificationSettingsRepository.Delete(defaultNotificationSettings);
        }
        /// <summary>
        /// Get all defaultNotificationSettings
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public IList<Domain.Entities.Notification.DefaultNotificationSettings> GetAllDefaultNotificationSettings(bool showHidden = false)
        {
            var query = _defaultNotificationSettingsRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

            var countries = query.ToList();
            return countries;
        }
        /// <summary>
        /// get defaultNotificationSettings by abbreviation
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public Domain.Entities.Notification.DefaultNotificationSettings GetDefaultNotificationSettingsByAbbreviation(string abbreviation)
        {
            var query = from sp in _defaultNotificationSettingsRepository.Table
                        where sp.Message == abbreviation
                        select sp;
            var defaultNotificationSettings = query.FirstOrDefault();
            return defaultNotificationSettings;
        }

        /// <summary>
        /// get defaultNotificationSettings by id
        /// </summary>
        /// <param name="defaultNotificationSettingsId"></param>
        /// <returns></returns>
        public Domain.Entities.Notification.DefaultNotificationSettings GetDefaultNotificationSettingsById(int defaultNotificationSettingsId)
        {
            if (defaultNotificationSettingsId == 0)
                return null;

            return _defaultNotificationSettingsRepository.GetById(defaultNotificationSettingsId);
        }

        public IList<Domain.Entities.Notification.DefaultNotificationSettings> GetDefaultNotificationSettingsByIds(int[] defaultNotificationSettingsIds)
        {
            if (defaultNotificationSettingsIds == null || defaultNotificationSettingsIds.Length == 0)
                return new List<Domain.Entities.Notification.DefaultNotificationSettings>();

            var query = from c in _defaultNotificationSettingsRepository.Table
                        where defaultNotificationSettingsIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Domain.Entities.Notification.DefaultNotificationSettings>();
            foreach (int id in defaultNotificationSettingsIds)
            {
                var country = countries.Find(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }
            return sortedCountries;
        }

        public IList<Domain.Entities.Notification.DefaultNotificationSettings> GetDefaultNotificationSettingssByCountryId(int countryId, bool showHidden = false)
        {
            var query = from sp in _defaultNotificationSettingsRepository.Table
                        orderby sp.DisplayOrder, sp.Name
                        where sp.CategoryId == countryId &&
                        (showHidden || sp.Published)
                        select sp;

            return query.ToList();
        }

        public void InsertDefaultNotificationSettings(Domain.Entities.Notification.DefaultNotificationSettings defaultNotificationSettings)
        {
            if (defaultNotificationSettings == null)
                throw new ArgumentNullException("defaultNotificationSettings");

            _defaultNotificationSettingsRepository.Insert(defaultNotificationSettings);
        }

        public void UpdateDefaultNotificationSettings(Domain.Entities.Notification.DefaultNotificationSettings defaultNotificationSettings)
        {
            if (defaultNotificationSettings == null)
                throw new ArgumentNullException("defaultNotificationSettings");

            _defaultNotificationSettingsRepository.Update(defaultNotificationSettings);
        }
         
        #endregion

    }
}
