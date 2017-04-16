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
        #endregion

        #region Ctor
        public ImportManager(IDoctorService doctorService)
        {
            this._doctorService = doctorService;
            _context =new DPTSDbContext();
        }
        #endregion

        #region Utilities
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
                        new PropertyByName<Doctor>("ShortProfile"),
                        new PropertyByName<Doctor>("RegistrationNumber"),
                        new PropertyByName<Doctor>("DateOfBirth"),

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

                    var doctor = new Doctor
                    {
                        DoctorId = userId,
                        Gender = manager.GetProperty("Gender").StringValue,
                        ShortProfile = manager.GetProperty("ShortProfile").StringValue,
                       // Language = manager.GetProperty("Qualifications").StringValue,
                        RegistrationNumber = manager.GetProperty("RegistrationNumber").StringValue,
                        DateOfBirth = manager.GetProperty("DateOfBirth").StringValue
                    };

                    _doctorService.AddDoctor(doctor);
                }
            }
        }
        
        #endregion
    }
}
