using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace AccuracyVASMinimalAPI.Extensions
{
    public static class NpoiExtensions
    {
        public static IRow CreateOrGetRow(this ISheet sheet, int rowIndex)
        {
            return sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);
        }

        public static ICell CreateOrGetCell(this IRow row, int colIndex)
        {
            return row.GetCell(colIndex) ?? row.CreateCell(colIndex);
        }

        public static string GetValueStrCell(this ICell cell)
        {
            if (cell.CellType == CellType.String)
            {
                return cell.StringCellValue;
            }
            else if (cell.CellType == CellType.Numeric)
            {
                return cell.NumericCellValue.ToString();
            }
            else
            {
                return "";
            }
        }

        public static void AutoSizeHeigthCell(this IRow row, int colIndex, bool isSubQuestion = false)
        {
            short hDefault = 22;
            short hMin = 44;
            double limit = 90.0;
            ICell cell = row.CreateOrGetCell(colIndex);
            string v = cell.StringCellValue;

            int x = (int)Math.Ceiling(v.Length / limit);
            short y = (short)(hDefault * x);
            short z = y > hMin ? y : isSubQuestion ? y : hMin; // isSubQuestion ? y : ;
            row.HeightInPoints = z;
            //row.HeightInPoints= hDefault;
        }

        public static short PxToPoint(this IRow row, int pixels)
        {
            float points = (pixels / 96f) * 72f;
            return (short)points;
        }

        public static void SetFontForMergedRegion(this ISheet sheet, CellRangeAddress region, XSSFCellStyle style)
        {
            // Establecer el estilo de celda para cada celda en el rango combinado
            for (int rowIndex = region.FirstRow; rowIndex <= region.LastRow; rowIndex++)
            {
                var row = sheet.CreateOrGetRow(rowIndex);
                for (int colIndex = region.FirstColumn; colIndex <= region.LastColumn; colIndex++)
                {
                    var cell = row.CreateOrGetCell(colIndex);
                    cell.CellStyle = style;
                }
            }
        }

        public static CellRangeAddress GetMergedRegionForCell(this ISheet sheet, int rowIndex, int columnIndex)
        {
            var mergedRegions = sheet.MergedRegions;
            foreach (var region in mergedRegions)
            {
                if (rowIndex >= region.FirstRow && rowIndex <= region.LastRow &&
                    columnIndex >= region.FirstColumn && columnIndex <= region.LastColumn)
                {
                    return region;
                }
            }
            return null;
        }

        public static ICellStyle CellRegularStyle(this XSSFWorkbook workbook, double fontSize = 14)
        {
            IFont regularFont = workbook.CreateFont();
            regularFont.FontHeightInPoints = fontSize;
            regularFont.FontName = "Arial";

            ICellStyle regularStyle = workbook.CreateCellStyle();
            regularStyle.VerticalAlignment = VerticalAlignment.Center;
            regularStyle.SetFont(regularFont);

            return regularStyle;
        }
        public static ICellStyle CellBoldStyle(this XSSFWorkbook workbook, double fontSize = 14)
        {
            return workbook.CellBoldStyle(IndexedColors.Black, fontSize);
        }
        public static ICellStyle CellBoldStyle(this XSSFWorkbook workbook, IndexedColors color, double fontSize = 14)
        {
            IFont boldFont = workbook.CreateFont();
            boldFont.FontHeightInPoints = fontSize;
            boldFont.FontName = "Arial";
            boldFont.IsBold = true;
            boldFont.Color = color.Index;

            ICellStyle boldStyle = workbook.CreateCellStyle();
            boldStyle.VerticalAlignment = VerticalAlignment.Center;
            boldStyle.SetFont(boldFont);

            return boldStyle;
        }

        public static ICellStyle CellBorderStyle(this XSSFWorkbook workbook, ICellStyle cellStyle)
        {
            ICellStyle borderStyle = workbook.CreateCellStyle();
            borderStyle.CloneStyleFrom(cellStyle);

            borderStyle.BorderTop = BorderStyle.Thin;
            borderStyle.BorderBottom = BorderStyle.Thin;
            borderStyle.BorderLeft = BorderStyle.Thin;
            borderStyle.BorderRight = BorderStyle.Thin;
            borderStyle.TopBorderColor = IndexedColors.Black.Index;
            borderStyle.BottomBorderColor = IndexedColors.Black.Index;
            borderStyle.LeftBorderColor = IndexedColors.Black.Index;
            borderStyle.RightBorderColor = IndexedColors.Black.Index;

            return borderStyle;
        }

        public static void RemplaceCellValueInformationStyle(this XSSFWorkbook workbook, ISheet sheet, int _row, int _cell, string value, string remplace, double fontSize = 14)
        {
            IFont boldFont = workbook.CreateFont();
            boldFont.FontHeightInPoints = fontSize;
            boldFont.FontName = "Arial";
            boldFont.IsBold = true;

            IFont regularFont = workbook.CreateFont();
            regularFont.FontHeightInPoints = fontSize;
            regularFont.FontName = "Arial";

            ICellStyle style = workbook.CreateCellStyle();
            style.VerticalAlignment = VerticalAlignment.Center;

            IRow row = sheet.GetRow(_row);
            ICell cell = row.GetCell(_cell);
            string str = cell.StringCellValue.Replace(remplace, value);
            cell.SetCellValue(str);
            IRichTextString richText = cell.RichStringCellValue;

            richText.ApplyFont(0, str.Length - value.Length - 1, boldFont);
            richText.ApplyFont(str.Length - value.Length, str.Length, regularFont);
            cell.SetCellValue(richText);
            cell.CellStyle = workbook.CellBorderStyle(style);

        }

        public static void RemplaceCellValue(this XSSFWorkbook workbook, ISheet sheet, int _row, int _cell, string value, string remplace, ICellStyle _style)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.CloneStyleFrom(_style);

            IRow row = sheet.GetRow(_row);
            ICell cell = row.GetCell(_cell);
            string str = cell.StringCellValue.Replace(remplace, value);
            cell.SetCellValue(str);
            IRichTextString richText = cell.RichStringCellValue;
            richText.ApplyFont(0, cell.StringCellValue.Length, style.GetFont(workbook));
            cell.SetCellValue(richText);
            cell.CellStyle = (XSSFCellStyle)style;

        }
    }
}
