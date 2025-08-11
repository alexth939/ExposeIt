using System;
using ExposeIt.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ExposeIt.Examples
{
    /// <summary>
    /// Demonstrates how to implement a custom play mode zone element provider by completely overriding<br/>
    /// the default behavior and supplying your own UI logic and elements.<br/>
    /// This example dynamically changes the buttons displayed depending on whether the Editor is in Edit or Play mode,<br/>
    /// showcasing how to respond to play mode state transitions.
    /// <para>
    /// Note: While it is possible to ignore the provided <c>defaultButtonProvider</c> and create your
    /// own <see cref="VisualElement"/> instances,<br/>
    /// caution is advised when using predefined toolbar components
    /// such as <see cref="UnityEditor.Toolbars.EditorToolbarButton"/>.<br/>
    /// These elements may appear without expected styling or visual feedback,<br/>
    /// particularly borders or backgrounds, due to quirks or limitations
    /// in Unity's internal styling system.
    /// </para>
    /// <para>
    /// In such cases, it may be necessary to wrap the toolbar elements inside additional containers<br/>
    /// (e.g., <see cref="VisualElement"/>) and apply custom styling (e.g., borders, margins, background colors)
    /// manually to restore acceptable appearance and usability.
    /// </para>
    /// </summary>
    //[PlayModeZoneElementProvider(0)]
    public class PlayModeZoneExample2 : IPlayModeZoneElementProvider
    {
        private readonly VisualElement _root = new();

        public VisualElement ProvideElement(Func<INicePredefinedButton> defaultButtonProvider)
        {
            _root.style.flexDirection = FlexDirection.Row;

            EditorApplication.playModeStateChanged += OnPlayModeChanged;
            RefreshButtons(EditorApplication.isPlaying);

            return _root;
        }

        private void OnPlayModeChanged(PlayModeStateChange change)
        {
            if(change == PlayModeStateChange.EnteredEditMode)
                RefreshButtons(false);
            else if(change == PlayModeStateChange.EnteredPlayMode)
                RefreshButtons(true);
        }

        private void RefreshButtons(bool isPlayMode)
        {
            _root.Clear();

            int count = 0;

            void AddButton(string label, Action onClick)
            {
                var btn = new UnityEditor.Toolbars.EditorToolbarButton(label, onClick);

                // Wrap the button to add borders
                var wrapper = new VisualElement();
                
                wrapper.style.borderTopLeftRadius = 4;
                wrapper.style.borderTopRightRadius = 4;
                wrapper.style.borderBottomLeftRadius = 4;
                wrapper.style.borderBottomRightRadius = 4;
                wrapper.style.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1f); // light gray

                wrapper.Add(btn);

                if(count > 0)
                    wrapper.style.marginLeft = 4;

                _root.Add(wrapper);
                count++;
            }

            if(isPlayMode)
            {
                AddButton("Play A", () => Debug.Log("Clicked Play A"));
                AddButton("Play B", () => Debug.Log("Clicked Play B"));
            }
            else
            {
                AddButton("Edit X", () => Debug.Log("Clicked Edit X"));
                AddButton("Edit Y", () => Debug.Log("Clicked Edit Y"));
            }
        }
    }
}
