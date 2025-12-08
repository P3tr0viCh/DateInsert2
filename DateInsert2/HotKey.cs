using P3tr0viCh.Utils;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DateInsert2
{
    internal class HotKey
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, Keys vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        public const int WM_HOTKEY = 0x0312;

        public const int MOD_ALT = 0x0001;
        public const int MOD_CONTROL = 0x0002;
        public const int MOD_SHIFT = 0x0004;

        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;
        public const int VK_SHIFT = 0x10;
        public const int VK_MENU = 0x12;

        public static bool RegisterHotKey(IntPtr hWnd, Keys keys, int id)
        {
            if (keys == Keys.None)
            {
                return false;
            }

            var modifiers = 0x0;

            if ((keys & Keys.Alt) == Keys.Alt)
            {
                modifiers |= MOD_ALT;
            }
            if ((keys & Keys.Control) == Keys.Control)
            {
                modifiers |= MOD_CONTROL;
            }
            if ((keys & Keys.Shift) == Keys.Shift)
            {
                modifiers |= MOD_SHIFT;
            }

            var key = keys & ~Keys.Modifiers;

            var result = RegisterHotKey(hWnd, id, modifiers, key);

            DebugWrite.Line($"{ConvertToString(keys)}, {result}");

            return result;
        }

        public static bool IsKey(int keyCode)
        {
            return (GetAsyncKeyState(keyCode) & 0x8000) != 0;
        }

        public static bool IsShift()
        {
            return IsKey(VK_SHIFT);
        }

        public static bool IsLCtrl()
        {
            return IsKey(VK_LCONTROL);
        }

        public static bool IsRCtrl()
        {
            return IsKey(VK_RCONTROL);
        }

        public static bool IsCtrl()
        {
            return IsLCtrl() || IsRCtrl();
        }

        public static bool IsAlt()
        {
            return IsKey(VK_MENU);
        }

        public static bool IsAnyModifyers()
        {
            return IsShift() || IsCtrl() || IsAlt();
        }

        public static string ConvertToString(Keys keys)
        {
            return new KeysConverter().ConvertToString(keys);
        }
    }
}