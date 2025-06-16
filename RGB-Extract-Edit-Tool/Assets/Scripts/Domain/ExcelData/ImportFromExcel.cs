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
            DLogger.LogError($"Excel 파일이 존재하지 않습니다: {path}");
        }
        return isExist;
    }

    public SaveData Import(string path)
    {
        var saveData = new SaveData
        {
            orderType = OrderType.Channel_Index, // 필요시 Raw Data에서 읽어올 수 있음  
            recordData = new Dictionary<SavedChannelKey, SavedChannelValue>(),
            colorSheetData = new Dictionary<int, Color32>()
        };

        if (!IsFileExist(path))
            return saveData;

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);

            // 1. Raw Data 시트에서 채널 메타 정보 읽기  
            ISheet rawDataSheet = workbook.GetSheet("Raw Data");
            var channelKeyMap = new Dictionary<int, SavedChannelKey>();
            if (rawDataSheet != null)
            {
                int rowIndex = Definitions.LastHeader; // 데이터는 LastHeader(3)부터 시작  
                while (true)
                {
                    IRow row = rawDataSheet.GetRow(rowIndex++);
                    if (row == null) break;

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
                    channelKeyMap[index] = key;
                }
            }

            // 2. Origin 시트에서 recordData 채우기  
            ISheet originSheet = workbook.GetSheet("Origin");
            if (originSheet != null)
            {
                IRow headerRow = originSheet.GetRow(Definitions.FirstHeaderRow);
                if (headerRow != null)
                {
                    int lastCellNum = headerRow.LastCellNum;
                    for (int col = 0; col < lastCellNum; col++)
                    {
                        ICell headerCell = headerRow.GetCell(col);
                        if (headerCell == null) continue;

                        string header = headerCell.StringCellValue;
                        if (!header.StartsWith("Ch")) continue;

                        if (!int.TryParse(header.Substring(2), out int channelIndex))
                            continue;

                        // Raw Data에서 읽은 key를 우선 사용, 없으면 기본값  
                        SavedChannelKey key = channelKeyMap.ContainsKey(channelIndex)
                            ? channelKeyMap[channelIndex]
                            : new SavedChannelKey
                            {
                                index = channelIndex,
                                position = Vector2.zero,
                                groupName = string.Empty,
                                groupInindex = 0,
                                sortDirection = 0
                            };

                        var colors = new List<Color32>();
                        for (int rowIdx = Definitions.LastHeader; ; rowIdx++)
                        {
                            IRow row = originSheet.GetRow(rowIdx);
                            if (row == null) break;
                            ICell cell = row.GetCell(col);
                            if (cell == null || cell.CellType == CellType.Blank) break;

                            string rgbString = cell.ToString().Replace("\r", "").Replace("\n", "");
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

                        var value = new SavedChannelValue
                        {
                            colors = colors
                        };
                        saveData.recordData[key] = value;
                    }
                }
            }

            // 3. Color Data 시트에서 colorSheetData 채우기  
            ISheet colorSheet = workbook.GetSheet("Color Data");
            if (colorSheet != null)
            {
                for (int rowIdx = 1; ; rowIdx++)
                {
                    IRow row = colorSheet.GetRow(rowIdx);
                    if (row == null) break;

                    ICell indexCell = row.GetCell(0);
                    ICell rCell = row.GetCell(1);
                    ICell gCell = row.GetCell(2);
                    ICell bCell = row.GetCell(3);

                    if (indexCell == null || rCell == null || gCell == null || bCell == null)
                        break;

                    if (!int.TryParse(indexCell.ToString(), out int colorIndex))
                        continue;

                    if (byte.TryParse(rCell.ToString(), out byte r) &&
                        byte.TryParse(gCell.ToString(), out byte g) &&
                        byte.TryParse(bCell.ToString(), out byte b))
                    {
                        saveData.colorSheetData[colorIndex] = new Color32(r, g, b, 255);
                    }
                }
            }
        }

        return saveData;
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
