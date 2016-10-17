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
        wc.DownloadFile("https://s3.ap-northeast-2.amazonaws.com/st2pm/Debug/ProtocolCS.dll",
            Application.dataPath + "/Plugins/protocolCS/ProtocolCS.dll");

        AssetDatabase.ImportAsset(Application.dataPath + "/Plugins/protocolCS/ProtocolCS.dll", ImportAssetOptions.ForceUpdate);
    }
}