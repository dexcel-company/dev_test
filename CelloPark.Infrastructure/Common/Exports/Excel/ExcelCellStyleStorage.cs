using NPOI.SS.UserModel;

namespace CelloPark.Infrastructure.Common.Exports.Excel;

internal class ExcelCellStyleStorage
{
    private ExcelCellStyleStorage(IWorkbook workbook)
    {
        ExcelFontStyleStorage = ExcelFontStorage.GetInstance(workbook);

        TopLeftCell = CreateTopLeftCellStyle(workbook);
        BottomLeftCell = CreateBottomLeftCellStyle(workbook);
        BottomMiddleCell = CreateBottomMiddleCellStyle(workbook);
        TopMiddleCell = CreateTopMiddleCellStyle(workbook);
        LeftMiddleCell = CreateLeftMiddleCellStyle(workbook);
        RightTopCell = CreateTopRightCellStyle(workbook);
        RightBottomCell = CreateaBottomRightCellStyle(workbook);
        RightMiddleCell = CreateRightMiddleCellStyle(workbook);
        StandardCell = CreateStandardCellStyle(workbook);
        StandardBoldCell = CreateStandardBoldCellStyle(workbook);
        StandardLeftBoldCell = CreateStandardLeftBoldCellStyle(workbook);
        StandardRightBoldCell = CreateStandardRightBoldCellStyle(workbook);
        StandardGreenCell = CreateStandardGreenCell(workbook);
        StandardYellowCell = CreateStandardYellowCell(workbook);
    }

    public ExcelFontStorage ExcelFontStyleStorage { get; }
    public ICellStyle TopLeftCell { get; }
    public ICellStyle BottomLeftCell { get; }
    public ICellStyle BottomMiddleCell { get; }
    public ICellStyle TopMiddleCell { get; }
    public ICellStyle LeftMiddleCell { get; }
    public ICellStyle RightTopCell { get; }
    public ICellStyle RightBottomCell { get; }
    public ICellStyle RightMiddleCell { get; }
    public ICellStyle StandardCell { get; }
    public ICellStyle StandardBoldCell { get; }
    public ICellStyle StandardLeftBoldCell { get; }
    public ICellStyle StandardRightBoldCell { get; }
    public ICellStyle StandardGreenCell { get; }
    public ICellStyle StandardYellowCell { get; }

    public static ExcelCellStyleStorage GetInstance(IWorkbook workbook)
    {
        return new ExcelCellStyleStorage(workbook);
    }

    private ICellStyle CreateStandardGreenCell(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.FillBackgroundColor = IndexedColors.BrightGreen.Index;

        return cellStyle;
    }

    private ICellStyle CreateStandardYellowCell(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.FillBackgroundColor = IndexedColors.LightYellow.Index;

        return cellStyle;
    }

    private ICellStyle CreateTopLeftCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderLeft = BorderStyle.Medium;
        cellStyle.BorderTop = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateBottomLeftCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderLeft = BorderStyle.Medium;
        cellStyle.BorderBottom = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateBottomMiddleCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderBottom = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateTopMiddleCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderTop = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateLeftMiddleCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderLeft = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateTopRightCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderTop = BorderStyle.Medium;
        cellStyle.BorderRight = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateaBottomRightCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderRight = BorderStyle.Medium;
        cellStyle.BorderBottom = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateRightMiddleCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);
        cellStyle.BorderRight = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateStandardCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.Standard);

        return cellStyle;
    }

    private ICellStyle CreateStandardBoldCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.StandardBold);
        cellStyle.BorderBottom = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateStandardLeftBoldCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.StandardBold);
        cellStyle.BorderTop = BorderStyle.Medium;
        cellStyle.BorderLeft = BorderStyle.Medium;
        cellStyle.BorderBottom = BorderStyle.Medium;

        return cellStyle;
    }

    private ICellStyle CreateStandardRightBoldCellStyle(IWorkbook workbook)
    {
        ICellStyle cellStyle = workbook.CreateCellStyle();

        cellStyle.SetFont(ExcelFontStyleStorage.StandardBold);
        cellStyle.BorderTop = BorderStyle.Medium;
        cellStyle.BorderRight = BorderStyle.Medium;
        cellStyle.BorderBottom = BorderStyle.Medium;

        return cellStyle;
    }
}
