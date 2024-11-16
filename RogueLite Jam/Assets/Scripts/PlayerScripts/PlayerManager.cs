using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject Player {  get; set; }
    public static event Action<GameObject> OnPlayerRegistered;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("PlayerManager Initialized");
    }

    public void RegisterPlayer(GameObject player)
    {
        
        Player = player;
        Debug.Log($"Player registered: {player.name}");
        OnPlayerRegistered?.Invoke(player);
        
        //else
        //{
        //    Debug.LogWarning("Player already registered");
        //}
    }
    //public static void NotifyPlayerRespawn(GameObject player)
    //{
    //    Instance.Player = player;
    //    OnPlayerRespawn?.Invoke(player);
    //    Debug.Log($"PlayerManager updated with new player: {player.name}");
    //}
}
