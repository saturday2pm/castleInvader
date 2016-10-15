using UnityEngine;
using UnityEditor;
using System.Net;

public class UpdateProtocolCS : MonoBehaviour
{

    [MenuItem("ProtocolCS/Update")]
    public static void Update()
    {
        System.Net.WebClient wc = new System.Net.WebClient();

        ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
        wc.DownloadFile("https://github.com/saturday2pm/ProtocolCS/blob/release/ProtocolCS.dll?raw=true",
            Application.dataPath + "/Plugins/protocolCS/ProtocolCS.dll");

        AssetDatabase.ImportAsset(Application.dataPath + "/Plugins/protocolCS/ProtocolCS.dll", ImportAssetOptions.ForceUpdate);
    }
}