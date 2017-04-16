using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Country;

namespace DPTS.Domain.Country
{
    /// <summary>
    /// Country service
    /// </summary>
    public partial class CountryService : ICountryService
    {
        #region Fields

        private readonly IRepository<Domain.Entities.Country> _countryRepository;

        #endregion

        #region Ctor

        public CountryService(IRepository<Domain.Entities.Country> countryRepository)
        {
            _countryRepository = countryRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void DeleteCountry(Domain.Entities.Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Delete(country);

        }
        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="languageId">Language identifier. It's used to sort countries by localized names (if specified); pass 0 to skip it</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        public virtual IList<Domain.Entities.Country> GetAllCountries(bool showHidden = false)
        {
                var query = _countryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Name);

                var countries = query.ToList();
                return countries;
        }

        /// <summary>
        /// Gets a country
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public virtual Domain.Entities.Country GetCountryById(int countryId)
        {
            if (countryId == 0)
                return null;

            return _countryRepository.GetById(countryId);
        }

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        public virtual IList<Domain.Entities.Country> GetCountriesByIds(int[] countryIds)
        {
            if (countryIds == null || countryIds.Length == 0)
                return new List<Domain.Entities.Country>();

            var query = from c in _countryRepository.Table
                        where countryIds.Contains(c.Id)
                        select c;
            var countries = query.ToList();
            //sort by passed identifiers
            var sortedCountries = new List<Domain.Entities.Country>();
            foreach (int id in countryIds)
            {
                var country = countries.Find(x => x.Id == id);
                if (country != null)
                    sortedCountries.Add(country);
            }
            return sortedCountries;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Domain.Entities.Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            if (String.IsNullOrEmpty(twoLetterIsoCode))
                return null;

            var query = from c in _countryRepository.Table
                        where c.TwoLetterIsoCode == twoLetterIsoCode
                        select c;
            var country = query.FirstOrDefault();
            return country;
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public virtual Domain.Entities.Country GetCountryByThreeLetterIsoCode(string threeLetterIsoCode)
        {
            if (String.IsNullOrEmpty(threeLetterIsoCode))
                return null;

            var query = from c in _countryRepository.Table
                        where c.ThreeLetterIsoCode == threeLetterIsoCode
                        select c;
            var country = query.FirstOrDefault();
            return country;
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void InsertCountry(Domain.Entities.Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Insert(country);
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        public virtual void UpdateCountry(Domain.Entities.Country country)
        {
            if (country == null)
                throw new ArgumentNullException("country");

            _countryRepository.Update(country);
        }
        #endregion
    }
}
