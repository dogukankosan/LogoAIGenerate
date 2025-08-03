using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReplicateImageAI.Classes
{
    internal static class DataHelper
    {
        internal static bool IsDataExists(DataTable dt)
        {
            return dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0]?.ToString());
        }
    }
}