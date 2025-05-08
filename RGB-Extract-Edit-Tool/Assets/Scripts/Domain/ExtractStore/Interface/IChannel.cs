using DataExtract;
using UnityEngine;

public interface IChannel
{
    int channelIndex { get; set; }
    Vector2 position { get; set; }

    void Create(CreateChannelParam param);

    /// <summary>
    /// �׷쿡���� ����
    /// </summary>
    public class IndividualInfo
    {
        public IGroup parentGroup;
        public int inIndex; // �׷� �������� index

        public IndividualInfo(IGroup parentGroup, int inIndex)
        {
            this.parentGroup = parentGroup;
            this.inIndex = inIndex;
        }

        public IndividualInfo()
        {
            parentGroup = null;
            inIndex = -1;
        }

        public void RedefineInGroupInIndex(int inIndex)
        {
            this.inIndex = inIndex;
        }

        public void Redefine(IGroup parentGroup, int inIndex)
        {
            this.parentGroup = parentGroup;
            this.inIndex = inIndex;
        }
    }

    IndividualInfo individualInfo { get; set; }

    /// <summary>
    /// �׷쿡 �߰��غ���
    /// </summary>
    /// <returns></returns>
    bool TryIncludeNewGroup(IndividualInfo info);

    /// <summary>
    /// �׷쿡 �߰��ϱ�
    /// </summary>
    void IncludeGroup(IndividualInfo info);

    /// <summary>
    /// �׷쿡 �����ִ� �� Ȯ��
    /// </summary>
    /// <returns></returns>
    bool IsIncludedGroup();

    /// <summary>
    /// �׷쿡�� �����ϱ� = NonGroup Chanel�� �����
    /// </summary>
    void ExcludeGroup();

    /// <summary>
    /// �׷���� ������ �������ϱ�
    /// </summary>
    void RedefineInGroupInIndex(int inIndex);

    IChannel Clone();
}