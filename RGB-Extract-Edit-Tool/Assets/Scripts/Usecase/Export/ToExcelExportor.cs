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

        //���� start

        CreateOriginSheet(workbook, data);
        CreateOnOutRed(workbook, data);
        CreateOnOutGreen(workbook, data);
        CreateOnOutBlue(workbook, data);
        CreateColorInferData(workbook, data);

        //���� end

        using (FileStream fileStream = new FileStream(filePathWithXlsxExtension, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

        DLogger.Log($"Excel ������ ���������� �����Ǿ����ϴ�: {filePathWithXlsxExtension}");
    }

    /// <summary>
    /// Origin ��Ʈ�� ����
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateOriginSheet(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
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
    }
    /// <summary>
    /// Red ���� ���� ��Ʈ�� ����
    /// </summary>
    private void CreateOnOutRed(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutRed");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key;

            // ���� ��� ���� ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // Red ���� ���
            for (int rowIndex = 0; rowIndex < rowEntry.Value.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + 1) ?? sheet.CreateRow(rowIndex + 1);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = rowEntry.Value[rowIndex];
                cell.SetCellValue(color.r);
            }
        }
    }

    /// <summary>
    /// Green ���� ���� ��Ʈ�� ����
    /// </summary>
    private void CreateOnOutGreen(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutGreen");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key;

            // ���� ��� ���� ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // Green ���� ���
            for (int rowIndex = 0; rowIndex < rowEntry.Value.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + 1) ?? sheet.CreateRow(rowIndex + 1);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = rowEntry.Value[rowIndex];
                cell.SetCellValue(color.g);
            }
        }
    }

    /// <summary>
    /// Blue ���� ���� ��Ʈ�� ����
    /// </summary>
    private void CreateOnOutBlue(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutBlue");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key;

            // ���� ��� ���� ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // Blue ���� ���
            for (int rowIndex = 0; rowIndex < rowEntry.Value.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + 1) ?? sheet.CreateRow(rowIndex + 1);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = rowEntry.Value[rowIndex];
                cell.SetCellValue(color.b);
            }
        }
    }


    private void CreateColorInferData(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("ColorInfer");
        ColorFlowReasoner colorFlowReasoner = new ColorFlowReasoner();

        List<ColorFlowReasoner.Inference> inference = colorFlowReasoner.Infer(data);

        foreach (var rowEntry in inference)
        {
            int columnIndex = rowEntry.chIndex;

            // ���� ��� ���� ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.chIndex}");

            for (int rowIndex = 0; rowIndex < rowEntry.steps.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + 1) ?? sheet.CreateRow(rowIndex + 1);
                ICell cell = row.CreateCell(columnIndex);
                
                int index = rowEntry.steps[rowIndex].colorindex;
                float density = rowEntry.steps[rowIndex].density;

                cell.SetCellValue($"{index}({density.ToString("N2")})");
            }
        }
    }
}
