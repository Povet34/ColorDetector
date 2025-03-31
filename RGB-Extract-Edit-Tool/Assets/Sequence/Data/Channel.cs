using System;
using UnityEngine;

namespace DataExtract
{
    public class Channel : IChannel
    {
        public int channelIndex { get; set; }           // 생성과 삭제에만 관여되는 index
        public Vector2 position { get; set; }
        public IChannel.IndividualInfo individualInfo { get; set; }

        public void Create(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;
        }

        public IChannel Clone()
        {
            return new Channel
            {
                channelIndex = this.channelIndex,
                position = this.position,
                individualInfo = this.individualInfo != null ? new IChannel.IndividualInfo(this.individualInfo.parentGroup, this.individualInfo.inIndex) : null
            };
        }

        #region Group

        public bool TryIncludeNewGroup(IChannel.IndividualInfo info)
        {
            if (IsIncludedGroup())
                return false;

            IncludeGroup(info);

            return true;
        }

        public void IncludeGroup(IChannel.IndividualInfo info)
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

        public void Redefine(IChannel.IndividualInfo info)
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
    }
}