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
        private GameObject _controlledSprayPrefab;
        private GameObject _managersPrefab;
        private GameObject _uiCanvasPrefab;

        [UnitySetUp]
        public IEnumerator SetUpTest()
        {
            _controlledSprayPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Sprays.ControlledCureSpray);
            Assert.IsNotNull(_controlledSprayPrefab);

            yield return new EnterPlayMode();
            SceneManager.LoadScene("Tests/EditMode/UnitTestScene");
            Time.timeScale = 10.0f;
        }

        [UnityTearDown]
        public IEnumerator TearDownTest()
        {
            Time.timeScale = 1.0f;
            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator IsActivatedByDefault()
        {
            var spray = GameObject.Instantiate(_controlledSprayPrefab);
            var controllerZombie = spray.transform.Find("ControllerZombie");
            Assert.IsNotNull(controllerZombie);

            yield return null;

            var pressurePlate = spray.transform.Find("PressurePlate").GetComponent<PressurePlate>();
            Assert.IsNotNull(pressurePlate);
            Assert.IsTrue(pressurePlate.IsPressed);

            for (var startTime = Time.time; Time.time < startTime + 0.5f; )
            {
                yield return null;
            }

            Assert.IsNotEmpty(GameObject.FindObjectsOfType<CloudScript>());
        }

        [UnityTest]
        public IEnumerator IsNotActiveWhenControllerZombieMoves()
        {
            var spray = GameObject.Instantiate(_controlledSprayPrefab);
            var controllerZombie = spray.transform.Find("ControllerZombie");
            // Move the zombie to the side before calling update for the first time. This should
            // be enough to deactivate the pressure plate before the spray starts spraying.
            controllerZombie.transform.position += new Vector3(3.0f, 0.0f, 0.0f);
            Assert.IsNotNull(controllerZombie);

            yield return null;

            var pressurePlate = spray.transform.Find("PressurePlate").GetComponent<PressurePlate>();
            Assert.IsNotNull(pressurePlate);
            Assert.IsFalse(pressurePlate.IsPressed);

            for (var startTime = Time.time; Time.time < startTime + 5.0f;)
            {
                yield return null;
            }

            Assert.IsEmpty(GameObject.FindObjectsOfType<CloudScript>());
        }
    }
}