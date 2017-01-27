using UnityEditor;
using UnityEngine;
using kode80.Versioning;

[InitializeOnLoad]
public class UpdateChecker
{
    static UpdateChecker()
    {
        EditorApplication.update += RunOnce;
    }
    private static void RunOnce()
    {
        if(EditorPrefs.GetFloat("UMA_EditorTime") > EditorApplication.timeSinceStartup)
        {
            EditorPrefs.SetFloat("UMA_EditorTime", (float)EditorApplication.timeSinceStartup);
            EditorApplication.update -= RunOnce;

            AssetUpdater.Instance.Refresh();

            AssetUpdater.Instance.remoteVersionDownloadFinished += RemoteVersionDownloadFinished;
            AssetUpdater.Instance.remoteVersionDownloadFailed += RemoteVersionDownloadFailed;
        }
    }

    private static void RemoteVersionDownloadFinished( AssetUpdater updater, int assetIndex)
    {
        AssetVersion remote = updater.GetRemoteVersion(0);
        AssetVersion local = updater.GetLocalVersion(0);
        if(remote != local)
        {
            AssetUpdateWindow.Init();
        }
    }

    private static void RemoteVersionDownloadFailed( AssetUpdater updater, int assetIndex)
    {
        Debug.Log("Failed to get remote UMA version.");
    }
}