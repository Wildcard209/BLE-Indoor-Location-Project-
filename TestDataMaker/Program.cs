using static TestDataMaker.Program;
using System.Reflection.PortableExecutable;
using System.Formats.Asn1;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;

namespace TestDataMaker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var records = new List<CsvRecord>();
            using (var reader = new StreamReader("x_values_1.csv"))
                      using (var csv = new CsvReader(reader, new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture) { HasHeaderRecord = true }))
            {
                records = csv.GetRecords<CsvRecord>().ToList();
            }

            var resultTable = new List<(float LocationX, float PredictedX)>();
            foreach (var record in records)
            {
                var data = new X_Value.ModelInput
                {
                    B1 = record.B1 ?? 0,
                    B2 = record.B2 ?? 0,
                    B3 = record.B3 ?? 0,
                    B4 = record.B4 ?? 0,
                    B5 = record.B5 ?? 0,
                    B6 = record.B6 ?? 0,
                    B7 = record.B7 ?? 0,
                    B8 = record.B8 ?? 0,
                    B9 = record.B9 ?? 0,
                    B10 = record.B10 ?? 0,
                    B11 = record.B11 ?? 0,
                    B12 = record.B12 ?? 0,
                    B13 = record.B13 ?? 0,
                    B14 = record.B14 ?? 0,
                    B15 = record.B15 ?? 0,
                    B16 = record.B16 ?? 0,
                    B17 = record.B17 ?? 0,
                    B18 = record.B18 ?? 0,
                    B19 = record.B19 ?? 0
                };

                var result = X_Value.Predict(data);

                resultTable.Add((record.LocationX, result.Score));
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PredictedDataX");

                worksheet.Cells[1, 1].Value = "LocationX";
                worksheet.Cells[1, 2].Value = "PredictedX";

                int row = 2;
                foreach (var (locationX, predictedX) in resultTable)
                {
                    worksheet.Cells[row, 1].Value = locationX;
                    worksheet.Cells[row, 2].Value = predictedX;
                    row++;
                }

                var fileInfo = new FileInfo("predicted_data_x.xlsx");
                package.SaveAs(fileInfo);
            }

            Console.WriteLine("Excel file saved as predicted_data_x.xlsx");
        }
    }
    public class CsvRecord
    {
        public float? B1 { get; set; }
        public float? B2 { get; set; }
        public float? B3 { get; set; }
        public float? B4 { get; set; }
        public float? B5 { get; set; }
        public float? B6 { get; set; }
        public float? B7 { get; set; }
        public float? B8 { get; set; }
        public float? B9 { get; set; }
        public float? B10 { get; set; }
        public float? B11 { get; set; }
        public float? B12 { get; set; }
        public float? B13 { get; set; }
        public float? B14 { get; set; }
        public float? B15 { get; set; }
        public float? B16 { get; set; }
        public float? B17 { get; set; }
        public float? B18 { get; set; }
        public float? B19 { get; set; }
        public float LocationX { get; set; }
        //public float LocationY { get; set; }
    }

}