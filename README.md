# ExposeIt

# ExposeIt

ExposeIt is a small Unity editor plugin that lets you expose *anything* into the **Play Mode toolbar** area — neatly wrapped in predefined or custom UI Toolkit elements.

No more cluttered context menus, no more temporary custom editors or nested inspectors. Just clean, instantly accessible buttons and fields right where you need them — in plain sight during Play Mode.

## ✅ Minimum Unity Version

**Unity 2022.3 LTS**

The plugin relies on UI Toolkit-based play mode controls, which Unity adopted in this version. Tested with versions up to `6000.0.36f1`.

## 🖥️ Supported Platforms

- ✅ Unity Editor (main target)
- ❌ Runtime support not included (planned for future updates)

## 📦 Installation

1. Download the latest `.unitypackage` from the [Releases](https://github.com/alexth939/ExposeIt/releases) tab.
2. Import it into your project via Unity’s `Assets > Import Package > Custom Package...` menu.

## 🧪 Usage (Editor)

> Features available only within the Unity Editor.

<!-- You fill this in -->

### <u>🔘 Static Element</u>

![](C:\Repos\ExposeIt\Docs\Images\static-button.png)

You can declare a static element provider, which will be automatically discovered and injected into the Play Mode zone:

```csharp
[PlayModeZoneElementProvider()]
public class CustomButtonProvider : IPlayModeZoneElementProvider
```

The `IPlayModeZoneElementProvider` interface allows you to choose between predefined elements or a fully custom `VisualElement` implementation.



> ⚠️ Note: Buttons do not persist across domain reloads, but will be re-injected automatically on `[InitializeOnLoadMethod]`.

> 💡 See examples for usage.

---

### <u>🏷️ Updatable Label</u>

![](C:\Repos\ExposeIt\Docs\Images\updatable-label.png)

This predefined element lets you display dynamic text that updates at a defined interval:

```csharp
[PlayModeZoneElementProvider()]
public class PlayModeZoneExample4 : IPlayModeZoneUpdatableLabelProvider
```

> 💡 See examples for usage.

---

## 🚀 Usage (Runtime-Compatible)

> Features available in both Editor and Runtime contexts.

### 🎛️ Manually Managed Button</u>

You can add buttons to the Play Mode zone programmatically — useful for dynamic testing scenarios:

```csharp
PlayModeZone.AddFunction("TestMe 0", () => Debug.Log($"testing method executed"));

PlayModeZone.AddOrReplaceFunction("TestMe 0", () => Debug.Log($"new impl on same button"));

PlayModeZone.RemoveFunction("TestMe 0");
```

> ⚠️ Note: You can't pass custom visual elements through this API — only simple hooks.

> 💡 See examples for usage.