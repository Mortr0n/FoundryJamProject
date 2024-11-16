using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject Player {  get; set; }
    public static event Action<GameObject> OnPlayerRegistered;

    private void Awake() // can only garauntee this will work due to Preload manager.  probably my new fav way of ensuring static singletons work from now on.
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
        // after it sets the Player GameObject then I invoke the event for OnPlayerRegistered which I have a function looking for it in the EnemyBase class
        OnPlayerRegistered?.Invoke(player); 
    }

}
