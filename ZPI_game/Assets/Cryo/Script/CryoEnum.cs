using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Cryo.Script
{
    public enum EyeDirection
    {
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
    }

    public class EyeTypeHandler
    {
        public static bool IsRotatable(EyeType eyeType)
        {
            switch(eyeType)
            {
                case EyeType.Eye: case EyeType.EyeBig: case EyeType.EyeSmall:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsAsimmetric(EyeType eyeType)
        {
            switch(eyeType)
            {
                case EyeType.Angry: case EyeType.Wink:
                    return true;
                default:
                    return false;
            }
        }
    }

    public enum EyeType
    {
        Eye,
        EyeBig,
        EyeSmall,
        Angry,
        Wink,
        Happy,
        Sad,
        Closed,
        Loading,
    }

    public enum MouthType
    {
        Angry,
        Confused,
        Crying,
        Smile,
        Sad,
        Line,
    }

    public class MouthTypeHandler
    {
        public static bool RequireFlip(MouthType mouthType)
        {
            switch(mouthType)
            {
                case MouthType.Sad:
                    return true;
                default:
                    return false;
            }
        }
    }
}
