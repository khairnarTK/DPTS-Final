using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Address;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Address
{
    public class AddressService : IAddressService
    {
        #region Fields
        private readonly IRepository<Domain.Entities.Address> _addressRepository;
        private readonly IRepository<AddressMapping> _addressMappingRepository;
        private readonly IRepository<ZipCodes> _zipCodeRepository;
        #endregion

        #region Constructor
        public AddressService(IRepository<Domain.Entities.Address> addressRepository, 
            IRepository<AddressMapping> addressMappingRepository,
            IRepository<ZipCodes> zipCodeRepository)
        {
            _addressRepository = addressRepository;
            _addressMappingRepository = addressMappingRepository;
            _zipCodeRepository = zipCodeRepository;
        }
        #endregion

        #region Methods

        public void AddAddress(Domain.Entities.Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            _addressRepository.Insert(address);
        }

        public void DeleteAddress(Domain.Entities.Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            _addressRepository.Delete(address);
        }

        public Domain.Entities.Address GetAddressbyId(int Id)
        {
            if (Id == 0)
                return null;

            return _addressRepository.GetById(Id);
        }

        public IList<Domain.Entities.Address> GetAllAddress()
        {
            var query = _addressRepository.Table;

            return query.ToList(); ;
        }
        public IList<AddressMapping> GetAllAddressMapping()
        {
            var query = _addressMappingRepository.Table;

            return query.ToList(); ;
        }

        public IList<Domain.Entities.Address> GetAllAddressByUser(string UserId)
        {
            if (string.IsNullOrWhiteSpace(UserId))
                return null;

            var query = _addressMappingRepository.Table;
                query = query.Where(c => c.UserId == UserId);

            var lstAddr = new List<Domain.Entities.Address>();
            foreach (var addrMap in query.ToList())
            {
                var addr = _addressRepository.GetById(addrMap.AddressId);
                if (addr != null)
                    lstAddr.Add(addr);
            }
            return lstAddr;
        }
        public IList<Domain.Entities.Address> GetAllAddressByZipcode(string zipcode)
        {
            if (string.IsNullOrWhiteSpace(zipcode))
                return null;

            var addrs = _addressRepository.Table.Where(a => a.ZipPostalCode == zipcode).ToList();
            return addrs;
        }

        public void UpdateAddress(Domain.Entities.Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            _addressRepository.Update(address);
        }

        public void AddAddressMapping(AddressMapping addressMapping)
        {
            if (addressMapping == null)
                throw new ArgumentNullException("addressMapping");

            _addressMappingRepository.Insert(addressMapping);
        }

        public void DeleteAddressMapping(AddressMapping addressMapping)
        {
            if (addressMapping == null)
                throw new ArgumentNullException("addressMapping");

            _addressMappingRepository.Delete(addressMapping); //AddressMapping GetAddressMappingbId(int Id)
        }

        public AddressMapping GetAddressMappingbuUserIdAddrId(string UserId,int AddressId)
        {
            if (string.IsNullOrWhiteSpace(UserId) && AddressId == 0)
                return null;

            var query = _addressMappingRepository.Table;
                query = query.Where(c => c.UserId == UserId && c.AddressId == AddressId);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get All Zip Codes
        /// </summary>
        /// <returns></returns>
        public IList<ZipCodes> GetAllZipCodes()
        {
            return _zipCodeRepository.TableNoTracking.ToList();
        }

        public void InsertZipCode(ZipCodes zipcode)
        {
            _zipCodeRepository.Insert(zipcode);
        }

        public ZipCodes GetZipCodeInfo(string zipcode)
        {
            return _zipCodeRepository.TableNoTracking.FirstOrDefault(z => z.ZipCode == zipcode);
        }

        #endregion

    }
}
