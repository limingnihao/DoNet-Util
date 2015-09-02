using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Org.Limingnihao.Api.Util
{
    public class GetCursorPosUtil
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINTt
        {
            public int X;
            public int Y;

            public POINTt(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINTt pt);
    }
}
