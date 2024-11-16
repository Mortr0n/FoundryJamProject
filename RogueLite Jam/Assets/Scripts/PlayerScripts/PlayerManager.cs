using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject Player {  get; set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static event Action<GameObject> OnPlayerRespawn;
    public static void NotifyPlayerRespawn(GameObject player)
    {
        OnPlayerRespawn?.Invoke(player);
    }
}
