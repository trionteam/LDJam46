using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestPressurePlate
    {
        private GameObject cloudPrefab;
        private GameObject cowPrefab;
        private GameObject pressurePlatePrefab;
        private GameObject zombiePrefab;

        [SetUp]
        public void SetUpTest()
        {
            cloudPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Sprays.Cloud);
            Assert.IsNotNull(cloudPrefab);
            cowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Characters.Cow);
            Assert.IsNotNull(cowPrefab);
            pressurePlatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Triggers.PressurePlate);
            Assert.IsNotNull(pressurePlatePrefab);
            zombiePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(Prefabs.Characters.Zombie);
            Assert.IsNotNull(zombiePrefab);
        }

        /// <summary>
        /// Tests the pressure plate with the given prefab. Instantiates a pressure plate at
        /// Vector3.zero, and the given prefab at a given position (Vector3.zero by default),
        /// and checks whether the pressure plate is considered to be pressed.
        /// </summary>
        /// <param name="prefab">The prefab used to test the trigger.</param>
        /// <param name="isExpectedToTrigger"><c>true</c> if the prefab at the given position
        /// is expected to trigger the pressure plate.</param>
        /// <param name="prefabPosition">The position of the prefab. Vector3.zero by default.
        /// </param>
        /// <returns>A Unity coroutine that runs the test.</returns>
        private IEnumerator TestWithPrefab(GameObject prefab,
                                           bool isExpectedToTrigger,
                                           Vector3 prefabPosition = new Vector3())
        {
            yield return new EnterPlayMode();

            // Instantiate a pressure plate.
            var pressurePlate = GameObject.Instantiate(pressurePlatePrefab).GetComponent<PressurePlate>();
            Assert.IsNotNull(pressurePlate);
            // The pressure plate should be unpressed (there is nothing to press it).
            Assert.IsFalse(pressurePlate.IsPressed);

            // Instantiate a triggering object at more or less the same position.
            var triggeringObject = GameObject.Instantiate(prefab, prefabPosition, Quaternion.identity);
            Assert.IsNotNull(triggeringObject);
            yield return null;

            // In the next frames, the pressure plate should be pressed.
            for (int i = 0; i < 10; ++i)
            {
                Assert.AreEqual(isExpectedToTrigger, pressurePlate.IsPressed);
                yield return null;
            }

            // Move the triggering object far far away.
            triggeringObject.transform.position = new Vector3(100.0f, 0.0f, 0.0f);
            yield return null;

            // In the next frame, the pressure plate should be depressed.
            Assert.IsFalse(pressurePlate.IsPressed);

            GameObject.Destroy(pressurePlate);
            GameObject.Destroy(triggeringObject);

            yield return new ExitPlayMode();
        }

        /// <summary>
        /// Tests that a cow standing over the pressure plate triggers it.
        /// </summary>
        [UnityTest]
        public IEnumerator RespondsToCows()
        {
            return TestWithPrefab(cowPrefab, true);
        }

        /// <summary>
        /// Tests that a zombie standing over the pressure plate triggers it.
        /// </summary>
        [UnityTest]
        public IEnumerator RespondsToZombies()
        {
            return TestWithPrefab(zombiePrefab, true);
        }

        /// <summary>
        /// Tests that a zombie standing next to the pressure plate (with other
        /// colliders overlapping it) does not trigger the plate.
        /// </summary>
        [UnityTest]
        public IEnumerator DoesNotRespondToZombieDetection()
        {
            return TestWithPrefab(zombiePrefab, false, new Vector3(0.0f, -0.265f, 0.0f));
        }

        /// <summary>
        /// Tests that a cloud flying over the plate does not trigger it.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator DoesNotRespondToCloud()
        {
            return TestWithPrefab(cloudPrefab, false);
        }
    }
}
