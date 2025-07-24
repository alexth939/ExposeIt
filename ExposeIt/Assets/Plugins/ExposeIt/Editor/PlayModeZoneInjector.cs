using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ExposeIt.Editor
{
    /// <summary>
    /// Responsible for discovering, creating, and injecting UI elements into the Unity Editor's Play Mode Zone toolbar.
    /// <para>
    /// This class automatically initializes on editor load (Unity 2022.1 or newer) and subscribes to runtime events
    /// for adding, replacing, or removing buttons dynamically in the Play Mode Zone.
    /// </para>
    /// <para>
    /// It locates the Play Mode Zone within Unity's internal toolbar hierarchy via reflection,
    /// and uses <see cref="PlayModeZoneElementProviderAttribute"/>-decorated classes to instantiate UI elements.<br/>
    /// These providers may supply either standard elements or dynamically updating labels.
    /// </para>
    /// <para>
    /// The injector manages element lifecycle and ensures buttons are added in a sorted order defined by the provider's
    /// <c>Order</c> property.<br/>
    /// It also provides callback handlers for client requests to add, replace, or remove buttons
    /// during runtime, encapsulating standard interaction patterns.
    /// </para>
    /// <remarks>
    /// - Utilizes reflection to access internal UnityEditor types and visual tree elements.<br/>
    /// - Assumes presence of a toolbar element named <c>ToolbarZonePlayMode</c> where elements are injected.<br/>
    /// - Uses the <see cref="INicePredefinedButton"/> abstraction to create standard buttons when needed.<br/>
    /// - Designed to be extensible via new element providers without modifying core injector code.
    /// </remarks>
    /// </summary>
#if UNITY_2022_1_OR_NEWER
    public class PlayModeZoneInjector
    {
        private const string PlayZoneElementName = "ToolbarZonePlayMode";

        [InitializeOnLoadMethod()]
        public static void DelayRunInjector()
        {
            Runtime.PlayModeZone.ClientRequestedToAddFunction += OnExposeButtonRequested;
            Runtime.PlayModeZone.ClientRequestedToAddOrReplaceFunction += OnExposeRewriteButtonRequested;
            Runtime.PlayModeZone.ClientRequestedToRemoveFunction += OnRemoveButtonRequested;

            Debug.Log($"preparing to inject playmode zone elements.");
            EditorApplication.delayCall += RunInjector;
        }

        public static void RunInjector()
        {
            TypeCache.TypeCollection providerTypes = TypeCache.GetTypesWithAttribute<PlayModeZoneElementProviderAttribute>();
            VisualElement playModeZone = GetPlayModeZone();

            var sortedProviders = providerTypes
                .Select(providerType =>
                {
                    var attribute = providerType.GetCustomAttribute<PlayModeZoneElementProviderAttribute>();

                    return new
                    {
                        Type = providerType,
                        Order = attribute?.Order ?? 0
                    };
                })
                .OrderBy(provider => provider.Order);

            foreach(var provider in sortedProviders)
            {
                var providerInstanceObject = Activator.CreateInstance(provider.Type);

                VisualElement element = null;

                if(providerInstanceObject is IPlayModeZoneElementProvider providerInstance)
                {
                    element = providerInstance.ProvideElement(CreatePredefinedButton);
                }
                else if(providerInstanceObject is IPlayModeZoneUpdatableLabelProvider updatableLabel)
                {
                    element = CreatePredefinedButton().AsVisualElement();
                    var box = updatableLabel.ProvideElement();
                    element.schedule.Execute(() =>
                    {
                        (element as INicePredefinedButton).SetText(box.TextGetter.Invoke());
                    }).Every(box.UpdateIntervalInMS);
                }

                playModeZone.Add(element);
            }
        }

        private static INicePredefinedButton CreatePredefinedButton() => new PredefinedPlayZoneButton();

        public static void OnRemoveButtonRequested(string buttonName)
        {
            VisualElement zone = GetPlayModeZone();
            VisualElement button = zone.Q(buttonName);

            if(button is null)
            {
                Debug.LogWarning(
                    $"ExposeIt: Tried to remove a button named [{buttonName}], but it was not found in the PlayMode zone.");

                return;
            }

            zone.Remove(button);
        }

        private static void OnExposeRewriteButtonRequested(string buttonName, Action action)
        {
            VisualElement zone = GetPlayModeZone();
            VisualElement button = zone.Q(buttonName);

            bool isButtonAlreadyExist = button != null;

            if(isButtonAlreadyExist)
                zone.Remove(button);

            INicePredefinedButton element = new PredefinedPlayZoneButton();
            element.SetText(buttonName);
            element.PointerClicked += action;

            zone.Add(element.AsVisualElement());
        }

        private static void OnExposeButtonRequested(string buttonName, Action action)
        {
            VisualElement zone = GetPlayModeZone();
            VisualElement button = zone.Q(buttonName);

            bool isButtonAlreadyExist = button != null;

            if(isButtonAlreadyExist)
                return;

            INicePredefinedButton element = new PredefinedPlayZoneButton();
            element.SetText(buttonName);
            element.PointerClicked += action;

            zone.Add(element.AsVisualElement());
        }

        public static VisualElement GetPlayModeZone()
        {
            var toolbarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.Toolbar");
            var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);

            if(toolbars == null)
                throw new Exception("failed to retrieve any toolbars.");

            if(toolbars.Length == 0)
                throw new Exception("failed to retrieve any toolbars.");

            foreach(var toolbar in toolbars)
            {
                FieldInfo rootField = toolbarType.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                VisualElement root = rootField?.GetValue(toolbar) as VisualElement;

                if(root == null)
                {
                    Debug.Log($"root was null");
                    continue;
                }

                // Find the specific container where Play/Pause/Step live
                var playModeZone = root.Q(PlayZoneElementName);

                if(playModeZone == null)
                {
                    Debug.Log($"playModeZone was null");
                    continue;
                }
                else
                {
                    return playModeZone;
                }
            }

            throw default;
        }
    }
}
#endif
