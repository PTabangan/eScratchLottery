using System.ComponentModel;

namespace eScratchLottery.Server
{
    public class AppSettings
    {
        [Description("The url to bind")]
        public string BindUrl { get; set; }

        [Description("The path to the directory of frontend")]
        public string WwwRoot { get; set; }
    }
}
