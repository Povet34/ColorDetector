using DataExtract;
using UnityEngine;

public interface IChannel
{
    int channelIndex { get; set; }
    Vector2 position { get; set; }

    void Create(CreateChannelParam param);

    /// <summary>
    /// 그룹에서의 정보
    /// </summary>
    public class IndividualInfo
    {
        public IGroup parentGroup;
        public int inIndex; // 그룹 내에서의 index

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
    /// 그룹에 추가해보기
    /// </summary>
    /// <returns></returns>
    bool TryIncludeNewGroup(IndividualInfo info);

    /// <summary>
    /// 그룹에 추가하기
    /// </summary>
    void IncludeGroup(IndividualInfo info);

    /// <summary>
    /// 그룹에 속해있는 지 확인
    /// </summary>
    /// <returns></returns>
    bool IsIncludedGroup();

    /// <summary>
    /// 그룹에서 제외하기 = NonGroup Chanel로 만들기
    /// </summary>
    void ExcludeGroup();

    /// <summary>
    /// 그룹원의 정보를 재정의하기
    /// </summary>
    void RedefineInGroupInIndex(int inIndex);

    IChannel Clone();
}