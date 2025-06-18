using System.Collections.Generic;
using UnityEngine;

public enum OrderType //Excel에 채널정렬이 
{
    Channel_Index,
    Group_Index     
}

public struct ImportResult
{
    public string path;
    public OriginData originData;
    public InferredColorFlowData inferredColorFlowData;
    public AdditionalData additionalData;

    public ImportResult(string path, OriginData originData, InferredColorFlowData inferredColorFlowData, AdditionalData additionalData)
    {
        this.path = path;
        this.originData = originData;
        this.inferredColorFlowData = inferredColorFlowData;
        this.additionalData = additionalData;
    }
}

public struct OriginData
{
    public OrderType orderType;
    public Dictionary<SavedChannelKey, SavedChannelValue> recordData;
    public Dictionary<int, Color32> colorSheetData;
    //public List<ColorFlowReasoner.Inference> inferredColorData; //recordData을 가지고 추론하는 것이기 때문에 따로 없어도되는것 처럼 보이지만, Domain에서 처리하는 것(ex - Excel생성에서 추론 등)은 바람직하지 않기에 먼저 만들어서 가져온다.
}

public struct AdditionalData
{
    public Dictionary<SavedChannelKey, SavedChannelValue> modifiedRecordData;
}

/// <summary>
/// 추론된 ColorFlow 데이터 (EXCEL로부터 Import할 때 씀)
/// </summary>
public struct InferredColorFlowData
{
    /// <summary>
    /// key는 index, value는 해당 색상 index와 농도를 순서대로 나열
    /// </summary>
    public Dictionary<int, List<ColorFlowReasoner.Step>> inferredColorFlow;
}


public struct SavedChannelKey
{
    public int index;
    public Vector2 position;
    public string groupName;
    public int groupInindex;
    public int sortDirection;
}

public struct SavedChannelValue
{
    public List<Color32> colors;
}

public struct SavedColorData
{
    public int index;
    public Color32 color;
}