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

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                Console.WriteLine("F1 pressed");
            }
            KeyPressed?.Invoke(this, e);
        }
    }
}
