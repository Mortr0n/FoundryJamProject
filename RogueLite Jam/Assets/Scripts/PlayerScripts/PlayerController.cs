using NUnit.Framework.Internal.Commands;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private Vector2 _movement;
    private Animator _pAnimator;

    #region Player Stats
    [SerializeField] private float _moveSpeed = 22f;
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _baseDamage = 5f;
    [SerializeField] private float _defense = 1f;
    [SerializeField] private float _critChance = .05f;
    [SerializeField] private float _critMultiple = 1.5f;
    [SerializeField] private float _luck = .05f;
    private bool _baseMoveEnabled = true;
    private bool _isMoving = false;
    private float _moveThreshold = .05f;
    #endregion

    #region Properties
    public float MoveSpeed {  get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float Health { get { return _health; } set { _health = value; } }
    public float BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float CritChance {  get { return _critChance; } set {_critChance = value; } }
    public float CritMultiple {  get { return _critMultiple; } set {_critMultiple = value; } }
    public float Luck { get { return _luck; } set { _luck = value; } }
    public bool BaseMoveEnabled { get { return _baseMoveEnabled; } set { _baseMoveEnabled=value; } }
    #endregion



    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        _pAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        SetMovement();
    }

    private void FixedUpdate()
    {
        if (_baseMoveEnabled)
        {
            MovePlayer();
        }
        
    }

    protected virtual void SetMovement()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");

        _movement = new Vector2(xMove, yMove);
        _movement.Normalize();

        _isMoving = Mathf.Abs(xMove) > _moveThreshold || Mathf.Abs(yMove) > _moveThreshold; //for animating
    }

    protected virtual void MovePlayer()
    {
        playerRb.AddForce(_movement * _moveSpeed);
        WalkAnimator();
    }

    protected void WalkAnimator()
    {
        if (!_isMoving)
        {
            _pAnimator.SetBool("isMoving", false);
        }
        else
        {
            _pAnimator.SetBool("isMoving", true);
        }
    }

    public void DamagePlayer(float amount, string damageType)
    {
        float modifiedAmount = amount;
        //TODO: ok so in here we can do the math for the defense and armor and magic protection if it's a magic attack.  Maybe I'll put a string for what kind of attack or whatever
        switch(damageType)
        {
            case ("magic"):
                Debug.Log("magic attack damage modifiers");
                break;
            case ("physical"):
                
                modifiedAmount -= Defense;
                Debug.Log($"physical attack damage modifiers modAmt: {modifiedAmount}");
                break;
            default:
                Debug.LogError($"unsupported attack damage type [{damageType}]");
                break;
        }

        // end result ends up finally being 
        Health -= modifiedAmount;
        
        if (Health <= 0)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("Player Die Now");
        throw new NotImplementedException();
    }
}
