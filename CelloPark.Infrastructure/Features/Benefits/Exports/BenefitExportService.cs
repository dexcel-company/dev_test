using CelloPark.Application.Features.Benefits.Exports.Abstractions;
using CelloPark.Domain.Features.Benefits;
using CelloPark.Infrastructure.Common.Providers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CelloPark.Infrastructure.Features.Benefits.Exports;

internal sealed class BenefitExportService :
    IBenefitExportService
{
    public FileStream Export(List<Benefit> benefits)
    {
        Directory.CreateDirectory(FileProvider.ExportFilesDirectory);

        string filePath = Path.Combine(FileProvider.ExportFilesDirectory, $"Benefit-{Guid.NewGuid()}");

        using FileStream writeStream = new(filePath, FileMode.Create, FileAccess.Write);
        XSSFWorkbook workbook = new();
        ISheet sheet = workbook.CreateSheet("Benefits");

        IRow headers = sheet.CreateRow(0);

        headers.CreateCell(0).SetCellValue("Benefit");
        headers.CreateCell(1).SetCellValue("Description");
        headers.CreateCell(2).SetCellValue("Coupon");
        headers.CreateCell(3).SetCellValue("Coupon duration");
        headers.CreateCell(4).SetCellValue("Activation duration");
        headers.CreateCell(5).SetCellValue("Amount");
        headers.CreateCell(6).SetCellValue("Amount type");
        headers.CreateCell(7).SetCellValue("Frequency");
        headers.CreateCell(8).SetCellValue("Frequency type");
        headers.CreateCell(9).SetCellValue("Limit");
        headers.CreateCell(10).SetCellValue("Applies to");
        headers.CreateCell(11).SetCellValue("Status");
        headers.CreateCell(12).SetCellValue("Start active date (UTC)");
        headers.CreateCell(13).SetCellValue("End active date (UTC)");
        headers.CreateCell(14).SetCellValue("Start promotion date (UTC)");
        headers.CreateCell(15).SetCellValue("End promotion date (UTC)");

        foreach (Benefit benefit in benefits)
        {
            IRow row = sheet.CreateRow(sheet.LastRowNum + 1);

            string applience = benefit.PaymentCategories.All(x => x.ItemId != null) ? "Items" :
                benefit.PaymentCategories.All(x => x.PackageId != null) ? "Package" :
                benefit.PaymentCategories.All(x => x.PlanId != null) ? "Plan" : "Unknown";

            row.CreateCell(0).SetCellValue(benefit.Name);
            row.CreateCell(1).SetCellValue(benefit.Description);
            row.CreateCell(2).SetCellValue(string.Join('\n', benefit.Coupons.Select(coupon => coupon.Coupon)));
            row.CreateCell(3).SetCellValue(benefit.CouponsDuration.ToString());
            row.CreateCell(4).SetCellValue(benefit.Duration.ToString());
            row.CreateCell(5).SetCellValue(benefit.PaymentCategories.Sum(paymentCategory => paymentCategory.Amount).ToString());
            row.CreateCell(6).SetCellValue(benefit.PaymentCategories.FirstOrDefault()?.AmountType?.ToString());
            row.CreateCell(7).SetCellValue(benefit.PaymentCategories.FirstOrDefault()?.Frequency?.ToString());
            row.CreateCell(8).SetCellValue(benefit.PaymentCategories.FirstOrDefault()?.FrequencyType?.ToString());
            row.CreateCell(9).SetCellValue(benefit.PaymentCategories.FirstOrDefault()?.AmountLimit?.ToString());
            row.CreateCell(10).SetCellValue(applience);
            row.CreateCell(11).SetCellValue(benefit.Status.ToString());
            row.CreateCell(12).SetCellValue(benefit.StartActiveDate.ToString());
            row.CreateCell(13).SetCellValue(benefit.EndActiveDate.ToString());
            row.CreateCell(14).SetCellValue(benefit.StartPromotionDate.ToString());
            row.CreateCell(15).SetCellValue(benefit.EndPromotionDate.ToString());
        }

        for (int i = 0; i < 16; i++)
        {
            if (i == 1)
            {
                continue;
            }

            sheet.AutoSizeColumn(i);
        }

        workbook.Write(writeStream);

        FileStream readStream = new(filePath, FileMode.Open, FileAccess.Read);

        return readStream;
    }
}
