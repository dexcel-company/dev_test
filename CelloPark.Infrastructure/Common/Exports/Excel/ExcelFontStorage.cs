using NPOI.SS.UserModel;

namespace CelloPark.Infrastructure.Common.Exports.Excel;

internal class ExcelFontStorage
{
    public ExcelFontStorage(IWorkbook workbook)
    {
        Standard = CreateStandardFont(workbook);
        StandardBold = CreateStandardBoldFont(workbook);
    }

    public IFont Standard { get; }
    public IFont StandardBold { get; }

    public static ExcelFontStorage GetInstance(IWorkbook workbook)
    {
        return new ExcelFontStorage(workbook);
    }

    private static IFont CreateStandardFont(IWorkbook workbook)
    {
        IFont font = workbook.CreateFont();

        font.FontHeightInPoints = 11;
        font.FontName = "Calibri";

        return font;
    }

    private static IFont CreateStandardBoldFont(IWorkbook workbook)
    {
        IFont font = workbook.CreateFont();

        font.FontHeightInPoints = 11;
        font.FontName = "Calibri";
        font.IsBold = true;

        return font;
    }
}
