using CelloPark.Application.Features.DailyUsageSummaries.Dtos;
using CelloPark.Application.Features.DailyUsageSummaries.Services.Abstractions;
using CelloPark.Infrastructure.Common.Exports.Excel;
using CelloPark.Infrastructure.Common.Providers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CelloPark.Infrastructure.Features.DailyUsageSummaries.Services;

internal sealed class DailyUsageSummariesExportService :
    IDailyUsageSummariesExportService
{
    public DailyUsageSummariesExportService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    private readonly TimeProvider _timeProvider;

    public FileStream Export(
        List<ExportDailyUsageDto> currentItems,
        List<ExportDailyUsageDto> referenceItems,
        List<ExportDailyUsageDto> currentPlans,
        List<ExportDailyUsageDto> referencePlans,
        List<ExportDailyUsageDto> currentPackages,
        List<ExportDailyUsageDto> referencePackages,
        DateOnly? currentStartDate,
        DateOnly? currentEndDate,
        DateOnly? referenceStartDate,
        DateOnly? referenceEndDate)
    {
        XSSFWorkbook workbook = new();
        ISheet sheet = workbook.CreateSheet("Summary");
        DateOnly utcDate = DateOnly.FromDateTime(_timeProvider.GetUtcNow().UtcDateTime);
        ExcelCellStyleStorage styleStorage = ExcelCellStyleStorage.GetInstance(workbook);

        currentStartDate ??= utcDate;
        currentEndDate ??= utcDate;
        referenceStartDate ??= utcDate;
        referenceEndDate ??= utcDate;

        InsertData(
            sectionName: "Items",
            sheet: sheet,
            styleStorage: styleStorage,
            currentValues: currentItems,
            referenceValues: referenceItems,
            currentStartDate: currentStartDate.Value,
            currentEndDate: currentEndDate.Value,
            referenceStartDate: referenceStartDate.Value,
            referenceEndDate: referenceEndDate.Value);

        InsertData(
            sectionName: "Plans",
            sheet: sheet,
            styleStorage: styleStorage,
            currentValues: currentPlans,
            referenceValues: referencePlans,
            currentStartDate: currentStartDate.Value,
            currentEndDate: currentEndDate.Value,
            referenceStartDate: referenceStartDate.Value,
            referenceEndDate: referenceEndDate.Value);

        InsertData(
            sectionName: "Packages",
            sheet: sheet,
            styleStorage: styleStorage,
            currentValues: currentPackages,
            referenceValues: referencePackages,
            currentStartDate: currentStartDate.Value,
            currentEndDate: currentEndDate.Value,
            referenceStartDate: referenceStartDate.Value,
            referenceEndDate: referenceEndDate.Value);

        for (int i = 0; i < 15; i++)
        {
            sheet.AutoSizeColumn(i);
        }

        string filePath = WriteToFile(workbook);

        FileStream readStream = new(filePath, FileMode.Open, FileAccess.Read);

        return readStream;
    }

    private static void InsertData(
        string sectionName,
        ISheet sheet,
        ExcelCellStyleStorage styleStorage,
        List<ExportDailyUsageDto> currentValues,
        List<ExportDailyUsageDto> referenceValues,
        DateOnly currentStartDate,
        DateOnly currentEndDate,
        DateOnly referenceStartDate,
        DateOnly referenceEndDate)
    {
        IRow dates = sheet.CreateRow(0);

        CreateCell(dates, 0, currentStartDate.ToString(), styleStorage.StandardLeftBoldCell);
        CreateCell(dates, 1, currentEndDate.ToString(), styleStorage.StandardRightBoldCell);
        CreateCell(dates, 8, referenceStartDate.ToString(), styleStorage.StandardLeftBoldCell);
        CreateCell(dates, 9, referenceEndDate.ToString(), styleStorage.StandardRightBoldCell);

        int rowIndex = sheet.LastRowNum != 0 ? sheet.LastRowNum : 2;
        IRow headers = sheet.CreateRow(rowIndex);

        CreateCell(headers, 1, sectionName, styleStorage.StandardBoldCell);
        CreateCell(headers, 2, "Quantity used", styleStorage.StandardBoldCell);
        CreateCell(headers, 3, "Gross earnings [NIS]", styleStorage.StandardBoldCell);
        CreateCell(headers, 4, "Benefits quantity", styleStorage.StandardBoldCell);
        CreateCell(headers, 5, "Benefits cost [NIS]", styleStorage.StandardBoldCell);
        CreateCell(headers, 6, "Total revenue [NIS]", styleStorage.StandardBoldCell);
        CreateCell(headers, 7, "          ", styleStorage.StandardBoldCell);
        CreateCell(headers, 8, "Quantity used", styleStorage.StandardBoldCell);
        CreateCell(headers, 9, "Gross earnings [NIS]", styleStorage.StandardBoldCell);
        CreateCell(headers, 10, "Benefits quantity", styleStorage.StandardBoldCell);
        CreateCell(headers, 11, "Benefits cost [NIS]", styleStorage.StandardBoldCell);
        CreateCell(headers, 12, "Total revenue [NIS]", styleStorage.StandardBoldCell);
        CreateCell(headers, 13, "Diff quantity", styleStorage.StandardLeftBoldCell);
        CreateCell(headers, 14, "Diff revenue", styleStorage.StandardRightBoldCell);

        IEnumerable<string> valueNames = currentValues
            .Select(currentValue => currentValue.Name)
            .Concat(referenceValues.Select(x => x.Name))
            .Distinct();

        int index = 0;

        foreach (string valueName in valueNames)
        {
            ExportDailyUsageDto? currentValue = currentValues.FirstOrDefault(value => value.Name == sectionName);

            currentValue ??= new ExportDailyUsageDto
            {
                Name = valueName,
                Quantity = 0,
                Gross = 0,
                BenefitQuantity = 0,
                BenefitCost = 0,
                Cost = 0
            };

            ExportDailyUsageDto? relatedValue = currentValues.FirstOrDefault(value => value.Name == sectionName);

            relatedValue ??= new ExportDailyUsageDto
            {
                Name = valueName,
                Quantity = 0,
                Gross = 0,
                BenefitQuantity = 0,
                BenefitCost = 0,
                Cost = 0
            };

            IRow row = sheet.CreateRow(rowIndex + index + 1);

            CreateCell(row, 1, currentValue.Name, index == 0 ? styleStorage.TopLeftCell : index == currentValues.Count - 1 ? styleStorage.BottomLeftCell : styleStorage.LeftMiddleCell);
            CreateCell(row, 2, currentValue.Quantity.ToString(), index == 0 ? styleStorage.TopLeftCell : index == currentValues.Count - 1 ? styleStorage.BottomLeftCell : styleStorage.LeftMiddleCell);
            CreateCell(row, 3, currentValue.Gross.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 4, currentValue.BenefitQuantity.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 5, currentValue.BenefitCost.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 6, currentValue.Cost.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 7, string.Empty, index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 8, relatedValue.Quantity.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 9, relatedValue.Gross.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 10, relatedValue.BenefitQuantity.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 11, relatedValue.BenefitCost.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 12, relatedValue.Cost.ToString(), index == 0 ? styleStorage.TopMiddleCell : index == currentValues.Count - 1 ? styleStorage.BottomMiddleCell : styleStorage.StandardCell);
            CreateCell(row, 13, (currentValue.Quantity - relatedValue.Quantity).ToString(), index == 0 ? styleStorage.TopLeftCell : index == currentValues.Count - 1 ? styleStorage.BottomLeftCell : styleStorage.LeftMiddleCell);
            CreateCell(row, 14, (currentValue.Cost - relatedValue.Cost).ToString(), index == 0 ? styleStorage.RightTopCell : index == currentValues.Count - 1 ? styleStorage.RightBottomCell : styleStorage.RightMiddleCell);

            index++;
        }

        sheet.CreateRow(sheet.LastRowNum + 2);
    }

    private static void CreateCell(
        IRow row, int position, string value, ICellStyle cellStyle)
    {
        ICell cell = row.CreateCell(position);
        cell.SetCellValue(value);
        cell.CellStyle = cellStyle;
    }

    private static string WriteToFile(XSSFWorkbook workbook)
    {
        Directory.CreateDirectory(FileProvider.ExportFilesDirectory);
        string filePath = Path.Combine(FileProvider.ExportFilesDirectory, $"Summary-{Guid.NewGuid()}");
        using FileStream writeStream = new(filePath, FileMode.Create, FileAccess.Write);
        workbook.Write(writeStream);

        return filePath;
    }
}
