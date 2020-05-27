using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class LevelListTest
    {
        private LevelList _levelList;

        [SetUp]
        public void SetUpTest()
        {
            _levelList = AssetDatabase.LoadAssetAtPath<LevelList>("Assets/Levels/LevelList.asset");
            Assert.IsNotNull(_levelList);
        }

        [Test]
        public void AllLevelsExistAndAreIncludedInBuild()
        {
            var sceneNames = new HashSet<string>();
            //for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
            foreach (var scene in EditorBuildSettings.scenes)
            {
                Assert.IsNotNull(scene);
                Assert.IsTrue(scene.path.StartsWith("Assets/"), "Unexpected scene name: {0}", scene.path);
                Assert.IsTrue(scene.path.EndsWith(".unity"), "Unexpected scene name: {0}", scene.path);
                // Drop leading "Assets/" and the trailing ".unity".
                var sceneName = scene.path.Substring(7, scene.path.Length - 13);
                sceneNames.Add(sceneName);
            }

            Assert.IsNotEmpty(_levelList.Levels);
            foreach(var level in _levelList.Levels)
            {
                Assert.IsNotNull(level);
                Assert.IsNotEmpty(level.LevelName);
                Assert.IsTrue(sceneNames.Contains(level.SceneName));
            }
        }
    }
}
