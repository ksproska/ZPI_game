using System.Collections.Generic;
using UnityEngine;

namespace DeveloperUtils
{
    public static class DebugExtension
    {
        /// <summary>
        /// Extension method for <c>type string</c>.
        /// <code>arg.Debug() = Debug.Log(arg)</code>
        /// </summary>
        /// <param name="arg">String to print in debug mode.</param>
        public static void Debug(this string arg)
        {
            UnityEngine.Debug.Log(arg);
        }

        /// <summary>
        /// Extension method for <c>type GameObject</c>.
        /// <code>obj.Debug() = Debug.Log(obj)</code>
        /// </summary>
        /// <param name="obj">GameObject to print in debug mode.</param>
        public static void Debug(this GameObject obj)
        {
            UnityEngine.Debug.Log(obj);
        }

        /// <summary>
        /// Extension method that converts a list to a string that is shown in debug.
        /// </summary>
        /// <param name="list">A list to be debugged</param>
        /// <param name="sep">A separator delimiting list elements</param>
        /// <typeparam name="T">Any type</typeparam>
        public static void DebugString<T>(this List<T> list, string sep = ", ")
        {
            var str = "";
            list.ForEach(elem => str += elem + sep);
            UnityEngine.Debug.Log(str.Substring(0, str.Length - sep.Length));
        }

        /// <summary>
        /// Extension method that debugs each element of the given list.
        /// </summary>
        /// <param name="list">A list to be debugged</param>
        /// <typeparam name="T">Any type</typeparam>
        public static void DebugLoop<T>(this List<T> list)
        {
            list.ForEach(elem => UnityEngine.Debug.Log(elem));
        }
    }
}