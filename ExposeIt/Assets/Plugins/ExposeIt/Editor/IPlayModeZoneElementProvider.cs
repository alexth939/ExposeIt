using System;
using UnityEngine.UIElements;

namespace ExposeIt.Editor
{
    /// <summary>
    /// Represents a provider capable of supplying a custom <see cref="VisualElement"/>
    /// to be displayed within the Play Mode Zone.
    /// </summary>
    /// <remarks>
    /// Implementations receive a delegate that returns a default button conforming to
    /// <see cref="INicePredefinedButton"/>.<br/>
    /// Providers may use this default button or
    /// construct entirely custom UI elements, thereby enabling flexible extension
    /// of the Play Mode Zone interface.
    /// </remarks>

    public interface IPlayModeZoneElementProvider
    {
        VisualElement ProvideElement(Func<INicePredefinedButton> defaultButtonProvider);
    }
}
