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

        //구현 start

        CreateOriginSheet(workbook, data);
        CreateOnOutRed(workbook, data);
        CreateOnOutGreen(workbook, data);
        CreateOnOutBlue(workbook, data);
        CreateColorInferData(workbook, data);

        //구현 end

        using (FileStream fileStream = new FileStream(filePathWithXlsxExtension, FileMode.Create, FileAccess.Write))
        {
            workbook.Write(fileStream);
        }

        DLogger.Log($"Excel 파일이 성공적으로 생성되었습니다: {filePathWithXlsxExtension}");
    }

    /// <summary>
    /// Origin 시트를 생성
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="data"></param>
    private void CreateOriginSheet(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
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
    }
    /// <summary>
    /// Red 색상에 대한 시트를 생성
    /// </summary>
    private void CreateOnOutRed(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutRed");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key;

            // 열의 헤더 설정 ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // Red 값만 기록
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
    /// Green 색상에 대한 시트를 생성
    /// </summary>
    private void CreateOnOutGreen(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutGreen");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key;

            // 열의 헤더 설정 ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // Green 값만 기록
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
    /// Blue 색상에 대한 시트를 생성
    /// </summary>
    private void CreateOnOutBlue(IWorkbook workbook, Dictionary<int, List<Color32>> data)
    {
        ISheet sheet = workbook.CreateSheet("OnOutBlue");

        foreach (var rowEntry in data)
        {
            int columnIndex = rowEntry.Key;

            // 열의 헤더 설정 ("Ch" + key)
            IRow headerRow = sheet.GetRow(0) ?? sheet.CreateRow(0);
            ICell headerCell = headerRow.CreateCell(columnIndex);
            headerCell.SetCellValue($"Ch{rowEntry.Key}");

            // Blue 값만 기록
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

            // 열의 헤더 설정 ("Ch" + key)
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
