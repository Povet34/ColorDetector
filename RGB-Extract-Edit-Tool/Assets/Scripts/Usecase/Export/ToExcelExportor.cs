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
        // ���� ��ο��� Ȯ���ڸ� �����ϰ� .xlsx Ȯ���ڸ� �߰�
        string filePathWithXlsxExtension = Path.Combine(
            Path.GetDirectoryName(filePath),
            Path.GetFileNameWithoutExtension(filePath) + ".xlsx"
        );

        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet = workbook.CreateSheet("Origin");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key; // Key�� ��(column)�� ���

            // ���� ��� ���� ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0); // ù ��° ��(���)�� �������ų� ����
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // ������ �߰�
            for (int rowIndex = 0; rowIndex < rowEntry.Value.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + 1) ?? sheet.CreateRow(rowIndex + 1); // �����ʹ� �� ��° ����� ����
                ICell cell = row.CreateCell(columnIndex); // ��(column)�� �� ����
                Color32 color = rowEntry.Value[rowIndex];

                // RGB ���� ���ڿ��� ����
                cell.SetCellValue($"{color.r}, {color.g}, {color.b}");
            }
        }

        using (FileStream fileStream = new FileStream(filePathWithXlsxExtension, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

        Debug.Log($"Excel ������ ���������� �����Ǿ����ϴ�: {filePathWithXlsxExtension}");
    }
}
