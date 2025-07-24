using System;
using UnityEngine.UIElements;

namespace ExposeIt.Editor
{
    /// <summary>
    /// Defines a contract for a customizable UI button with predefined behavior and appearance
    /// intended for use within the Play Mode Zone or similar editor extensions.
    /// </summary>
    /// <remarks>
    /// This interface exposes events for pointer interactions, such as clicks and pointer down,<br/>
    /// allowing clients to respond to user input.<br/>
    /// It also provides methods to set the button's displayed text and tooltip,<br/>
    /// and a method to retrieve the underlying <see cref="VisualElement"/> for integration
    /// within UI Toolkit visual hierarchies.
    /// </remarks>

    public interface INicePredefinedButton
    {
        public event Action PointerClicked;

        public event Action PointerDown;

        VisualElement AsVisualElement();

        public void SetText(string text);

        public void SetTooltip(string tooltip);
    }
}
