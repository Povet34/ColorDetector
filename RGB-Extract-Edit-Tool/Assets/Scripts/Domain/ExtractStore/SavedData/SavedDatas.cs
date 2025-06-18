using System.Collections.Generic;
using UnityEngine;

public enum OrderType //Excel�� ä�������� 
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
    //public List<ColorFlowReasoner.Inference> inferredColorData; //recordData�� ������ �߷��ϴ� ���̱� ������ ���� ����Ǵ°� ó�� ��������, Domain���� ó���ϴ� ��(ex - Excel�������� �߷� ��)�� �ٶ������� �ʱ⿡ ���� ���� �����´�.
}

public struct AdditionalData
{
    public Dictionary<SavedChannelKey, SavedChannelValue> modifiedRecordData;
}

/// <summary>
/// �߷е� ColorFlow ������ (EXCEL�κ��� Import�� �� ��)
/// </summary>
public struct InferredColorFlowData
{
    /// <summary>
    /// key�� index, value�� �ش� ���� index�� �󵵸� ������� ����
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