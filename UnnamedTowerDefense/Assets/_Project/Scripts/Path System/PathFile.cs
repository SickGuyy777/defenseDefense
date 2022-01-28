using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Scripts.Path_System
{
    public class PathFile : ScriptableObject
    {
        public const string DefaultPathFilePath = "Assets/_Project/Scripts/Path System/_Paths/";
        
        [TextArea(30, 80)]
        public string data;

        /// <summary>
        /// Create an instance of <see cref="PathFile"/>.
        /// </summary>
        /// <returns></returns>
        [MenuItem("Assets/Create/PathFile")]
        public static PathFile Create() => Create(DefaultPathFilePath, "Unnamed Path");

        /// <summary>
        /// Create an instance of <see cref="PathFile"/>.
        /// </summary>
        /// <param name="path">The path of the FilePath in the files.</param>
        /// <param name="name">The name of the file.</param>
        /// <returns></returns>
        public static PathFile Create(string path, string name)
        {
            // Assertions
            Assert.AreNotEqual(0, path.Length, "Length of path is empty");
            Assert.AreNotEqual(0, name.Length, "Name of path is empty");
            
            // Add a forward slash in the beginning of the name
            if (name[0] != '/')
                name = '/' + name; 

            // Add the .assets extension
            if (name.Length <= 6 || name[^6..] != ".asset")
                name += ".asset";

            var result = CreateInstance<PathFile>();
            AssetDatabase.CreateAsset(result, path + name);

            return result;
        }
    }
}