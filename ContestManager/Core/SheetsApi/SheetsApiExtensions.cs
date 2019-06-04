using System.Collections.Generic;
using System.Linq;
using Google.Apis.Sheets.v4.Data;

namespace Core.SheetsApi
{
    public static class SheetsApiExtensions
    {
        public static List<RowData> ToRowsData(this IEnumerable<IEnumerable<string>> source) => source
            .Select(
                r => new RowData
                {
                    Values = r.Select(c => new CellData
                        {
                            UserEnteredValue = new ExtendedValue { StringValue = c }
                        })
                        .ToList()
                })
            .ToList();
    }
}