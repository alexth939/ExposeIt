using ExposeIt.Runtime.Carriers;

namespace ExposeIt.Editor
{
    /// <summary>
    /// Defines a contract for providing dynamically updating textual labels within the Play Mode Zone UI.
    /// </summary>
    /// <remarks>
    /// Implementers supply an <see cref="UpdatableLabelCarrier"/>, which encapsulates
    /// the text retrieval delegate and update interval in milliseconds.<br/>
    /// This enables periodic refresh of label content to reflect changing runtime state or data.
    /// </remarks>
    public interface IPlayModeZoneUpdatableLabelProvider
    {
        UpdatableLabelCarrier ProvideElement();
    }
}
