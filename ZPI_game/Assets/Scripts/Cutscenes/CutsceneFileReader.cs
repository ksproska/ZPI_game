using System.Collections.Generic;
using UnityEngine;

namespace Cutscenes
{
    public class CutsceneFileReader
    {
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
}