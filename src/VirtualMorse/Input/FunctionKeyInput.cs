using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualMorse.Input
{
    public class FunctionKeyInput
    {
        public event EventHandler<SwitchInputEventArgs> KeyPressed;

        public Dictionary<Keys, Switch> targetKeys = new Dictionary<Keys, Switch>()
        {
            { Keys.F1, Switch.Switch1 },
            { Keys.F2, Switch.Switch2 },
            { Keys.F3, Switch.Switch3 },
            { Keys.F4, Switch.Switch4 },
            { Keys.F5, Switch.Switch5 },
            { Keys.F6, Switch.Switch6 },
            { Keys.F7, Switch.Switch7 },
            { Keys.F8, Switch.Switch8 },
            { Keys.F9, Switch.Switch9 },
            { Keys.F10, Switch.Switch10 },
        };

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (targetKeys.ContainsKey(e.KeyCode))
            {
                if (e.KeyCode == Keys.F10)
                {
                    e.SuppressKeyPress = true;
                }
                KeyPressed?.Invoke(this, new SwitchInputEventArgs(targetKeys[e.KeyCode]));
            }
        }
    }
}
