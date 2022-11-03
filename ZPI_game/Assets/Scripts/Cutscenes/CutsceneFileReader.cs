using Assets.Cryo.Script;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes
{
    public class CutsceneFileReader
    {

        public static List<string> ReadLines(string allText)
        {
            return new List<string>(allText.Split('\n'));
        }

        public static List<string> GetTextSequence(string allText, string textSeparator = "===")
        {
            var ret = new List<string>();
            var lines = allText.Split('\n');
            var temp = "";
            foreach (var line in lines)
            {
                if (!line.Trim().Equals(textSeparator))
                {
                    temp += line + "\n";
                }
                else
                {
                    ret.Add(temp);
                    temp = "";
                }
            }
            return ret;
        }
    }

    public class CutsceneLineParser
    {
        private enum EyeSide
        {
            Both,
            Left,
            Right,
        }

        public static string WhoSays(string line)
        {
            return line.Split('|')[0];
        }

        public static string GetCharacterLine(string line)
        {
            var split = line.Split('|');
            if (split.Length < 1) return null;
            return split[split.Length - 1];
        }
        public static void SetupCryo(ref CryoUI cryo, string line)
        {
            // Example Cryo line:    C|B:Happy,Smile|some other text
            var (whichEye, eyeTypeStr, mouthTypeStr) = GetCryoArgs(line);

            if (mouthTypeStr != null && mouthTypeStr != "")
            {
                var mouthType = GetCryoEnumType<MouthType>(mouthTypeStr);
                cryo.SetMouthType(mouthType);
            }

            EyeType? eyeType = null;
            if(eyeTypeStr != null && eyeTypeStr != "")
            {
                eyeType = GetCryoEnumType<EyeType>(eyeTypeStr);
            }
            if (whichEye == null || whichEye == "") return;
            var eyeDirection = GetEyeSide(whichEye);
            if (!eyeType.HasValue) return;

            switch(eyeDirection)
            {
                case EyeSide.Both:
                    cryo.SetBothEyesTypes(eyeType.Value);
                    break;
                case EyeSide.Left:
                    cryo.SetLeftEyeType(eyeType.Value);
                    break;
                case EyeSide.Right:
                    cryo.SetRightEyeType(eyeType.Value);
                    break;
            }
        }

        private static (string, string, string) GetCryoArgs(string line)
        {
            // Example Cryo line:    C|B:Happy,Smile|some other text
            var split = line.Split('|');
            if (split.Length <= 2) return (null, null, null);
            var args = split[1];
            var parameters = args.Split(',');
            if (parameters.Length != 2) return (null, null, null);
            var eyeParams = parameters[0].Split(':');
            if (eyeParams.Length != 2) return (null, null, parameters[1]);
            return (eyeParams[0], eyeParams[1], parameters[1]);
        }

        private static T GetCryoEnumType<T>(string arg)
        {
            return (T)Enum.Parse(typeof(T), arg);
        }

        private static EyeSide GetEyeSide(string arg)
        {
            switch (arg.ToLower())
            {
                case "l":
                    return EyeSide.Left;
                case "r":
                    return EyeSide.Right;
                default:
                    return EyeSide.Both;
            }
        }
    }
}