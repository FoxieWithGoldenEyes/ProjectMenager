using APIsDll.Local.APIs;
using System.Diagnostics;

namespace OSDll
{
    public class WindowsFunctionsProvider : IOsFunctionsProvider
    {
        public void RunExplorer(string path) =>
            Process.Start("explorer.exe", path);
    }
}