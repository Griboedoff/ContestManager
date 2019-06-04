using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataBaseEntities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Core.SheetsApi
{
    public interface ISheetsApiClient
    {
        Task<string> CreateTable(string contestName);

        Task FillParticipantsData(
            string tableId,
            IReadOnlyList<Participant> participants,
            Dictionary<Class, List<string>> tasksDescription);
    }

    public class SheetsApiClient : ISheetsApiClient
    {
        private const string ApplicationName = "ContestManager";

        private readonly SheetsService sheetsService;
        private readonly DriveService driveService;

        public SheetsApiClient(GoogleCredential config)
        {
            sheetsService = new SheetsService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = config,
                    ApplicationName = ApplicationName,
                });
            driveService = new DriveService(
                new BaseClientService.Initializer
                {
                    HttpClientInitializer = config,
                    ApplicationName = ApplicationName,
                });
        }

        public async Task<string> CreateTable(string contestName)
        {
            var createSheet = sheetsService.Spreadsheets.Create(
                new Spreadsheet
                {
                    Properties = new SpreadsheetProperties { Title = contestName }
                });
            var sheet = await createSheet.ExecuteAsync();

            var addPermissions = driveService.Permissions.Create(
                new Permission
                {
                    Role = "writer",
                    Type = "anyone",
                },
                sheet.SpreadsheetId);
            await addPermissions.ExecuteAsync();

            return sheet.SpreadsheetId;
        }

        public async Task FillParticipantsData(
            string tableId,
            IReadOnlyList<Participant> participants,
            Dictionary<Class, List<string>> tasksDescription)
        {
            var res = new List<(string, IEnumerable<IEnumerable<string>>)>();
            foreach (var (@class, value) in tasksDescription)
            {
                var data = new List<List<string>> { new[] { "" }.Concat(value).ToList() };
                data.AddRange(
                    participants.Where(p => p.UserSnapshot.Class == @class)
                        .Select(p => p.Pass)
                        .OrderBy(x => x)
                        .Select(p => new List<string> { p }));
                res.Add(($"{@class:D} класс", data));
            }

            var batchUpdateRequest = sheetsService.Spreadsheets.BatchUpdate(
                new BatchUpdateSpreadsheetRequest
                {
                    Requests = CreateAddSheetsRequest(res)
                },
                tableId);

            await batchUpdateRequest.ExecuteAsync();
        }

        private static IList<Request> CreateAddSheetsRequest(
            IEnumerable<(string title, IEnumerable<IEnumerable<string>> data)> sheetsInfo)
            => sheetsInfo.SelectMany(
                    (t, i) => new[]
                    {
                        new Request
                        {
                            AddSheet = new AddSheetRequest
                            {
                                Properties = new SheetProperties
                                {
                                    SheetId = i + 1,
                                    Title = t.title
                                }
                            },
                            UpdateCells = new UpdateCellsRequest
                            {
                                Fields = "*",
                                Start = new GridCoordinate
                                {
                                    ColumnIndex = 0,
                                    RowIndex = 0,
                                    SheetId = i + 1
                                },
                                Rows = t.data.ToRowsData()
                            }
                        }
                    })
                .ToList();
    }
}