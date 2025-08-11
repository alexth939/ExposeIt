using ExposeIt.Runtime;
using UnityEngine;

namespace ExposeIt.Examples
{
    /// <summary>
    /// Demonstrates how to dynamically register and remove developer functions
    /// in the Play Mode Zone for quick testing and debugging.
    /// <para>
    /// This example sets up a temporary "backdoor" function during runtime initialization,<br/>
    /// then replaces or updates the registered function multiple times using a coroutine,<br/>
    /// simulating dynamic command updates in real time.
    /// </para>
    /// <para>
    /// Functions are removed upon destruction to avoid stale references and side effects
    /// across play mode sessions.
    /// </para>
    /// <remarks>
    /// - Useful during development to expose test hooks, debug tools, or quick execution points.<br/>
    /// - Function names must be unique and are case-sensitive.<br/>
    /// - Removing a non-existent function will raise a warning but is safe.
    /// </remarks>
    /// </summary>

    public class PlayModeZoneExample3 : MonoBehaviour
    {
        private static void MakeABackdoorForEasyTesting()
        {
            Debug.Log($"setting backdoor");
            PlayModeZone.AddFunction("TestMe 0", () => Debug.Log($"testing method executed"));
        }

        //[RuntimeInitializeOnLoadMethod]
        private static void RunTest()
        {
            GameObject a = new GameObject("a", typeof(PlayModeZoneExample3));
        }

        private void Awake()
        {
            MakeABackdoorForEasyTesting();
            StartCoroutine(Test());
        }

        private System.Collections.IEnumerator Test()
        {
            yield return new WaitForSeconds(1.0f);
            PlayModeZone.AddOrReplaceFunction("TestMe 1", () => Debug.Log($"test 1"));
            yield return new WaitForSeconds(1.0f);
            PlayModeZone.AddOrReplaceFunction("TestMe 1", () => Debug.Log($"test 1"));
            yield return new WaitForSeconds(1.0f);
            PlayModeZone.AddOrReplaceFunction("TestMe 1", () => Debug.Log($"test 1"));
        }

        private void OnDestroy()
        {
            RemoveBackdoor();
        }

        private void RemoveBackdoor()
        {
            Debug.Log($"removing backdoor");
            PlayModeZone.RemoveFunction("TestMe 0");
            PlayModeZone.RemoveFunction("TestMe 1");

            // This will raise a warning since testme2 wont be found.
            PlayModeZone.RemoveFunction("TestMe 2");
        }
    }
}
