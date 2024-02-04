using CsvHelper;
using FreelanceBotBase.Domain.Product;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace FreelanceBotBase.Infrastructure.Helpers
{
    /// <summary>
    /// Helper for parsing data from google sheets.
    /// </summary>
    public class GoogleSheetsHelper
    {
        private readonly HttpClient _client;
        private readonly string _spreadsheetId;

        public GoogleSheetsHelper(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _client = clientFactory.CreateClient("google_sheets_client");
            _spreadsheetId = configuration["SPREADSHEET_ID"]!;
        }

        /// <summary>
        /// Gets records from google sheets.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ProductRecord>> GetRecordsAsync()
        {
            string url = $"https://docs.google.com/spreadsheets/d/{_spreadsheetId}/gviz/tq?tqx=out:csv";
            string csvString = await _client.GetStringAsync(url);

            using (var reader = new StringReader(csvString))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csvReader.GetRecords<ProductRecord>().ToList();
            }
        }
    }
}
