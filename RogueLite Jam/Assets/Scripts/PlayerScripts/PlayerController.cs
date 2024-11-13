using NUnit.Framework.Internal.Commands;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector2 _movement;

    #region Player Stats
    [SerializeField] private float _moveSpeed = 22f;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _baseDamage = 5f;
    [SerializeField] private float _defense = 5f;
    [SerializeField] private float _critChance = .05f;
    [SerializeField] private float _critMultiple = 1.5f;
    [SerializeField] private float _luck = .05f;
    #endregion

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        SetMovement();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SetMovement()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        _movement = new Vector2(xMove, yMove);
        //Debug.Log($"x: {xMove}, y: {yMove}");
    }

    private void MovePlayer()
    {
        playerRb.AddForce(_movement * _moveSpeed);
        //Debug.Log($"movement {_movement.x}, {_movement.y} and speed: {_moveSpeed}");
    }
}
