using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtualMorse
{
    public class SwitchInputEventArgs : EventArgs
    {
        public int switchNum;

        public SwitchInputEventArgs(int switchNumber)
        {
            this.switchNum = switchNumber;
        }
    }
}
