using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace DeveloperUtils
{
    public static class DebugExtension
    {
        /// <summary>
        /// Extension method for <c>type T</c>.
        /// <code>arg.Debug() = Debug.Log(arg)</code>
        /// </summary>
        /// <param name="arg">String to print in debug mode.</param>
        /// <param name="message">Additional message to print after object is printed</param>
        /// <typeparam name="T">Any type</typeparam>
        public static void Debug<T>(this T arg, string message="")
        {
            UnityEngine.Debug.Log(arg + " " + message);
        }

        /// <summary>
        /// Extension method that converts a list to a string that is shown in debug.
        /// </summary>
        /// <param name="list">a list to be debugged</param>
        /// <param name="sep">a separator delimiting list elements</param>
        /// <typeparam name="T">Any type</typeparam>
        public static void DebugString<T>([NotNull] this List<T> list, string sep = ", ")
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            var str = string.Join(sep, list);
            UnityEngine.Debug.Log(str);
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