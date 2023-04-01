using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualMorse
{
    public class SwitchInputEventArgs : EventArgs
    {
        public Switch switchInput;

        public SwitchInputEventArgs(Switch switchActivated)
        {
            this.switchInput = switchActivated;
        }
    }
}
