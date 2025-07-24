using System;
using ExposeIt.Editor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ExposeIt.Examples
{
    /// <summary>
    /// Demonstrates how to use the default button provider to create a simple, customized toolbar button
    /// for the play mode zone.<br/>
    /// This example modifies the button's label, tooltip, and behavior by subscribing
    /// to pointer events provided by the <see cref="INicePredefinedButton"/> interface.
    /// <para>
    /// This approach leverages the predefined visual styling and integration provided by the framework,<br/>
    /// making it a convenient and safe option for basic UI additions without having to manually construct
    /// and style toolbar elements.
    /// </para>
    /// <para>
    /// Recommended when only minor customizations are needed, such as changing the label, tooltip, or
    /// responding to pointer interactions.
    /// </para>
    /// </summary>

    //[PlayModeZoneElementProvider(-1)]
    public class PlayModeZoneExample1 : IPlayModeZoneElementProvider
    {
        public VisualElement ProvideElement(Func<INicePredefinedButton> defaultButtonProvider)
        {
            var button = defaultButtonProvider.Invoke();
            button.SetText("haha button 2");
            button.PointerDown += () => Debug.Log($"Nice to have this capability.");
            button.PointerClicked += () => Debug.Log($"Frequent event they will use.");
            button.SetTooltip("buya");

            return button.AsVisualElement();
        }
    }
}
