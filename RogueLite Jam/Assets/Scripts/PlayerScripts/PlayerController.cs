using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICombatEntity
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

    [SerializeField] private float _pForce = 1f;
    private bool _baseMoveEnabled = true;
    private bool _isMoving = false;
    private float _moveThreshold = .05f;
    #endregion
    private List<Weapon> _weapons = new List<Weapon>();

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
        InitializePlayer();
    }

    private void Awake()
    {
        // Utilize Preload manager to ensure that this exists.  The only way I've found that makes damn sure it's there.
        // I do not understand if there's any other way that people can reliably ensure these are available at start and 
        // awake time.
        PlayerManager.Instance.RegisterPlayer(this.gameObject);  // Registering player on awake to make sure it's available to everyone
    }

    private void Update()
    {
        SetMovement();
        foreach (var weapon in _weapons)
        {
            GameObject target = weapon.GetTarget(TargetType.Closest);
            if (target != null)
            {
                weapon.Attack(target);
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (_baseMoveEnabled)
        {
            MovePlayer();
        }
        
    }

    private void InitializePlayer()
    {
        playerRb = GetComponent<Rigidbody2D>(); 
        _pAnimator = GetComponentInChildren<Animator>();
        Debug.Log($"Movement: {_movement}, Velocity: {playerRb.linearVelocity}");
        EquipWeapon(new MagicProjectile());
    }

    private void EquipWeapon(Weapon weapon)
    {
        weapon.Initialize(this, attackPower: 5f, cooldown: 1f, range: 10f, damageType: DamageType.Magic);
        _weapons.Add(weapon);
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
        if (playerRb != null)
        {
            playerRb.AddForce(_movement * _moveSpeed);
            //playerRb.linearVelocity = _movement * _moveSpeed; //for testing.  do not use!
            WalkAnimator();
        }        
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 pushForce = (transform.position - collision.transform.position).normalized * _pForce;
        if (playerRb != null)
        {
            playerRb.AddForce(pushForce);
        }
    }

    public void TakeDamage(float amount, DamageType damageType)
    {
        float modifiedAmount = amount;
        //TODO: ok so in here we can do the math for the defense and armor and magic protection if it's a magic attack.  Maybe I'll put a string for what kind of attack or whatever
        switch(damageType)
        {
            case (DamageType.Magic):
                Debug.Log("magic attack damage modifiers");
                break;
            case (DamageType.Physical):
                
                modifiedAmount -= Defense;
                Debug.Log($"physical attack damage modifiers modAmt: {modifiedAmount} amt: {amount} and def: {Defense}");
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



    private void RespawnPlayer() // for respawning player after death.  Might need to be public.
    {
        // passing in the current player on respawn to the RegisterPlayer where it sets it equal to it's GameObject Player which is available anywhere
        PlayerManager.Instance.RegisterPlayer(this.gameObject);  // Registering player to ensure everything that depends on it can run.
    }
}
