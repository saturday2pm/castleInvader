using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

using UnityEngine;

namespace UniChain
{
    public static class UniChainEditor
    {
        private static readonly string path = Application.dataPath + "/UniChain";

        //[UnityEditor.InitializeOnLoadMethod]
        private static void ProcessAssembly()
        {
            ProcessStartInfo info = new ProcessStartInfo();

            info.FileName = Application.dataPath + "/../unichain/unichain.exe";
            info.WorkingDirectory = Application.dataPath + "/../unichain";
            info.Arguments = Application.dataPath;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            
            Process.Start(info).WaitForExit();
        }
         
        [UnityEditor.MenuItem("Window/Profiler.Ext", isValidateFunction: false, priority: 2007)]
        private static void Refresh()
        {

#if UNICHAIN_DISABLED
            try
            {
                System.IO.File.WriteAllText(path + "/TrashScript.cs", "/* " + DateTime.Now + " */");
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
            }

            UnityEditor.AssetDatabase.Refresh();
#endif
            Install();
            ProcessAssembly();

            UnityEditor.EditorApplication.ExecuteMenuItem("Window/Profiler");
        }

        private static void Install()
        {
            var installPath = Application.dataPath + "/../unichain";
            if (Directory.Exists(installPath))
                return;

            Directory.CreateDirectory(installPath);
            try
            {
                foreach (var file in Directory.GetFiles(path + "/Install", "*.bb"))
                {
                    var fileName = Path.GetFileName(file);
                    File.Copy(file, installPath + "/" + fileName.Substring(0, fileName.Length - 2));
                }
            }
            catch 
            {
                Directory.Delete(installPath);
            }
        }
    }

}
