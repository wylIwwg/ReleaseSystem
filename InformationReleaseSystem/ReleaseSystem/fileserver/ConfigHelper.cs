using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseSystem.fileserver
{
    public class ConfigHelper
    {
        #region Methods

        public static string GetWorkDir()
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".cfg");
            if (File.Exists(file))
                return File.ReadAllText(file);
            return string.Empty;
        }

        public static void SaveWorkDir(string dir)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Process.GetCurrentProcess().ProcessName + ".cfg");
            try
            {
                File.WriteAllText(file, dir);
            }
            catch
            {
            }
        }

        #endregion Methods
    }
}