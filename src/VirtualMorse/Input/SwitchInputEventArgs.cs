// class SwitchInputEventArgs
// Derived from EventArgs
// Used to pass switch values as an argument when raising an event

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualMorse.Input
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
