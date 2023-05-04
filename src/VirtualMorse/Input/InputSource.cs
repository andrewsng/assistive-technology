using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualMorse.Input
{
    public abstract class InputSource
    {
        public event EventHandler<SwitchInputEventArgs> switchActivated;

        protected RichTextBox textBox;

        public InputSource(RichTextBox textBox)
        {
            this.textBox = textBox;
        }

        protected void OnSwitchActivated(SwitchInputEventArgs e)
        {
            switchActivated?.Invoke(this, e);
        }
    }
}
