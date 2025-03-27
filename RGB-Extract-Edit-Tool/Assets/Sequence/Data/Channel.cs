using System;
using UnityEngine;
using static IGroupable;

namespace DataExtract
{
    public class Channel : IGroupable, IChannel
    {
        public int channelIndex { get; set; }           // 생성과 삭제에만 관여되는 index
        public Vector2 position { get; set; }
        public IndividualInfo individualInfo { get; set; }

        public void Create(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;
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
}