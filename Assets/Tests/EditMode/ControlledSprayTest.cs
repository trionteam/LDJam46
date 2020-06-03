using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class ControlledSprayTest
    {
        private GameObject _controlledCureSprayPrefab;
        private GameObject _controlledVaccineSprayPrefab;
        private GameObject _controlledZombieSprayPrefab;

        [UnitySetUp]
        public IEnumerator SetUpTest()
        {
            yield return new EnterPlayMode();

            var scene = EditorSceneManager.LoadSceneInPlayMode(
                "Tests/EditMode/UnitTestScene", new LoadSceneParameters(LoadSceneMode.Single));
            while (!scene.isLoaded)
            {
                Debug.Log("Waiting for scene to load");
                yield return null;
            }

            _controlledCureSprayPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Sprays.ControlledCureSpray);
            Assert.IsNotNull(_controlledCureSprayPrefab);
            _controlledZombieSprayPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Sprays.ControlledZombieSpray);
            Assert.IsNotNull(_controlledZombieSprayPrefab);
            
            Time.timeScale = 10.0f;
        }

        [UnityTearDown]
        public IEnumerator TearDownTest()
        {
            Time.timeScale = 1.0f;
            yield return new ExitPlayMode();
        }

        /// <summary>
        /// Tests that by default, the spray is activated (i.e. the pressure plate that controls it
        /// is pressed). This method is a generic test that works with all controlled spray prefabs.
        /// </summary>
        /// <param name="prefab">The controlled spray prefab to test with.</param>
        /// <returns>A coroutine that runs the test.</returns>
        private IEnumerator TestActivatedByDefaultWithPrefab(GameObject prefab)
        {
            var spray = GameObject.Instantiate(prefab);
            var controllerZombie = spray.transform.Find("ControllerZombie");
            Assert.IsNotNull(controllerZombie);

            yield return null;

            var pressurePlate = spray.transform.Find("PressurePlate").GetComponent<PressurePlate>();
            Assert.IsNotNull(pressurePlate);
            Assert.IsTrue(pressurePlate.IsPressed);

            for (var startTime = Time.time; Time.time < startTime + 0.5f;)
            {
                yield return null;
            }

            Assert.IsNotEmpty(GameObject.FindObjectsOfType<CloudScript>());
        }

        [UnityTest]
        public IEnumerator CureSprayIsActivatedByDefault()
        {
            return TestActivatedByDefaultWithPrefab(_controlledCureSprayPrefab);
        }

        [UnityTest]
        public IEnumerator ZombieSprayIsActivatedByDefault()
        {
            return TestActivatedByDefaultWithPrefab(_controlledZombieSprayPrefab);
        }

        /// <summary>
        /// Tests that the spray gets deactivated when the controlling zombie is moved away from its
        /// initial position (and it releases the pressure plate). This method is a generic test that
        /// works with all the controlled spray prefabs.
        /// </summary>
        /// <param name="prefab">The controlled spray prefab to test with.</param>
        /// <returns>A coroutine that runs the test.</returns>
        private IEnumerator TestIsNotActiveWhenControllerZombieMovesWithPrefab(GameObject prefab)
        {
            var spray = GameObject.Instantiate(_controlledCureSprayPrefab);
            Assert.IsNotNull(spray, "Could not instantiate the spray");
            var controllerZombie = spray.transform.Find("ControllerZombie");
            // Move the zombie to the side before calling update for the first time. This should
            // be enough to deactivate the pressure plate before the spray starts spraying.
            controllerZombie.transform.position += new Vector3(3.0f, 0.0f, 0.0f);
            Assert.IsNotNull(controllerZombie);

            yield return null;

            var pressurePlateObject = spray.transform.Find("PressurePlate");
            Assert.IsNotNull(pressurePlateObject, "The pressure plate game object was not found");
            var pressurePlate = pressurePlateObject.GetComponent<PressurePlate>();
            Assert.IsNotNull(pressurePlate);
            Assert.IsFalse(pressurePlate.IsPressed);

            for (var startTime = Time.time; Time.time < startTime + 5.0f;)
            {
                yield return null;
            }

            Assert.IsEmpty(GameObject.FindObjectsOfType<CloudScript>());
        }

        [UnityTest]
        public IEnumerator CureSprayIsNotActiveWhenControllerZombieMoves()
        {
            return TestIsNotActiveWhenControllerZombieMovesWithPrefab(_controlledCureSprayPrefab);
        }

        [UnityTest]
        public IEnumerator ZombieSprayIsNotActiveWhenControllerZombieMoves()
        {
            return TestIsNotActiveWhenControllerZombieMovesWithPrefab(_controlledZombieSprayPrefab);
        }
    }
}