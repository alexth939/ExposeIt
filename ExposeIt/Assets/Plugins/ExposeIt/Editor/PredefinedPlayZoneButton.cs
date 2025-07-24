using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ExposeIt.Editor
{
    /// <summary>
    /// A custom implementation of <see cref="INicePredefinedButton"/> providing a styled, interactive toolbar button
    /// for the Play Mode Zone UI within the Unity Editor.
    /// </summary>
    /// <remarks>
    /// This button visually consists of a container with configurable background and border styling,
    /// and a centered, bold white label.<br/>
    /// It supports hover color changes, click and pointer down events,
    /// and exposes methods to set its text and tooltip.
    /// <para>
    /// The class manages mouse and pointer events internally, invoking <see cref="PointerClicked"/> and <see cref="PointerDown"/>
    /// actions accordingly.<br/>
    /// It also exposes itself as a <see cref="VisualElement"/> for integration into UI Toolkit hierarchies.
    /// </para>
    /// <para>
    /// This button implementation is designed to be lightweight and visually consistent with editor styling,<br/>
    /// while providing straightforward customization for the Play Mode Zone environment.
    /// </para>
    /// </remarks>

    public class PredefinedPlayZoneButton : VisualElement, INicePredefinedButton
    {
        private readonly Color _baseColor = new Color(0.22f, 0.22f, 0.22f);
        private readonly VisualElement _container;
        private readonly Color _hoverColor = new Color(0.33f, 0.33f, 0.33f);
        private readonly Label _label;

        public PredefinedPlayZoneButton()
        {
            name = "PredefinedPlayZoneButton";

            // Container setup
            _container = new VisualElement();
            _container.style.flexDirection = FlexDirection.Row;
            _container.style.alignItems = Align.Center;
            _container.style.justifyContent = Justify.Center;
            _container.style.paddingLeft = 6;
            _container.style.paddingRight = 6;
            _container.style.height = 22;
            _container.style.marginLeft = 4;
            _container.style.marginRight = 4;
            _container.style.backgroundColor = _baseColor;

            // Border styling
            _container.style.borderBottomWidth = 1;
            _container.style.borderTopWidth = 1;
            _container.style.borderLeftWidth = 1;
            _container.style.borderRightWidth = 1;
            _container.style.borderBottomColor = Color.black;
            _container.style.borderTopColor = Color.black;
            _container.style.borderLeftColor = Color.black;
            _container.style.borderRightColor = Color.black;
            _container.style.borderTopLeftRadius = 3;
            _container.style.borderBottomLeftRadius = 3;
            _container.style.borderTopRightRadius = 3;
            _container.style.borderBottomRightRadius = 3;

            // Label setup
            //_label = new Label("button");
            _label = new Label();
            _label.name = "label";
            _label.style.color = Color.white;
            _label.style.unityFontStyleAndWeight = FontStyle.Bold;
            _label.style.unityTextAlign = TextAnchor.MiddleCenter;
            _label.style.flexShrink = 0;

            _container.Add(_label);
            Add(_container);

            // Hover effect
            _container.RegisterCallback<MouseEnterEvent>(_ =>
            {
                _container.style.backgroundColor = _hoverColor;
            });
            _container.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                _container.style.backgroundColor = _baseColor;
            });

            // Click / Pointer logic
            _container.RegisterCallback<PointerDownEvent>(evt =>
            {
                if(evt.button == 0)
                {
                    PointerDown?.Invoke();
                    evt.StopPropagation();
                }
            });

            _container.RegisterCallback<ClickEvent>(_ =>
            {
                PointerClicked?.Invoke();
            });
        }

        public event Action PointerClicked;

        public event Action PointerDown;

        VisualElement INicePredefinedButton.AsVisualElement() => (VisualElement)this;

        public void SetText(string text)
        {
            _label.text = text;
            name = text;
        }

        public void SetTooltip(string tooltip) => _container.tooltip = tooltip;
    }
}
