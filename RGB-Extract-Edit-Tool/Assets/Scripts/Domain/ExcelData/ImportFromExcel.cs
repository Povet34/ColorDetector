using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class ImportFromExcel : IImportFrom
{

    bool IsFileExist(string path)
    {
        bool isExist = File.Exists(path);
        if (!isExist)
        {
            DLogger.LogError($"Excel ������ �������� �ʽ��ϴ�: {path}");
        }

        return isExist;
    }

    public Dictionary<SavedChannelKey, SavedChannelValue> Import(string path)
    {
        if (!IsFileExist(path))
            return null;

        var result = new Dictionary<SavedChannelKey, SavedChannelValue>();

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet("Origin");
            if (sheet == null)
            {
                DLogger.LogError("Origin ��Ʈ�� ã�� �� �����ϴ�.");
                return result;
            }

            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
            {
                DLogger.LogError("��� ���� �����ϴ�.");
                return result;
            }

            int lastCellNum = headerRow.LastCellNum;

            for (int col = 0; col < lastCellNum; col++)
            {
                ICell headerCell = headerRow.GetCell(col);
                if (headerCell == null) continue;

                string header = headerCell.StringCellValue;
                // ����� "Ch{index}" �������� Ȯ��
                if (!header.StartsWith("Ch")) continue;

                if (!int.TryParse(header.Substring(2), out int channelIndex))
                    continue;

                var colors = new List<Color32>();

                // ������ �� �б�
                for (int rowIdx = 1; ; rowIdx++)
                {
                    IRow row = sheet.GetRow(rowIdx);
                    if (row == null) break;
                    ICell cell = row.GetCell(col);
                    if (cell == null || cell.CellType == CellType.Blank) break;

                    string rgbString = cell.ToString();
                    var rgbParts = rgbString.Split(',');
                    if (rgbParts.Length != 3) break;

                    if (byte.TryParse(rgbParts[0].Trim(), out byte r) &&
                        byte.TryParse(rgbParts[1].Trim(), out byte g) &&
                        byte.TryParse(rgbParts[2].Trim(), out byte b))
                    {
                        colors.Add(new Color32(r, g, b, 255));
                    }
                    else
                    {
                        break;
                    }
                }

                var key = new SavedChannelKey
                {
                    index = channelIndex,
                    position = Vector2.zero,
                    groupName = string.Empty,
                    groupInindex = 0
                };
                var value = new SavedChannelValue
                {
                    colors = colors
                };
                result.Add(key, value);
            }
        }

        return result;
    }

    public List<SavedChannelKey> LoadChannelInfos(string path)
    {
        if (!IsFileExist(path))
            return null;

        var result = new List<SavedChannelKey>();

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet("Raw Data");
            if (sheet == null)
            {
                DLogger.LogError("Raw Data ��Ʈ�� ã�� �� �����ϴ�.");  
                return result;
            }

            // 0�� ���� ����̹Ƿ� 1�� ����� ����
            int rowIndex = 1;
            while (true)
            {
                IRow row = sheet.GetRow(rowIndex++);
                if (row == null) break;

                // �ʼ����� ������ ����
                ICell indexCell = row.GetCell(0);
                ICell posXCell = row.GetCell(1);
                ICell posYCell = row.GetCell(2);
                ICell groupNameCell = row.GetCell(3);
                ICell groupInIndexCell = row.GetCell(4);
                ICell sortDirectionCell = row.GetCell(5);

                if (indexCell == null || posXCell == null || posYCell == null || groupInIndexCell == null || sortDirectionCell == null)
                    break;

                int index = (int)indexCell.NumericCellValue;
                float posX = (float)posXCell.NumericCellValue;
                float posY = (float)posYCell.NumericCellValue;
                string groupName = groupNameCell?.ToString() ?? string.Empty;
                int groupInIndex = (int)groupInIndexCell.NumericCellValue;
                int sortDirection = (int)sortDirectionCell.NumericCellValue;

                var key = new SavedChannelKey
                {
                    index = index,
                    position = new Vector2(posX, posY),
                    groupName = groupName,
                    groupInindex = groupInIndex,
                    sortDirection = sortDirection,
                };

                result.Add(key);
            }
        }

        return result;
    }
}
