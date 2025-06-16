using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExportToExcel : IToExport
{
    public void Export(SaveData data, string filePath)
    {
        string filePathWithXlsxExtension = Path.Combine(
            Path.GetDirectoryName(filePath),
            Path.GetFileNameWithoutExtension(filePath) + ".xlsx"
        );

        IWorkbook workbook = new XSSFWorkbook();

        //CreatePositionSheet(workbook, data);
        CreateOriginSheet(workbook, data.recordData);
        CreateOnOutRed(workbook, data.recordData);
        CreateOnOutGreen(workbook, data.recordData);
        CreateOnOutBlue(workbook, data.recordData);
        CreateInferedColor(workbook, data.recordData);
        CreateColorIndexSheet(workbook, data.colorSheetData);

        CreateRawDataSheet(workbook, data.recordData);

        using (FileStream fileStream = new FileStream(filePathWithXlsxExtension, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

        DLogger.Log($"Excel ������ ���������� �����Ǿ����ϴ�: {filePathWithXlsxExtension}");
    }

    /// <summary>
    /// Position ��Ʈ ���� (Raw Data ��Ʈ�� �ߺ��Ǵ� �����̹Ƿ� �ʿ����� ���� �� ����)
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreatePositionSheet(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("Position");
        IRow headerRow = sheet.CreateRow(Definitions.FirstHeaderRow);
        headerRow.CreateCell(0).SetCellValue("Channel Index");
        headerRow.CreateCell(1).SetCellValue("Position X");
        headerRow.CreateCell(2).SetCellValue("Position Y");

        int rowIndex = Definitions.LastHeader;
        foreach (var rowEntry in data)
        {
            IRow row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(rowEntry.Key.index);
            row.CreateCell(1).SetCellValue(rowEntry.Key.position.x);
            row.CreateCell(2).SetCellValue(rowEntry.Key.position.y);
        }
    }

    /// <summary>
    /// Origin ��Ʈ ����
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateOriginSheet(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("Origin");
        SetGroupNameHeader(sheet, data);

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key.index;

            IRow headerRow = sheet.GetRow(Definitions.FirstHeaderRow) ?? sheet.CreateRow(Definitions.FirstHeaderRow);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key.index}");

            var colors = rowEntry.Value.colors;
            for (int rowIndex = 0; rowIndex < colors.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + Definitions.LastHeader) ?? sheet.CreateRow(rowIndex + Definitions.LastHeader);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = colors[rowIndex];
                cell.SetCellValue($"{color.r}, {color.g}, {color.b}");
            }
        }
    }

    /// <summary>
    /// OnOutRed ��Ʈ ����
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateOnOutRed(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutRed");
        SetGroupNameHeader(sheet, data);

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key.index;

            IRow headerRow = sheet.GetRow(Definitions.FirstHeaderRow) ?? sheet.CreateRow(Definitions.FirstHeaderRow);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key.index}");

            var colors = rowEntry.Value.colors;
            for (int rowIndex = 0; rowIndex < colors.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + Definitions.LastHeader) ?? sheet.CreateRow(rowIndex + Definitions.LastHeader);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = colors[rowIndex];
                cell.SetCellValue(color.r);
            }
        }
    }

    /// <summary>
    /// OnOutGreen ��Ʈ ����
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateOnOutGreen(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutGreen");
        SetGroupNameHeader(sheet, data);

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key.index;

            IRow headerRow = sheet.GetRow(Definitions.FirstHeaderRow) ?? sheet.CreateRow(Definitions.FirstHeaderRow);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key.index}");

            var colors = rowEntry.Value.colors;
            for (int rowIndex = 0; rowIndex < colors.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + Definitions.LastHeader) ?? sheet.CreateRow(rowIndex + Definitions.LastHeader);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = colors[rowIndex];
                cell.SetCellValue(color.g);
            }
        }
    }

    /// <summary>
    /// OnOutBlue ��Ʈ ����
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateOnOutBlue(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutBlue");
        SetGroupNameHeader(sheet, data);

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key.index;

            IRow headerRow = sheet.GetRow(Definitions.FirstHeaderRow) ?? sheet.CreateRow(Definitions.FirstHeaderRow);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key.index}");

            var colors = rowEntry.Value.colors;
            for (int rowIndex = 0; rowIndex < colors.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + Definitions.LastHeader) ?? sheet.CreateRow(rowIndex + Definitions.LastHeader);
                ICell cell = row.CreateCell(columnIndex);
                Color32 color = colors[rowIndex];
                cell.SetCellValue(color.b);
            }
        }
    }

    /// <summary>
    /// Infered Color ��Ʈ ����
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateInferedColor(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("Infered Color");
        SetGroupNameHeader(sheet, data);

        ColorFlowReasoner colorFlowReasoner = new ColorFlowReasoner();

        // Dictionary<SavedChannelKey, SavedChannelValue> -> Dictionary<int, List<Color32>> ��ȯ
        var simpleData = new Dictionary<int, List<Color32>>();
        foreach (var rowEntry in data)
        {
            simpleData[rowEntry.Key.index] = rowEntry.Value.colors;
        }

        List<ColorFlowReasoner.Inference> inference = colorFlowReasoner.Infer(simpleData);

        foreach (var rowEntry in inference)
        {
            int columnIndex = rowEntry.chIndex;

            IRow headerRow = sheet.GetRow(Definitions.FirstHeaderRow) ?? sheet.CreateRow(Definitions.FirstHeaderRow);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.chIndex}");

            for (int rowIndex = 0; rowIndex < rowEntry.steps.Count; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex + Definitions.LastHeader) ?? sheet.CreateRow(rowIndex + Definitions.LastHeader);
                ICell cell = row.CreateCell(columnIndex);

                int index = rowEntry.steps[rowIndex].colorindex;
                float density = rowEntry.steps[rowIndex].density;

                if (index == 0)
                    cell.SetCellValue($"0.00");
                else
                    cell.SetCellValue($"{index} ({density.ToString("N2")})");
            }
        }
    }

    /// <summary>
    /// Raw Data ��Ʈ ����
    /// </summary>
    /// <param name="curSheet"></param>
    /// <param name="data"></param>
    private void CreateRawDataSheet(IWorkbook workbook, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        ISheet sheet = workbook.CreateSheet("Raw Data");

        // ��� �ۼ�
        IRow headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Index");
        headerRow.CreateCell(1).SetCellValue("Position X");
        headerRow.CreateCell(2).SetCellValue("Position Y");
        headerRow.CreateCell(3).SetCellValue("Group Name");
        headerRow.CreateCell(4).SetCellValue("Group In Index");
        headerRow.CreateCell(5).SetCellValue("SortDirection");

        int rowIndex = 1;
        foreach (var rowEntry in data)
        {
            var key = rowEntry.Key;
            IRow row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(key.index);
            row.CreateCell(1).SetCellValue(key.position.x);
            row.CreateCell(2).SetCellValue(key.position.y);
            row.CreateCell(3).SetCellValue(key.groupName ?? "");
            row.CreateCell(4).SetCellValue(key.groupInindex);
            row.CreateCell(5).SetCellValue(key.sortDirection);
        }
    }


    private void CreateColorIndexSheet(IWorkbook workbook, Dictionary<int, Color32> colorData)
    {
        ISheet sheet = workbook.CreateSheet("Color Data");

        // ��� �ۼ�
        IRow headerRow = sheet.CreateRow(0);
        headerRow.CreateCell(0).SetCellValue("Index");
        headerRow.CreateCell(1).SetCellValue("R");
        headerRow.CreateCell(2).SetCellValue("G");
        headerRow.CreateCell(3).SetCellValue("B");

        int rowIndex = 1;
        foreach (var kvp in colorData)
        {
            int index = kvp.Key;
            Color32 color = kvp.Value;

            IRow row = sheet.CreateRow(rowIndex++);
            row.CreateCell(0).SetCellValue(index);
            row.CreateCell(1).SetCellValue(color.r);
            row.CreateCell(2).SetCellValue(color.g);
            row.CreateCell(3).SetCellValue(color.b);
        }
    }

    /// <summary>
    /// �׷� �̸� ��� ����
    /// </summary>
    private void SetGroupNameHeader(ISheet curSheet, Dictionary<SavedChannelKey, SavedChannelValue> data)
    {
        // 0��° �࿡ ��� �׷��� �ִ� ä���� �÷��� �׷� �̸��� ǥ��
        IRow groupHeaderRow = curSheet.GetRow(Definitions.GroupHeaderRow) ?? curSheet.CreateRow(Definitions.GroupHeaderRow);

        foreach (var rowEntry in data)
        {
            string groupName = rowEntry.Key.groupName;
            int columnIndex = rowEntry.Key.index;

            if (!string.IsNullOrEmpty(groupName))
            {
                groupHeaderRow.CreateCell(columnIndex).SetCellValue(groupName);
            }
        }
    }
}
