using System.Collections.Generic;
using LizardKit.Scaffolding;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputMode
{
    MouseKeyboard,
    Touch
}

public class InputModeDetector : BaseManager<InputModeDetector>
{
    public List<GameObject> MobileOnlyButtons;
    public static InputMode Current { get; private set; } = InputMode.MouseKeyboard;

    protected override void Awake()
    {
        base.Awake();
        
        #if UNITY_ANDROID
        SetMode(InputMode.Touch);
        this.enabled = false;
        #endif
    }

    private void Update()
    {
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            SetMode(InputMode.Touch);
        }

        if ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame))
        {
            SetMode(InputMode.MouseKeyboard);
        }
    }

    private void SetMode(InputMode mode)
    {
        if (Current == mode) return;

        Current = mode;
        Log($"Input mode changed to: {mode}");

        foreach (var button in MobileOnlyButtons)
        {
            button.SetActive(mode == InputMode.Touch);
        }
    }
}