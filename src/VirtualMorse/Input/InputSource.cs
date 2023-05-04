// abstract class InputSource
// Base class for classes implementing an input source

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
        // Subscribe to this event to know when a switch has been activated
        public event EventHandler<SwitchInputEventArgs> switchActivated;

        // Reference to a RichTextBox in case derived classes need it
        protected RichTextBox textBox;

        public InputSource(RichTextBox textBox)
        {
            this.textBox = textBox;
        }

        // Function that derived classes can call to raise the switchActivated event,
        //   passing the switch value that was activated
        protected void OnSwitchActivated(SwitchInputEventArgs e)
        {
            switchActivated?.Invoke(this, e);
        }
    }
}
