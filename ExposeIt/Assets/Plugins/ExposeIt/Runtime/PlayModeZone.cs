using System;

namespace ExposeIt.Runtime
{
    /// <summary>
    /// Provides a centralized API for clients to register, update, and remove named functions
    /// in the Play Mode Zone.<br/>
    /// These functions typically correspond to UI elements or commands
    /// exposed during play mode within the Unity Editor.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="PlayModeZone"/> class exposes static methods to add, replace, or remove
    /// functions by name, triggering internal events to notify the system of client requests.
    /// </para>
    /// <para>
    /// All events are internal, ensuring controlled access and modification via the provided
    /// public static methods.<br/>
    /// This design allows the Play Mode Zone infrastructure to
    /// dynamically respond to changes in registered functions and update the UI accordingly.
    /// </para>
    /// </remarks>

    public static class PlayModeZone
    {
        internal static event CreateFunctionDelegate ClientRequestedToAddFunction;
        internal static event CreateFunctionDelegate ClientRequestedToAddOrReplaceFunction;
        internal static event DisposeFunctionDelegate ClientRequestedToRemoveFunction;

        public static void AddFunction(string functionName, Action action)
        {
            ClientRequestedToAddFunction?.Invoke(functionName, action);
        }

        public static void AddOrReplaceFunction(string functionName, Action action)
        {
            ClientRequestedToAddOrReplaceFunction?.Invoke(functionName, action);
        }

        public static void RemoveFunction(string functionName)
        {
            ClientRequestedToRemoveFunction?.Invoke(functionName);
        }
    }
}
