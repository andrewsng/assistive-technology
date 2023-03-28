using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualMorse
{
    public class InputHandler
    {
        public event EventHandler<SwitchInputEventArgs> InputReceived;

        public void DeviceInputReceived(object sender, SwitchInputEventArgs e)
        {
            InputReceived?.Invoke(sender, e);
        }
    }
}
