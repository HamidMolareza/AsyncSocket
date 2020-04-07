using System;

namespace AsyncSocket.Utility {
    public static class OperatingSystemUtility {
        public static bool IsLinux {
            get {
                var p = (int) Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }
    }
}