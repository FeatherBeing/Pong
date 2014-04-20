using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Pong
{
    static class Keyboard
    {
        public static Keys[] GetPressedKeys()
        {
            Byte[] keys = new Byte[256];
            var pressedKeys = new List<Keys>(); 

            if (!NativeMethods.GetKeyboardState(keys))
            {
                int err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }

            for (int i = 0; i < keys.Length; i++)
            {
                byte key = keys[i];

                if ((key & 0x80) != 0)
                {
                    pressedKeys.Add((Keys)i);
                }
                
            }
            return pressedKeys.ToArray();
        }
    }
}
