using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualMorse
{
    public class FunctionKeyInput
    {
        public event EventHandler KeyPressed;

        public HashSet<Keys> targetKeys = new HashSet<Keys>()
        {
            Keys.F1,
            Keys.F2,
            Keys.F3,
            Keys.F4,
            Keys.F5,
            Keys.F6,
            Keys.F7,
            Keys.F8,
            Keys.F9,
            Keys.F10,
        };

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (targetKeys.Contains(e.KeyCode))
            {
                if (e.KeyCode == Keys.F10)
                {
                    e.SuppressKeyPress = true;
                }
                Console.WriteLine("F Key pressed");
                KeyPressed?.Invoke(this, e);
            }
        }
    }
}
