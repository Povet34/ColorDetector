using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using UnityEngine;

public class ToExcelExportor
{
    public ToExcelExportor() { }

    public void ExportToExcel(Dictionary<int, List<Color32>> data, string filePath)
    {
        // 파일 경로에서 확장자를 제거하고 .xlsx 확장자를 추가
        string filePathWithXlsxExtension = Path.Combine(
            Path.GetDirectoryName(filePath),
            Path.GetFileNameWithoutExtension(filePath) + ".xlsx"
        );

        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Origin");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key; // Key를 열(column)로 사용

            // 열의 헤더 설정 ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0); // 첫 번째 행(헤더)을 가져오거나 생성
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // 데이터 추가
            for (int rowIndex = 0; rowIndex < rowEntry.Value.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + 1) ?? sheet.CreateRow(rowIndex + 1); // 데이터는 두 번째 행부터 시작
                ICell cell = row.CreateCell(columnIndex); // 열(column)에 셀 생성
                Color32 color = rowEntry.Value[rowIndex];

                // RGB 값을 문자열로 저장
                cell.SetCellValue($"{color.r}, {color.g}, {color.b}");
            }
        }

        using (FileStream fileStream = new FileStream(filePathWithXlsxExtension, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

        Debug.Log($"Excel 파일이 성공적으로 생성되었습니다: {filePathWithXlsxExtension}");
    }
}
