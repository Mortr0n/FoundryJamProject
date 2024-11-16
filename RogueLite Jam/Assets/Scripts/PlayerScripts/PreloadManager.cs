using UnityEngine;

public class PreloadManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Preload()
    {
        if (PlayerManager.Instance == null)
        {
            GameObject playerManager = new GameObject("PlayerManager");
            playerManager.AddComponent<PlayerManager>();
        }



        Debug.Log("All Managers Preloaded");  // will make more sense once we have a few more in here.
    }

}
