using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace eve_log_watcher
{
    public sealed class KeyboardHook : IDisposable
    {
        private readonly Window _Window = new Window();
        private int _CurrentId;

        public KeyboardHook() {
            _Window.KeyPressed += delegate(object sender, KeyPressedEventArgs args) { KeyPressed?.Invoke(this, args); };
        }

        public void Dispose() {
            for (int i = _CurrentId; i > 0; i--) {
                UnregisterHotKey(_Window.Handle, i);
            }

            _Window.Dispose();
        }

        public void RegisterHotKey(ModifierKeys modifier, Keys key) {
            _CurrentId = _CurrentId + 1;

            if (!RegisterHotKey(_Window.Handle, _CurrentId, (uint) modifier, (uint) key)) {
                throw new InvalidOperationException("Couldn’t register the hot key.");
            }
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private sealed class Window : NativeWindow, IDisposable
        {
            private static readonly int WM_HOTKEY = 0x0312;

            public Window() {
                CreateHandle(new CreateParams());
            }

            public void Dispose() {
                DestroyHandle();
            }

            protected override void WndProc(ref Message m) {
                base.WndProc(ref m);

                if (m.Msg == WM_HOTKEY) {
                    Keys key = (Keys) (((int) m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys) ((int) m.LParam & 0xFFFF);
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key) {
            Modifier = modifier;
            Key = key;
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public ModifierKeys Modifier { get; }
        public Keys Key { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global
    }

    [Flags]
    public enum ModifierKeys : uint
    {
        // ReSharper disable UnusedMember.Global
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
        // ReSharper restore UnusedMember.Global
    }
}
