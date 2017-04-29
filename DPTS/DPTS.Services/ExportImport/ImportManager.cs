using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DPTS.Domain.Core.ExportImport;
using DPTS.Domain.Entities;
using DPTS.Data.Context;
using DPTS.Domain.Core.Doctors;
using DPTS.Services.ExportImport.Help;
using OfficeOpenXml;
using DPTS.Domain.Core.Speciality;
using DPTS.Domain.Core.Country;
using DPTS.Domain.Core.StateProvince;
using System.Xml.Linq;
using DPTS.Domain.Core.Address;

namespace DPTS.Services.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager : IImportManager
    {
        #region Fields
        private readonly IDoctorService _doctorService;
        private readonly DPTSDbContext _context;
        private readonly ISpecialityService _specialityService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressService _addressService;
        #endregion

        #region Ctor
        public ImportManager(IDoctorService doctorService,
            ISpecialityService specialityService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IAddressService addressService)
        {
            this._doctorService = doctorService;
            _context =new DPTSDbContext();
            _specialityService = specialityService;
            _specialityService = specialityService;
            _stateProvinceService = stateProvinceService;
            _countryService = countryService;
            _addressService = addressService;
        }
        #endregion

        #region Utilities
        private static Dictionary<string, double> GetGeoCoordinate(string address)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            try
            {
                string requestUri = $"http://maps.google.com/maps/api/geocode/xml?address={address}&sensor=false";
                var request = System.Net.WebRequest.Create(requestUri);
                var response = request.GetResponse();
                var xdoc = XDocument.Load(response.GetResponseStream());
                var xElement = xdoc.Element(Constants.GeocodeResponse);
                var result = xElement?.Element(Constants.Result);
                var locationElement = result?.Element(Constants.Geometry)?.Element(Constants.Location);
                ParseLatLong(dictionary, locationElement);
            }
            catch (Exception ex)
            {
            }
            return dictionary;
        }
        public class Constants
        {
            public const string GeocodeResponse = "GeocodeResponse";
            public const string Result = "result";
            public const string Geometry = "geometry";
            public const string Location = "location";
            public const string Lat = "lat";
            public const string Lng = "lng";
        }
        private static void ParseLatLong(Dictionary<string, double> dictionary, XElement locationElement)
        {
            if (locationElement != null)
            {
                var lat = locationElement.Element(Constants.Lat);
                if (lat != null)
                    dictionary.Add(Constants.Lat, Double.Parse(lat.Value));
                var _long = locationElement.Element(Constants.Lng);
                if (_long != null)
                    dictionary.Add(Constants.Lng, Double.Parse(_long.Value));
            }
        }
        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }

        protected virtual string ConvertColumnToString(object columnValue)
        {
            if (columnValue == null)
                return null;

            return Convert.ToString(columnValue);
        }


        #endregion

        #region Methods

        /// <summary>
        /// Import doctors from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="userId"></param>
        /// <param name="iRow"></param>
        public virtual void ImportDoctorsFromXlsx(Stream stream,string userId,int iRow)
        {
            //property array
            var properties = new[]
            {
                       new PropertyByName<Doctor>("FirstName"),
                        new PropertyByName<Doctor>("LastName"),
                        new PropertyByName<Doctor>("Email"),
                        new PropertyByName<Doctor>("PhoneNumber"),

                        new PropertyByName<Doctor>("Gender"),
                        new PropertyByName<Doctor>("Speciality"),
                        new PropertyByName<Doctor>("ShortProfile"),
                        new PropertyByName<Doctor>("RegistrationNumber"),
                        new PropertyByName<Doctor>("DateOfBirth"),
                        new PropertyByName<Doctor>("Address1"),
                        new PropertyByName<Doctor>("Address2"),
                        new PropertyByName<Doctor>("Pincode"),
                        new PropertyByName<Doctor>("City"),
                        new PropertyByName<Doctor>("State"),
                        new PropertyByName<Doctor>("Country"),
                        new PropertyByName<Doctor>("Fax"),
                        new PropertyByName<Doctor>("Website")

            };

            var manager = new PropertyManager<Doctor>(properties);

            using (var xlPackage = new ExcelPackage(stream))
            {
                // get the first worksheet in the workbook
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new DptsException("No worksheet found");

                if (userId != null && iRow > 0)
                {
                    var allColumnsAreEmpty = manager.GetProperties
                        .Select(property => worksheet.Cells[iRow, property.PropertyOrderPosition])
                        .All(cell => cell == null || cell.Value == null || String.IsNullOrEmpty(cell.Value.ToString()));

                    if (allColumnsAreEmpty)
                        throw new DptsException("column are empty");

                    manager.ReadFromXlsx(worksheet, iRow);

                    #region DoctorInfo
                    var doctor = new Doctor
                    {
                        DoctorId = userId,
                        Gender = manager.GetProperty("Gender").StringValue,
                        ShortProfile = manager.GetProperty("ShortProfile").StringValue,
                        // Language = manager.GetProperty("Qualifications").StringValue,
                        RegistrationNumber = manager.GetProperty("RegistrationNumber").StringValue,
                        DateOfBirth = manager.GetProperty("DateOfBirth").StringValue,
                    };
                    _doctorService.AddDoctor(doctor);
                    #endregion

                    #region Specialities
                    if (!string.IsNullOrWhiteSpace(manager.GetProperty("Speciality").StringValue) &&
                        !string.IsNullOrWhiteSpace(doctor.DoctorId))
                    {
                        var specilities = manager.GetProperty("Speciality").StringValue.Trim();
                        foreach (var item in specilities.Split(',').ToList())
                        {
                            var spec = _specialityService.GetAllSpeciality(false).Where(s => s.Title.Contains(item)).FirstOrDefault();
                            if(spec != null)
                            {
                                var sp = new SpecialityMapping
                                {
                                    Speciality_Id = spec.Id,
                                    Doctor_Id = doctor.DoctorId,
                                    DateCreated = DateTime.UtcNow,
                                    DateUpdated = DateTime.UtcNow
                                };
                                if(!_specialityService.IsDoctorSpecialityExists(sp))
                                {
                                    _specialityService.AddSpecialityByDoctor(sp);
                                }
                            }
                        }

                        //foreach (var specilityMap in specilities.Split(',').ToList().Select(item => new SpecialityMapping
                        //{
                        //    Speciality_Id =
                        //    Doctor_Id = doctor.DoctorId,
                        //    DateCreated = DateTime.UtcNow,
                        //    DateUpdated = DateTime.UtcNow
                        //}).Where(specilityMap => !_specialityService.IsDoctorSpecialityExists(specilityMap)))
                        //{
                        //    _specialityService.AddSpecialityByDoctor(specilityMap);
                        //}
                    }
                    #endregion

                    #region Address
                    if (!string.IsNullOrWhiteSpace(doctor.DoctorId) &&
                        !string.IsNullOrWhiteSpace(manager.GetProperty("Pincode").StringValue) &&
                        !string.IsNullOrWhiteSpace(manager.GetProperty("Address1").StringValue) &&
                        !string.IsNullOrWhiteSpace(manager.GetProperty("City").StringValue) &&
                        !string.IsNullOrWhiteSpace(manager.GetProperty("State").StringValue) &&
                        !string.IsNullOrWhiteSpace(manager.GetProperty("Country").StringValue))
                    {
                        int countryId = 0;
                        int stateId = 0;
                        var country = _countryService.GetAllCountries().Where(c => c.Name.Contains(manager.GetProperty("Country").StringValue.Trim())).FirstOrDefault();
                        countryId = (country == null) ? 0 : country.Id;
                        string rr = manager.GetProperty("State").StringValue.Trim();

                        var states = _stateProvinceService.GetAllStateProvince().Where(c => c.Name.Contains(rr)).FirstOrDefault();
                        stateId = (states == null) ? 0 : states.Id;

                        var address = new Address
                        {
                            StateProvinceId = stateId,
                            //_,
                            CountryId = countryId,
                            Address1 = manager.GetProperty("Address1").StringValue.Trim(),
                            Address2 = manager.GetProperty("Address2").StringValue.Trim(),
                            Hospital = manager.GetProperty("Hospital").StringValue.Trim(),
                            FaxNumber = manager.GetProperty("FaxNumber").StringValue.Trim(),
                            PhoneNumber = manager.GetProperty("PhoneNumber").StringValue.Trim(),
                            Website = manager.GetProperty("Website").StringValue.Trim(),
                            ZipPostalCode = manager.GetProperty("Pincode").StringValue.Trim(),
                            City = manager.GetProperty("City").StringValue.Trim()
                        };

                        string docAddress = address.Address1 + ", " + address.City + ", " + states.Name + ", " + address.ZipPostalCode;
                        var geoCoodrinate = GetGeoCoordinate(docAddress);
                        if (geoCoodrinate.Count == 2)
                        {
                            address.Latitude = geoCoodrinate[Constants.Lat];
                            address.Longitude = geoCoodrinate[Constants.Lng];
                        }
                        else
                        {
                            var geoCoodrinates = GetGeoCoordinate(address.ZipPostalCode);
                            if (geoCoodrinates.Count == 2)
                            {
                                address.Latitude = geoCoodrinates[Constants.Lat];
                                address.Longitude = geoCoodrinates[Constants.Lng];
                            }
                        }
                        _addressService.AddAddress(address);
                        if (doctor != null)
                        {
                            var addrMap = new AddressMapping
                            {
                                AddressId = address.Id,
                                UserId = doctor.DoctorId
                            };
                            _addressService.AddAddressMapping(addrMap);
                        }

                    }
                    #endregion

                }
            }
        }
        
        #endregion
    }
}
