using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Extensions
{
    public class NotMapNavigableException : Exception
    {
        public NotMapNavigableException(string message = "") : base(message)
        {
            
        }
    }

    public class InvalidMapNavigableName : Exception
    {
        public InvalidMapNavigableName(string message = "") : base(message)
        {
            
        }
    }
    
    public static class SceneExtension
    {
        /// <summary>
        /// Extension method that check if scene is navigable from WorldMap.
        /// </summary>
        /// <param name="scene">scene on which the method is called</param>
        /// <returns>true if scene name has format like "map_SceneName", false otherwise</returns>
        public static bool IsNavigableFromMap(this Scene scene)
        {
            return scene.name.StartsWith("map");
        }

        /// <summary>
        /// Extension methods for Unity <c>Scene</c> that returns the clear name of a scene.
        /// </summary>
        /// <param name="scene">scene on which the method is called</param>
        /// <returns>the clear name of the scene</returns>
        /// <exception cref="NotMapNavigableException">thrown when map is not navigable from the WorldMap. Navigable maps names should have format like "map_ClearName"</exception>
        /// <exception cref="InvalidMapNavigableName">thrown when scene has an invalid name. Navigable maps names should have format like "map_ClearName"</exception>
        public static string GetClearName(this Scene scene)
        {
            if (!scene.IsNavigableFromMap())
                throw new NotMapNavigableException($"Scene {scene.name} is not navigable from world map.");
            var splitName = scene.name.Split('_');
            if (splitName.Length < 3)
                throw new InvalidMapNavigableName($"Scene {scene.name} has an invalid name " +
                                                  $"structure for a map navigable scene. Correct " +
                                                  $"structure should look like: " +
                                                  $"\"map_1_SceneName\"");
            return splitName[2];
        }
        
        /// <summary>
        /// Extension method that returns the number of menu navigable scene
        /// </summary>
        /// <param name="scene">Scene on which the method is called</param>
        /// <returns>Scene number</returns>
        /// <exception cref="NotMapNavigableException">thrown when map is not navigable from the WorldMap. Navigable maps names should have format like "map_ClearName"</exception>
        /// <exception cref="InvalidMapNavigableName">thrown when scene has an invalid name. Navigable maps names should have format like "map_ClearName"</exception>
        public static int GetSceneNumber(this Scene scene)
        {
            if (!scene.IsNavigableFromMap())
                throw new NotMapNavigableException($"Scene {scene.name} is not navigable from world map.");
            var split = scene.name.Split('_');
            if (split.Length < 3)
                throw new InvalidMapNavigableName($"Scene {scene.name} has an invalid name " +
                                                  $"structure for a map navigable scene. Correct " +
                                                  $"structure should look like: " +
                                                  $"\"map_1_SceneName\"");
            return int.Parse(split[1]);
        }

        /// <summary>
        /// Extension method that returns the list of previous levels.
        /// </summary>
        /// <param name="scene">scene on which the method is called</param>
        /// <returns>a list of previous levels</returns>
        /// <exception cref="NotMapNavigableException">thrown when map is not navigable from the WorldMap. Navigable maps names should have format like "map_ClearName"</exception>
        /// <exception cref="InvalidMapNavigableName">thrown when scene has an invalid name. Navigable maps names should have format like "map_ClearName"</exception>
        public static List<int> GetPreviousLevels(this Scene scene)
        {
            if (!scene.IsNavigableFromMap())
                throw new NotMapNavigableException($"Scene {scene.name} is not navigable from world map.");
            var split = scene.name.Split('_');
            if (split.Length < 3)
                throw new InvalidMapNavigableName($"Scene {scene.name} has an invalid name " +
                                                  $"structure for a map navigable scene. Correct " +
                                                  $"structure should look like: " +
                                                  $"\"map_1_SceneName\"");
            if (split.Length == 2) return new List<int>();
            return split.Skip(3).Select(int.Parse).ToList();
        }
    
        /// <summary>
        /// A function that returns the proper map navigable scene name.
        /// </summary>
        /// <param name="clearName">the clear name of the scene</param>
        /// <param name="number">scene number</param>
        /// <param name="previousLevels">an enumerable of previous scenes</param>
        /// <returns>a name of a map navigable scene in format "map_n0_ClearName[_p0*]</returns>
        /// <example>
        /// Name "<c>map_4_YeetOfTheCliff_1_3</c>" means that the scene name is YeetOfTheCliff, scene number is 4
        /// and the previous levels are levels 3 and 1.
        /// </example>
        public static string MakeSceneName(string clearName, int number, IEnumerable<int> previousLevels)
        {
            string previousLevelsChain = string.Join('_', previousLevels);
            var ret = $"map_{number}_{clearName}";
            if (previousLevelsChain.Length != 0)
            {
                ret += $"_{previousLevelsChain}";
            }
            return ret;
        }
    }
}