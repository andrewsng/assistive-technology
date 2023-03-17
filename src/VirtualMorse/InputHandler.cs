using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VirtualMorse
{
    public class InputHandler
    {
        public event EventHandler InputReceived;

        public void DeviceInputReceived(object sender, SwitchInputEventArgs e)
        {
            Console.WriteLine("Received input in handler - Switch " + e.switchNum.ToString());
            InputReceived?.Invoke(sender, e);
        }
    }
}
