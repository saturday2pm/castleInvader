using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEditor;

class Jenkins
{
    private static readonly string Host = "http://52.78.200.200:8080/";

    private static readonly string User = "pjc";
    private static readonly string Pass = "asdf1234";

    [MenuItem("Jenkins/win32")]
    public static void Build_Win32()
    {
        var header = new Dictionary<string, string>()
        {
            {"Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(User + ":" + Pass)) }
        };
        var www = new WWW(Host + "job/client.win32/build?jenkins_status=1&jenkins_sleep=3", new byte[] { 0 }, header);

        while (www.isDone == false)
        {
        }

        if (www.error != null)
            Debug.LogError(www.error);
        else
            Debug.Log("DONE");
    }
}