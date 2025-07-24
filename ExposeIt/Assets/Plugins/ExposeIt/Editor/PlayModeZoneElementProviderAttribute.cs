using System;

namespace ExposeIt.Editor
{
    /// <summary>
    /// Declares a class as a provider of UI elements for the Play Mode Zone in the Unity Editor.
    /// </summary>
    /// <remarks>
    /// This attribute should be applied to classes that implement element provider interfaces recognized
    /// by the Play Mode Zone system.<br/>
    /// The system will automatically discover and initialize such providers
    /// at editor-time or play-time, depending on their context.
    ///<para/>
    /// The <see cref="Order"/> property controls the relative positioning of UI elements contributed
    /// by multiple providers.<br/>
    /// Providers with lower order values are initialized and displayed earlier.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class PlayModeZoneElementProviderAttribute : Attribute
    {
        public int Order { get; }

        public PlayModeZoneElementProviderAttribute(int order = 0)
        {
            Order = order;
        }
    }
}
