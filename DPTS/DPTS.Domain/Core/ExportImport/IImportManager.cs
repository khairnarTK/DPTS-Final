using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPTS.Domain.Core.ExportImport
{
    /// <summary>
    /// Import manager interface
    /// </summary>
    public partial interface IImportManager
    {
        /// <summary>
        /// Import products from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="userId"></param>
        /// <param name="iRow"></param>
        void ImportDoctorsFromXlsx(Stream stream,string userId,int iRow);
    }
}
