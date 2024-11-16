using System;
using System.Collections;
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

    [SerializeField] private float _pForce = 1f;
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
        StartCoroutine(WaitToAssignPlayerInstance());
        
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

    private IEnumerator WaitToAssignPlayerInstance()
    {
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance.Player = this.gameObject; // needs reference to the player on start
        playerRb = GetComponent<Rigidbody2D>();
        _pAnimator = GetComponentInChildren<Animator>();
        Debug.Log($"Movement: {_movement}, Velocity: {playerRb.linearVelocity}");
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



    private void RespawnPlayer()
    {
        PlayerManager.Instance.Player = this.gameObject;
        PlayerManager.NotifyPlayerRespawn(this.gameObject);
    }
}
