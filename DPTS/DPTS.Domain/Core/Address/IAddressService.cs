using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Core.Address
{
    public interface IAddressService
    {
        /// <summary>
        /// Inserts an Address
        /// </summary>
        
        void AddAddress(Entities.Address address);

        /// <summary>
        /// Get Address by Id
        /// </summary>
        Entities.Address GetAddressbyId(int Id);

        /// <summary>
        /// Delete Addressr by Id
        /// </summary>
        void DeleteAddress(Entities.Address address);

        /// <summary>
        /// update Address
        /// </summary>
        void UpdateAddress(Entities.Address address);

        /// <summary>
        /// get list of Address
        /// </summary>
        IList<Entities.Address> GetAllAddress();

        /// <summary>
        /// Get all address by userId
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        IList<Entities.Address> GetAllAddressByUser(string UserId);

        /// <summary>
        /// Inserts an Address Mapping
        /// </summary>
        void AddAddressMapping(AddressMapping addressMapping);

        void DeleteAddressMapping(AddressMapping addressMapping);

        AddressMapping GetAddressMappingbuUserIdAddrId(string UserId, int AddressId);

        IList<Entities.Address> GetAllAddressByZipcode(string zipcode);

        IList<AddressMapping> GetAllAddressMapping();

        //zipcode

        IList<ZipCodes> GetAllZipCodes();

        void InsertZipCode(ZipCodes zipcode);

        ZipCodes GetZipCodeInfo(string zipCode);

    }
}
