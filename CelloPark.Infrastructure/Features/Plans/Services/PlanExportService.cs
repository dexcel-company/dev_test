using CelloPark.Application.Features.Plans.Services.Abstractions;
using CelloPark.Domain.Features.Packages.Entities.PlanPackages;
using CelloPark.Domain.Features.Plans;
using CelloPark.Infrastructure.Common.Providers;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace CelloPark.Infrastructure.Features.Plans.Services;

internal sealed class PlanExportService :
    IPlanExportService
{
    public FileStream Export(List<Plan> plans)
    {
        XSSFWorkbook workbook = new();

        ISheet sheet = workbook.CreateSheet("Plans");

        IRow headers = sheet.CreateRow(0);

        IFont regular = workbook.CreateFont();
        regular.FontHeightInPoints = 11;
        regular.FontName = "Calibri";

        IFont bold = workbook.CreateFont();
        bold.FontHeightInPoints = 11;
        bold.FontName = "Calibri";
        bold.IsBold = true;

        ICellStyle headerStyle = workbook.CreateCellStyle();
        headerStyle.SetFont(bold);
        headerStyle.FillForegroundColor = IndexedColors.Coral.Index;
        headerStyle.FillPattern = FillPattern.SolidForeground;

        ICellStyle packageStyle = workbook.CreateCellStyle();
        packageStyle.SetFont(regular);
        packageStyle.FillForegroundColor = IndexedColors.BrightGreen.Index;
        packageStyle.FillPattern = FillPattern.SolidForeground;

        ICellStyle planStyle = workbook.CreateCellStyle();
        planStyle.SetFont(regular);
        planStyle.FillForegroundColor = IndexedColors.LightYellow.Index;
        planStyle.FillPattern = FillPattern.SolidForeground;

        ICell cell = headers.CreateCell(0);
        cell.SetCellValue("Plan");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(1);
        cell.SetCellValue("Identifier");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(2);
        cell.SetCellValue("Description");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(3);
        cell.SetCellValue("Status");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(4);
        cell.SetCellValue("Contract type");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(5);
        cell.SetCellValue("Calculation type");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(6);
        cell.SetCellValue("Start date");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(7);
        cell.SetCellValue("End date");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(8);
        cell.SetCellValue("Price");
        cell.CellStyle = headerStyle;

        cell = headers.CreateCell(9);
        cell.SetCellValue("Vat");
        cell.CellStyle = headerStyle;

        foreach (Plan plan in plans)
        {
            IRow row = sheet.CreateRow(sheet.LastRowNum + 1);

            cell = row.CreateCell(0);
            cell.SetCellValue(plan.Name);
            cell.CellStyle = planStyle;

            cell = row.CreateCell(1);
            cell.SetCellValue(plan.ShadowId.ToString());
            cell.CellStyle = planStyle;

            cell = row.CreateCell(2);
            cell.SetCellValue(plan.Description);
            cell.CellStyle = planStyle;

            cell = row.CreateCell(3);
            cell.SetCellValue(plan.Status.ToString());
            cell.CellStyle = planStyle;

            cell = row.CreateCell(4);
            cell.SetCellValue(plan.ContractType.Value);
            cell.CellStyle = planStyle;

            cell = row.CreateCell(5);
            cell.SetCellValue(plan.CalculationType.Value);
            cell.CellStyle = planStyle;

            cell = row.CreateCell(6);
            cell.SetCellValue(plan.StartDate.ToString());
            cell.CellStyle = planStyle;

            cell = row.CreateCell(7);
            cell.SetCellValue(plan.EndDate.ToString());
            cell.CellStyle = planStyle;

            cell = row.CreateCell(8);
            cell.SetCellValue(plan.Price.ToString());
            cell.CellStyle = planStyle;

            cell = row.CreateCell(9);
            cell.SetCellValue(plan.Vat.ToString());
            cell.CellStyle = planStyle;

            if (plan.PlanPackages.Count != 0)
            {
                headers = sheet.CreateRow(sheet.LastRowNum + 1);

                cell = headers.CreateCell(1);
                cell.SetCellValue("Package");
                cell.CellStyle = headerStyle;

                cell = headers.CreateCell(2);
                cell.SetCellValue("Identifier");
                cell.CellStyle = headerStyle;

                cell = headers.CreateCell(3);
                cell.SetCellValue("Price");
                cell.CellStyle = headerStyle;

                cell = headers.CreateCell(4);
                cell.SetCellValue("Vat");
                cell.CellStyle = headerStyle;

                foreach (PlanPackage planPackage in plan.PlanPackages)
                {
                    row = sheet.CreateRow(sheet.LastRowNum + 1);

                    cell = row.CreateCell(1);
                    cell.SetCellValue(planPackage.Package.Name);
                    cell.CellStyle = packageStyle;

                    cell = row.CreateCell(2);
                    cell.SetCellValue(planPackage.Price.ToString());
                    cell.CellStyle = packageStyle;

                    cell = row.CreateCell(3);
                    cell.SetCellValue(planPackage.Vat.ToString());
                    cell.CellStyle = packageStyle;
                }
            }

            sheet.CreateRow(sheet.LastRowNum + 1);
        }

        sheet.AutoSizeColumn(0);
        sheet.AutoSizeColumn(1);
        sheet.AutoSizeColumn(3);
        sheet.AutoSizeColumn(4);
        sheet.AutoSizeColumn(5);
        sheet.AutoSizeColumn(6);
        sheet.AutoSizeColumn(7);
        sheet.AutoSizeColumn(8);
        sheet.AutoSizeColumn(9);

        string filePath = WriteToFile(workbook);

        FileStream readStream = new(filePath, FileMode.Open, FileAccess.Read);

        return readStream;
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
