public interface IGroupable
{
    /// <summary>
    /// 그룹에서의 정보
    /// </summary>
    public class IndividualInfo
    {
        public Group parentGroup;
        public int inIndex; // 그룹 내에서의 index

        public IndividualInfo(Group parentGroup, int inIndex)
        {
            this.parentGroup = parentGroup;
            this.inIndex = inIndex;
        }

        public IndividualInfo()
        {
            parentGroup = null;
            inIndex = -1;
        }
    }

    /// <summary>
    /// 그룹에 추가해보기
    /// </summary>
    /// <returns></returns>
    public bool TryIncludeNewGroup(IndividualInfo info);

    /// <summary>
    /// 그룹에 추가하기
    /// </summary>
    public void IncludeGroup(IndividualInfo info);

    /// <summary>
    /// 그룹에 속해있는 지 확인
    /// </summary>
    /// <returns></returns>
    public bool IsIncludedGroup();

    /// <summary>
    /// 그룹에서 제외하기 = NonGroup Chanel로 만들기
    /// </summary>
    public void ExcludeGroup();
}
