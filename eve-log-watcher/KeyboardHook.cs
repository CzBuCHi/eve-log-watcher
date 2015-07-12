using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace eve_log_watcher
{
    public sealed class KeyboardHook : IDisposable
    {
        private readonly Window _Window = new Window();
        private const int cCurrentId = 1697536;

        public KeyboardHook() {
            _Window.KeyPressed += delegate(object sender, KeyPressedEventArgs args) {
                KeyPressed?.Invoke(this, args);
            };
        }

        public void Dispose() {
            for (int i = cCurrentId; i > 0; i--) {
                UnregisterHotKey(_Window.Handle, i);
            }

            _Window.Dispose();
        }

        public void RegisterHotKey(Keys key) {
            ModifierKeys modifier = ModifierKeys.None;

            if ((key & Keys.Alt) == Keys.Alt) {
                modifier |= ModifierKeys.Alt;
                key = key & ~Keys.Alt;
            }
            if ((key & Keys.Control) == Keys.Control) {
                modifier |= ModifierKeys.Control;
                key = key & ~Keys.Control;
            }
            if ((key & Keys.Shift) == Keys.Shift) {
                modifier |= ModifierKeys.Shift;
                key = key & ~Keys.Shift;
            }

            if (!RegisterHotKey(_Window.Handle, cCurrentId, (uint) modifier, (uint) key)) {
                throw new InvalidOperationException("Couldn’t register the hot key.");
            }
        }

        public void UnregisterHotKey() {
            UnregisterHotKey(_Window.Handle, cCurrentId);
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private sealed class Window : NativeWindow, IDisposable
        {
            // ReSharper disable once InconsistentNaming
            private const int WM_HOTKEY = 0x0312;

            public Window() {
                CreateHandle(new CreateParams());
            }

            public void Dispose() {
                DestroyHandle();
            }

            protected override void WndProc(ref Message m) {
                base.WndProc(ref m);

                if (m.Msg != WM_HOTKEY) {
                    return;
                }

                Keys key = (Keys) (((int) m.LParam >> 16) & 0xFFFF);
                ModifierKeys modifier = (ModifierKeys) ((int) m.LParam & 0xFFFF);

                if ((modifier & ModifierKeys.Alt) == ModifierKeys.Alt) {
                    key = key | Keys.Alt;
                }
                if ((modifier & ModifierKeys.Control) == ModifierKeys.Control) {
                    key = key | Keys.Control;
                }
                if ((modifier & ModifierKeys.Shift) == ModifierKeys.Shift) {                        
                    key = key | Keys.Shift;
                }


                KeyPressed?.Invoke(this, new KeyPressedEventArgs(key));
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;
        }
    }

    public class KeyPressedEventArgs : EventArgs
    {
        internal KeyPressedEventArgs(Keys key) {
            Key = key;
        }

        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
        public Keys Key { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global
    }

    [Flags]
    public enum ModifierKeys : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
