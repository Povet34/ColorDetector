using System;
using UnityEngine;

namespace DataExtract
{
    public class Channel : IChannel
    {
        public int channelIndex { get; set; }           // ������ �������� �����Ǵ� index
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

        public void RedefineInGroupInIndex(int inIndex)
        {
            individualInfo.RedefineInGroupInIndex(inIndex);
        }

        #endregion
    }
}