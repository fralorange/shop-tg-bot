using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Google.Apis.Sheets.v4.Data;

namespace FeatureBeingTested
{
    public class ParseGoogleSheet
    {
        private IConfigurationRoot _config;

        public ParseGoogleSheet()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<ParseGoogleSheet>();

            _config = builder.Build();
        }

        [Fact]
        public async void IsWorkingProperlyWithGoodPerformanceWithoutUsingApi()
        {
            // Arrange
            HttpClient client = new();
            string url = $"https://docs.google.com/spreadsheets/d/{_config["SPREADSHEET_ID"]}/gviz/tq?tqx=out:csv";

            // Act
            string csvString  = await client.GetStringAsync(url);
            using (var reader = new StringReader(csvString))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<dynamic>();
                int cost = 0;
                int i = 0;
                foreach (var record in records)
                {
                    cost += 100;
                    i++;

                    // Assert
                    Assert.Equal("Продукт" + i, record.Product);
                    Assert.Equal(cost, Convert.ToInt32(record.Cost));
                }
            }
        }

        [Fact]
        public async void IsWorkingProperlyWithGoodPerformanceWithUsingApi()
        {
            // Arrange
            string[] scopes = { SheetsService.Scope.SpreadsheetsReadonly };
            string AppName = "Google Sheets API Performance Test";

            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = _config["CLIENT_ID"],
                    ClientSecret = _config["CLIENT_SECRET"]
                },
                scopes,
                "user",
                CancellationToken.None);

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = AppName
            });

            SpreadsheetsResource.GetRequest getSpreadsheetRequest = service.Spreadsheets.Get(_config["SPREADSHEET_ID"]);
            getSpreadsheetRequest.IncludeGridData = false; 
            Spreadsheet spreadsheet = await getSpreadsheetRequest.ExecuteAsync();

            string firstSheetName = spreadsheet.Sheets[0].Properties.Title;

            SpreadsheetsResource.ValuesResource.GetRequest getRequest =
                service.Spreadsheets.Values.Get(_config["SPREADSHEET_ID"], firstSheetName);


            // Act
            ValueRange response = await getRequest.ExecuteAsync();
            IList<IList<object>> values = response.Values;

            int cost = 0;
            int i = 0;
            foreach (var row in values.Skip(1))
            {
                cost += 100;
                i++;

                // Assert
                Assert.Equal("Продукт" + i, row[0]);
                Assert.Equal(cost, Convert.ToInt32(row[1]));
            }
        }
    }
}