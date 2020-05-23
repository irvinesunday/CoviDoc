using System;

namespace CoviDoc
{
    public class Constants
    {
        private const string SandboxApiHost = "https://api.sandbox.africastalking.com/version1/messaging";
        private const string LiveApiHost = "https://api.africastalking.com/version1/messaging";

        public static Uri SandboxApiHostUri = new Uri(SandboxApiHost);
        public static Uri LiveApiHostUri = new Uri(LiveApiHost);
    }
}