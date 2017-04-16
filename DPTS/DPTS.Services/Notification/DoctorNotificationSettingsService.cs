using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Notification;
using DPTS.Domain.Entities.Notification;

namespace DPTS.Domain.Notification
{
    /// <summary>
    /// DoctorNotificationSettings Service
    /// </summary>
    public class DoctorNotificationSettingsService : IDoctorNotificationSettingsService
    {
        #region Fields

        private readonly IRepository<DoctorNotificationSettings> _doctorNotificationSettingsRepository;

        #endregion

        #region Ctor
        public DoctorNotificationSettingsService(IRepository<DoctorNotificationSettings> doctorNotificationSettingsRepository)
        {
            _doctorNotificationSettingsRepository = doctorNotificationSettingsRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// delete doctorNotificationSettings
        /// </summary>
        /// <param name="doctorNotificationSettings"></param>
        public void DeleteDoctorNotificationSettings(DoctorNotificationSettings doctorNotificationSettings)
        {
            if (doctorNotificationSettings == null)
                throw new ArgumentNullException("doctorNotificationSettings");

            _doctorNotificationSettingsRepository.Delete(doctorNotificationSettings);
        }
        /// <summary>
        /// Get all doctorNotificationSettings
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public IList<DoctorNotificationSettings> GetAllDoctorNotificationSettings(bool showHidden = false)
        {
            var query = _doctorNotificationSettingsRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

            var countries = query.ToList();
            return countries;
        }
        /// <summary>
        /// get doctorNotificationSettings by abbreviation
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <returns></returns>
        public DoctorNotificationSettings GetDoctorNotificationSettingsByAbbreviation(string abbreviation)
        {
            var query = from sp in _doctorNotificationSettingsRepository.Table
                        where sp.Message == abbreviation
                        select sp;
            var doctorNotificationSettings = query.FirstOrDefault();
            return doctorNotificationSettings;
        }

        /// <summary>
        /// get doctorNotificationSettings by id
        /// </summary>
        /// <param name="doctorNotificationSettingsId"></param>
        /// <returns></returns>
        public DoctorNotificationSettings GetDoctorNotificationSettingsById(int doctorNotificationSettingsId)
        {
            if (doctorNotificationSettingsId == 0)
                return null;

            return _doctorNotificationSettingsRepository.GetById(doctorNotificationSettingsId);
        }

        public IList<DoctorNotificationSettings> GetDoctorNotificationSettingsByIds(int[] doctorNotificationSettingsIds)
        {
            if (doctorNotificationSettingsIds == null || doctorNotificationSettingsIds.Length == 0)
                return new List<DoctorNotificationSettings>();

            var query = from c in _doctorNotificationSettingsRepository.Table
                        where doctorNotificationSettingsIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<DoctorNotificationSettings>();
            foreach (int id in doctorNotificationSettingsIds)
            {
                var country = countries.Find(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }
            return sortedCountries;
        }

        public IList<DoctorNotificationSettings> GetDoctorNotificationSettingssByCountryId(int countryId, bool showHidden = false)
        {
            var query = from sp in _doctorNotificationSettingsRepository.Table
                        orderby sp.DisplayOrder, sp.Name
                        where sp.CategoryId == countryId &&
                        (showHidden || sp.Published)
                        select sp;

            return query.ToList();
        }

        public void InsertDoctorNotificationSettings(DoctorNotificationSettings doctorNotificationSettings)
        {
            if (doctorNotificationSettings == null)
                throw new ArgumentNullException("doctorNotificationSettings");

            _doctorNotificationSettingsRepository.Insert(doctorNotificationSettings);
        }

        public void UpdateDoctorNotificationSettings(DoctorNotificationSettings doctorNotificationSettings)
        {
            if (doctorNotificationSettings == null)
                throw new ArgumentNullException("doctorNotificationSettings");

            _doctorNotificationSettingsRepository.Update(doctorNotificationSettings);
        }
         
        #endregion

    }
}
