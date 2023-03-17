using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualMorse
{
    public class FunctionKeyInput
    {
        public event EventHandler<SwitchInputEventArgs> KeyPressed;

        public Dictionary<Keys, int> targetKeys = new Dictionary<Keys, int>()
        {
            { Keys.F1, 1 },
            { Keys.F2, 2 },
            { Keys.F3, 3 },
            { Keys.F4, 4 },
            { Keys.F5, 5 },
            { Keys.F6, 6 },
            { Keys.F7, 7 },
            { Keys.F8, 8 },
            { Keys.F9, 9 },
            { Keys.F10, 10 },
        };

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (targetKeys.ContainsKey(e.KeyCode))
            {
                if (e.KeyCode == Keys.F10)
                {
                    e.SuppressKeyPress = true;
                }
                Console.WriteLine("F Key pressed");
                KeyPressed?.Invoke(this, new SwitchInputEventArgs(targetKeys[e.KeyCode]));
            }
        }
    }
}
