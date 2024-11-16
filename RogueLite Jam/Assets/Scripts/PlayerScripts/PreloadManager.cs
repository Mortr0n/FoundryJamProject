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
    }

}
