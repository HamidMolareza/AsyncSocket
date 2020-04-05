using System;

namespace AsyncSocket.Utility {
    public static class OperatingSystemUtility {
        public static bool IsLinux {
            get {
                int p = (int) Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}