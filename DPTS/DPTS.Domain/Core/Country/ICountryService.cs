using System.Collections.Generic;

namespace DPTS.Domain.Core.Country
{
    /// <summary>
    /// Country service interface
    /// </summary>
    public partial interface ICountryService
    {
        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="country">Country</param>
        void DeleteCountry(Entities.Country country);

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Countries</returns>
        IList<Entities.Country> GetAllCountries(bool showHidden = false);

        /// <summary>
        /// Gets a country
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        Entities.Country GetCountryById(int countryId);

        /// <summary>
        /// Get countries by identifiers
        /// </summary>
        /// <param name="countryIds">Country identifiers</param>
        /// <returns>Countries</returns>
        IList<Entities.Country> GetCountriesByIds(int[] countryIds);

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        Entities.Country GetCountryByTwoLetterIsoCode(string twoLetterIsoCode);

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        Entities.Country GetCountryByThreeLetterIsoCode(string threeLetterIsoCode);

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="country">Country</param>
        void InsertCountry(Entities.Country country);

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="country">Country</param>
        void UpdateCountry(Entities.Country country);
    }
}