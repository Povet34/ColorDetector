using DataEdit;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DataEdit.DataEditMain;


    
public class ChannelEditPanel : MonoBehaviour, 
    IEventBusSubscriber<StartEditArgs>
{
    #region Injection

    EditDataUpdater editDataUpdater;

    #endregion

    [SerializeField] SummationView summationView;
    [SerializeField] PaintArea paintArea;
    [SerializeField] Palette palette;

    [SerializeField] Button saveButton;
    [SerializeField] Button saveBlendButton;
    [SerializeField] Button cancelButton;

    int channelIndex;
    Brush brush;

    public void Init(EditDataInjection editDataInjection)
    {
        editDataUpdater = editDataInjection.editDataUpdater;
        
        this.SubscribeEvent<StartEditArgs>();

        saveButton.onClick.AddListener(SaveEdit);
        cancelButton.onClick.AddListener(CancelEdit);
        saveBlendButton.onClick.AddListener(SaveBlendEdit);
    }

    private void OnDestroy()
    {
        this.UnsubscribeEvent<StartEditArgs>();
    }

    private void CancelEdit()
    {
        gameObject.SetActive(false);
    }

    private void SaveEdit()
    {
        editDataUpdater.UpdateRawData_OneChannel(channelIndex, paintArea.GetRawColors());
        Bus<RefreshPanelArgs>.Raise(new RefreshPanelArgs(1));

        gameObject.SetActive(false);
        channelIndex = -1;
    }

    /// <summary>
    /// Duty ���� ������ ��Ģ�� �ǰ��� �������� ���·� ������ �� �ִ� ���
    /// </summary>
    private void SaveBlendEdit()
    {
        const int MIN_CONTINUOUS_COUNT = 5;

        editDataUpdater.UpdateRawData_OneChannel(channelIndex, paintArea.GetRawColors());
        Bus<RefreshPanelArgs>.Raise(new RefreshPanelArgs(1));
        gameObject.SetActive(false);
        channelIndex = -1;
    }

    private void ApplyGaussianToRange(List<FrameNode> nodes, int start, int end, Color32 baseColor)
    {
        int length = end - start + 1;
        double sigma = length / 6.0; // 3�ñ׸� ����, ������ 99%�� ���Ե�
        double mean = (length - 1) / 2.0;

        for (int i = 0; i < length; i++)
        {
            double x = i;
            double gauss = System.Math.Exp(-0.5 * System.Math.Pow((x - mean) / sigma, 2));
            // ����ȭ: �߾��� 1, �糡�� 0�� �����
            byte r = (byte)(baseColor.r * gauss);
            byte g = (byte)(baseColor.g * gauss);
            byte b = (byte)(baseColor.b * gauss);
            nodes[start + i].Paint(new Color32(r, g, b, baseColor.a));
        }
    }

    public void OnEventReceived(StartEditArgs args)
    {
        channelIndex = args.channelIndex;

        brush = new Brush();
        palette.Init(args.palette, brush);
        paintArea.EditStart(args.colors, brush);

        gameObject.SetActive(true);
    }
}
