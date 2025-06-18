using System.Collections.Generic;
using System.IO;
using UnityEngine;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

public class ImportFromExcel : IImportFrom
{
    private bool IsFileExist(string path)
    {
        bool isExist = File.Exists(path);
        if (!isExist)
        {
            DLogger.LogError($"Excel 파일이 존재하지 않습니다: {path}");
        }
        return isExist;
    }

    public ImportResult Import(string path)
    {
        if (!IsFileExist(path))
            return default(ImportResult);

        return new ImportResult(
                path, 
                ImportOriginData(path), 
                ImportInferredColorFlowData(path),
                ImportAdditionalData(path)
            );
    }

    private Dictionary<int, SavedChannelKey> GetSavedChannelKeys(string path)
    {
        var dict = new Dictionary<int, SavedChannelKey>();

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet(Definitions.Channel_Data);
            if (sheet == null)
                return dict;

            int rowIndex = Definitions.FirstHeaderRow;
            while (true)
            {
                IRow row = sheet.GetRow(rowIndex++);
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

                dict[index] = key;
            }
        }

        return dict;
    }

    private OriginData ImportOriginData(string path)
    {
        var originData = new OriginData
        {
            orderType = OrderType.Channel_Index,
            recordData = new Dictionary<SavedChannelKey, SavedChannelValue>(),
            colorSheetData = new Dictionary<int, Color32>(),
        };

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);

            // 1. Raw Data 시트에서 채널 메타 정보 읽기
            ISheet rawDataSheet = workbook.GetSheet(Definitions.Channel_Data);
            var channelKeyMap = GetSavedChannelKeys(path);

            // 2. Origin 시트에서 recordData 채우기
            ISheet originSheet = workbook.GetSheet(Definitions.OriginColor);
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
                        originData.recordData[key] = value;
                    }
                }
            }

            // 3. Color Data 시트에서 colorSheetData 채우기
            ISheet colorSheet = workbook.GetSheet(Definitions.ColorPalette);
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
                        originData.colorSheetData[colorIndex] = new Color32(r, g, b, 255);
                    }
                }
            }
        }
        return originData;
    }

    private InferredColorFlowData ImportInferredColorFlowData(string path)
    {
        var inferredColorFlowData = new InferredColorFlowData
        {
            inferredColorFlow = new Dictionary<int, List<ColorFlowReasoner.Step>>()
        };

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet(Definitions.InferredColor);
            if (sheet == null)
                return inferredColorFlowData;

            // 헤더 행에서 chIndex 추출
            IRow headerRow = sheet.GetRow(Definitions.FirstHeaderRow);
            if (headerRow == null)
                return inferredColorFlowData;

            int lastCellNum = headerRow.LastCellNum;
            for (int col = 0; col < lastCellNum; col++)
            {
                ICell headerCell = headerRow.GetCell(col);
                if (headerCell == null) continue;

                string header = headerCell.StringCellValue;
                if (!header.StartsWith("Ch")) continue;

                if (!int.TryParse(header.Substring(2), out int chIndex))
                    continue;

                var steps = new List<ColorFlowReasoner.Step>();

                // 데이터 행 파싱
                for (int rowIdx = Definitions.LastHeader; ; rowIdx++)
                {
                    IRow row = sheet.GetRow(rowIdx);
                    if (row == null) break;
                    ICell cell = row.GetCell(col);
                    if (cell == null || cell.CellType == CellType.Blank) break;

                    string cellValue = cell.ToString().Trim();

                    if (cellValue == "0.00")
                    {
                        steps.Add(new ColorFlowReasoner.Step { colorindex = -1, density = 0f });
                    }
                    else
                    {
                        // 예: "5 (0.23)"
                        int leftParen = cellValue.IndexOf('(');
                        int rightParen = cellValue.IndexOf(')');
                        if (leftParen > 0 && rightParen > leftParen)
                        {
                            string colorIndexStr = cellValue.Substring(0, leftParen).Trim();
                            string densityStr = cellValue.Substring(leftParen + 1, rightParen - leftParen - 1).Trim();

                            if (int.TryParse(colorIndexStr, out int colorindex) &&
                                float.TryParse(densityStr, out float density))
                            {
                                steps.Add(new ColorFlowReasoner.Step { colorindex = colorindex, density = density });
                            }
                            else
                            {
                                // 파싱 실패 시 -1 처리
                                steps.Add(new ColorFlowReasoner.Step { colorindex = -1, density = 0f });
                            }
                        }
                        else
                        {
                            // 포맷이 맞지 않으면 -1 처리
                            steps.Add(new ColorFlowReasoner.Step { colorindex = -1, density = 0f });
                        }
                    }
                }

                inferredColorFlowData.inferredColorFlow[chIndex] = steps;
            }
        }

        return inferredColorFlowData;
    }

    private AdditionalData ImportAdditionalData(string path)
    {
        var additionalData = new AdditionalData
        {
            modifiedRecordData = new Dictionary<SavedChannelKey, SavedChannelValue>()
        };

        // 1. 기존 채널 키 정보 맵을 가져온다
        var channelKeyMap = GetSavedChannelKeys(path);

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);

            // "ModifiedColor" 시트 읽기
            ISheet modifiedSheet = workbook.GetSheet(Definitions.ModifiedColor);
            if (modifiedSheet == null)
                return additionalData;

            IRow headerRow = modifiedSheet.GetRow(Definitions.FirstHeaderRow);
            if (headerRow == null)
                return additionalData;

            int lastCellNum = headerRow.LastCellNum;
            for (int col = 0; col < lastCellNum; col++)
            {
                ICell headerCell = headerRow.GetCell(col);
                if (headerCell == null) continue;

                string header = headerCell.StringCellValue;
                if (!header.StartsWith("Ch")) continue;

                if (!int.TryParse(header.Substring(2), out int channelIndex))
                    continue;

                // 각 컬럼별로 색상 리스트 읽기
                var colors = new List<Color32>();
                for (int rowIdx = Definitions.LastHeader; ; rowIdx++)
                {
                    IRow row = modifiedSheet.GetRow(rowIdx);
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

                // 기존 key를 사용, 없으면 기본값
                SavedChannelKey key = channelKeyMap.TryGetValue(channelIndex, out var foundKey)
                    ? foundKey
                    : new SavedChannelKey
                    {
                        index = channelIndex,
                        position = Vector2.zero,
                        groupName = string.Empty,
                        groupInindex = 0,
                        sortDirection = 0
                    };

                var value = new SavedChannelValue
                {
                    colors = colors
                };

                additionalData.modifiedRecordData[key] = value;
            }
        }

        return additionalData;
    }

    public List<SavedChannelKey> LoadChannelInfos(string path)
    {
        if (!IsFileExist(path))
            return null;

        var result = new List<SavedChannelKey>();

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            IWorkbook workbook = new XSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet(Definitions.Channel_Data);
            if (sheet == null)
            {
                DLogger.LogError("Raw Data is not founds.");
                return result;
            }

            int rowIndex = 1;
            while (true)
            {
                IRow row = sheet.GetRow(rowIndex++);
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

                result.Add(key);
            }
        }

        return result;
    }
}
