using System;
using System.Numerics;
using static IGroupable;

public class Channel : IGroupable
{
    int channelIndex;           // 생성과 삭제에만 관여되는 index
    Vector2 position;

    IndividualInfo individualInfo;

    public class InitInfo
    {
        public int channelIndex;
        public Vector2 position;
    }

    /// <summary>
    /// 생성
    /// </summary>
    /// <param name="initInfo"></param>
    public void Create(InitInfo initInfo)
    {
        channelIndex = initInfo.channelIndex;
    }


    #region Group

    public bool TryIncludeNewGroup(IndividualInfo info)
    {
        if (IsIncludedGroup())
            return false;

        IncludeGroup(info);

        return true;
    }

    public void IncludeGroup(IndividualInfo info)
    {
        individualInfo = info;
    }

    public bool IsIncludedGroup()
    {
        return individualInfo != null;
    }

    public void ExcludeGroup()
    {
        individualInfo = null;
    }

    public void Redefine(IndividualInfo info)
    {
        individualInfo = info;
    }

    public void Redefine(Group parentGroup, int inIndex)
    {
        individualInfo.Redefine(parentGroup, inIndex);
    }

    public void Redefine(int inIndex)
    {
        individualInfo.Redefine(inIndex);
    }

    #endregion

    #region Channel 

    #endregion
}
