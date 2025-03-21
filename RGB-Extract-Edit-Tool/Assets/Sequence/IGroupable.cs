public interface IGroupable
{
    /// <summary>
    /// �׷쿡���� ����
    /// </summary>
    public class IndividualInfo
    {
        public Group parentGroup;
        public int inIndex; // �׷� �������� index

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
    /// �׷쿡 �߰��غ���
    /// </summary>
    /// <returns></returns>
    public bool TryIncludeNewGroup(IndividualInfo info);

    /// <summary>
    /// �׷쿡 �߰��ϱ�
    /// </summary>
    public void IncludeGroup(IndividualInfo info);

    /// <summary>
    /// �׷쿡 �����ִ� �� Ȯ��
    /// </summary>
    /// <returns></returns>
    public bool IsIncludedGroup();

    /// <summary>
    /// �׷쿡�� �����ϱ� = NonGroup Chanel�� �����
    /// </summary>
    public void ExcludeGroup();
}
