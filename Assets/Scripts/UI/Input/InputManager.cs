﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Input
{
    public class InputManager : MonoBehaviour
    {
        private static Dictionary<InputCannon, KeyCode> inputCannon = new Dictionary<InputCannon, KeyCode>();
        private static Dictionary<InputHorse, KeyCode> inputHorse = new Dictionary<InputHorse, KeyCode>();
        private static Dictionary<InputHuman, KeyCode> inputHuman = new Dictionary<InputHuman, KeyCode>();
        private static Dictionary<InputTitan, KeyCode> inputTitan = new Dictionary<InputTitan, KeyCode>();
        private static Dictionary<InputUi, KeyCode> inputUi = new Dictionary<InputUi, KeyCode>();

        private const string CannonPlayerPrefs = "InputCannon";
        private const string HorsePlayerPrefs = "InputHorse";
        private const string HumanPlayerPrefs = "InputHuman";
        private const string TitanPlayerPrefs = "InputTitan";
        private const string UiPlayerPrefs = "InputUi";

        public const KeyCode ScrollUp = KeyCode.Joystick8Button18;
        public const KeyCode ScrollDown = KeyCode.Joystick8Button19;

        private void Awake()
        {
            LoadRebinds();
        }

        #region Default Rebinds

        private static void SetDefaultCannonKeyBindings()
        {
            inputCannon = new Dictionary<InputCannon, KeyCode>
            {
                [InputCannon.Up] = KeyCode.W,
                [InputCannon.Down] = KeyCode.S,
                [InputCannon.Left] = KeyCode.A,
                [InputCannon.Right] = KeyCode.D,
                [InputCannon.Shoot] = KeyCode.Q,
                [InputCannon.Slow] = KeyCode.LeftShift,
                [InputCannon.Mount] = KeyCode.G
            };

            PlayerPrefs.SetString(CannonPlayerPrefs, JsonConvert.SerializeObject(inputCannon));
        }

        private static void SetDefaultHorseKeyBindings()
        {
            inputHorse = new Dictionary<InputHorse, KeyCode>
            {
                [InputHorse.Forward] = KeyCode.W,
                [InputHorse.Backward] = KeyCode.S,
                [InputHorse.Left] = KeyCode.A,
                [InputHorse.Right] = KeyCode.D,
                [InputHorse.Jump] = KeyCode.LeftShift,
                [InputHorse.Walk] = KeyCode.Space,
                [InputHorse.Mount] = KeyCode.LeftControl
            };

            PlayerPrefs.SetString(HorsePlayerPrefs, JsonConvert.SerializeObject(inputHorse));
        }

        private static void SetDefaultHumanKeyBindings()
        {
            inputHuman = new Dictionary<InputHuman, KeyCode>
            {
                [InputHuman.Forward] = KeyCode.W,
                [InputHuman.Backward] = KeyCode.S,
                [InputHuman.Left] = KeyCode.A,
                [InputHuman.Right] = KeyCode.D,
                [InputHuman.Jump] = KeyCode.LeftShift,
                [InputHuman.Gas] = KeyCode.LeftShift,
                [InputHuman.Dodge] = KeyCode.LeftControl,
                [InputHuman.Salute] = KeyCode.N,
                [InputHuman.Reload] = KeyCode.R,
                [InputHuman.ReelIn] = KeyCode.Space,
                [InputHuman.ReelOut] = KeyCode.Mouse2,
                [InputHuman.GasBurst] = KeyCode.LeftAlt,
                [InputHuman.Attack] = KeyCode.Mouse0,
                [InputHuman.AttackSpecial] = KeyCode.Mouse1,
                [InputHuman.HookLeft] = KeyCode.Q,
                [InputHuman.HookRight] = KeyCode.E,
                [InputHuman.HookBoth] = KeyCode.None,
                [InputHuman.Item1] = KeyCode.Alpha1,
                [InputHuman.Item2] = KeyCode.Alpha2,
                [InputHuman.Item3] = KeyCode.Alpha3,
                [InputHuman.Focus] = KeyCode.F
            };

            PlayerPrefs.SetString(HumanPlayerPrefs, JsonConvert.SerializeObject(inputHuman));
        }

        private static void SetDefaultTitanKeyBindings()
        {
            inputTitan = new Dictionary<InputTitan, KeyCode>
            {
                [InputTitan.Forward] = KeyCode.W,
                [InputTitan.Backward] = KeyCode.S,
                [InputTitan.Left] = KeyCode.A,
                [InputTitan.Right] = KeyCode.D,
                [InputTitan.AttackGrabFront] = KeyCode.Alpha1,
                [InputTitan.AttackBite] = KeyCode.Alpha2,
                [InputTitan.AttackGrabBack] = KeyCode.Alpha3,
                [InputTitan.Jump] = KeyCode.Space,
                [InputTitan.Walk] = KeyCode.LeftShift,
                [InputTitan.AttackPunch] = KeyCode.Q,
                [InputTitan.AttackBodySlam] = KeyCode.E,
                [InputTitan.AttackSlap] = KeyCode.Mouse0,
                [InputTitan.AttackGrabNape] = KeyCode.Mouse1,
                [InputTitan.Cover] = KeyCode.Z,
                [InputTitan.Blend] = KeyCode.F
            };

            PlayerPrefs.SetString(TitanPlayerPrefs, JsonConvert.SerializeObject(inputTitan));
        }

        private static void SetDefaultUiKeyBindings()
        {
            inputUi = new Dictionary<InputUi, KeyCode>
            {
                [InputUi.Chat] = KeyCode.Return,
                [InputUi.ToggleCursor] = KeyCode.X,
                [InputUi.LiveCamera] = KeyCode.Y,
                [InputUi.Minimap] = KeyCode.M,
                [InputUi.Fullscreen] = KeyCode.Backspace,
                [InputUi.Camera] = KeyCode.C,
                [InputUi.Pause] = KeyCode.P,
                [InputUi.Restart] = KeyCode.T,
                [InputUi.InteractionWheel] = KeyCode.Tab
            };

            PlayerPrefs.SetString(UiPlayerPrefs, JsonConvert.SerializeObject(inputUi));
        }

        #endregion

        public static void LoadRebinds()
        {
            var cannonRebinds = PlayerPrefs.GetString(CannonPlayerPrefs);
            if (string.IsNullOrEmpty(cannonRebinds))
                SetDefaultCannonKeyBindings();

            inputCannon = JsonConvert.DeserializeObject<Dictionary<InputCannon, KeyCode>>(cannonRebinds);

            var horseRebinds = PlayerPrefs.GetString(HorsePlayerPrefs);
            if (string.IsNullOrEmpty(horseRebinds))
                SetDefaultHorseKeyBindings();

            inputHorse = JsonConvert.DeserializeObject<Dictionary<InputHorse, KeyCode>>(horseRebinds);

            var humanRebinds = PlayerPrefs.GetString(HumanPlayerPrefs);
            if (string.IsNullOrEmpty(humanRebinds))
                SetDefaultHumanKeyBindings();

            inputHuman = JsonConvert.DeserializeObject<Dictionary<InputHuman, KeyCode>>(humanRebinds);

            var titanRebinds = PlayerPrefs.GetString(TitanPlayerPrefs);
            if (string.IsNullOrEmpty(titanRebinds))
                SetDefaultTitanKeyBindings();

            inputTitan = JsonConvert.DeserializeObject<Dictionary<InputTitan, KeyCode>>(titanRebinds);

            var uiRebinds = PlayerPrefs.GetString(UiPlayerPrefs);
            if (string.IsNullOrEmpty(uiRebinds))
                SetDefaultUiKeyBindings();

            inputUi = JsonConvert.DeserializeObject<Dictionary<InputUi, KeyCode>>(uiRebinds);
        }

        public static void SaveRebinds<T>(Dictionary<T, KeyCode> newInput)
        {
            var json = JsonConvert.SerializeObject(newInput);
            PlayerPrefs.SetString(GetPlayerPrefs<T>(), json);
            LoadRebinds();
        }

        public static void SetDefaultRebinds()
        {
            SetDefaultCannonKeyBindings();
            SetDefaultHorseKeyBindings();
            SetDefaultHumanKeyBindings();
            SetDefaultTitanKeyBindings();
            SetDefaultUiKeyBindings();
        }

        private static Dictionary<T, KeyCode> GetRebinds<T>(T type)
        {
            if (type is InputCannon)
            {
                return inputCannon as Dictionary<T, KeyCode>;
            }
            if (type is InputHorse)
            {
                return inputHorse as Dictionary<T, KeyCode>;
            }
            if (type is InputHuman)
            {
                return inputHuman as Dictionary<T, KeyCode>;
            }
            if (type is InputTitan)
            {
                return inputTitan as Dictionary<T, KeyCode>;
            }
            if (type is InputUi)
            {
                return inputUi as Dictionary<T, KeyCode>;
            }

            throw new ArgumentException($"{type.GetType()} is not implemented in InputManager.GetRebinds");
        }

        private static bool CanUseKey<T>(T type)
        {
            return true;
        }

        private static string GetPlayerPrefs<T>()
        {
            if (typeof(T) == typeof(InputCannon))
            {
                return CannonPlayerPrefs;
            }
            if (typeof(T) == typeof(InputHorse))
            {
                return HorsePlayerPrefs;
            }
            if (typeof(T) == typeof(InputHuman))
            {
                return HumanPlayerPrefs;
            }
            if (typeof(T) == typeof(InputTitan))
            {
                return TitanPlayerPrefs;
            }
            if (typeof(T) == typeof(InputUi))
            {
                return UiPlayerPrefs;
            }
            throw new ArgumentException($"{typeof(T)} is not implemented in InputManager.GetPlayerPrefs");
        }

        public static bool KeyPressed<T>(T inputEnum)
        {
            if (!CanUseKey(inputEnum)) return false;
            var rebinds = GetRebinds(inputEnum);
            return IsMouseScrollKeyCode(rebinds[inputEnum])
                ? IsScrolling(rebinds[inputEnum])
                : UnityEngine.Input.GetKey(rebinds[inputEnum]);
        }

        public static bool KeyDown<T>(T inputEnum)
        {
            if (!CanUseKey(inputEnum)) return false;
            var rebinds = GetRebinds(inputEnum);
            return IsMouseScrollKeyCode(rebinds[inputEnum])
                ? IsScrolling(rebinds[inputEnum])
                : UnityEngine.Input.GetKeyDown(rebinds[inputEnum]);
        }

        public static bool KeyUp<T>(T inputEnum)
        {
            if (!CanUseKey(inputEnum)) return false;
            var rebinds = GetRebinds(inputEnum);
            return IsMouseScrollKeyCode(rebinds[inputEnum])
                ? IsScrolling(rebinds[inputEnum])
                : UnityEngine.Input.GetKeyUp(rebinds[inputEnum]);
        }

        public static KeyCode GetKey<T>(T inputEnum)
        {
            var rebinds = GetRebinds(inputEnum);
            return rebinds[inputEnum];
        }

        public static bool IsMouseScrollKeyCode(KeyCode keyCode)
        {
            return keyCode == KeyCode.Joystick8Button18 || keyCode == KeyCode.Joystick8Button19;
        }

        public static bool IsScrolling(KeyCode keyCode)
        {
            if (keyCode == ScrollUp)
            {
                return IsScrollUp();
            }

            if (keyCode == ScrollDown)
            {
                return IsScrollDown();
            }

            return false;
        }

        public static bool IsScrollUp()
        {
            return UnityEngine.Input.mouseScrollDelta.y > 0;
        }

        public static bool IsScrollDown()
        {
            return UnityEngine.Input.mouseScrollDelta.y < 0;
        }
    }
}
