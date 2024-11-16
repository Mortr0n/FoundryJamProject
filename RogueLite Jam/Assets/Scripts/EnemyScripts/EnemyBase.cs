using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private GameObject _playerObject;
    private Vector2 playerPos;
    private Rigidbody2D thisRb;
    private Animator eAnimator;


    private float _moveSpeed = 2f;
    private float _baseDamage = 5f;
    private float _maxHealth = 100f;
    private float _defense = 5f;
    private float _critChance = .05f;
    private float _hitPercent = .5f;
    private DamageType _damageType = DamageType.Physical;

    private bool _attackEnabled = true;
    private float _attackTimer = .2f;
    private bool _chasePlayer = true;
    private bool _attackingPlayer = false;


    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float CritChance { get { return _critChance; } set { _critChance = value; } }
    public float HitPercent { get { return _hitPercent; } set { _hitPercent = value; } }
    public bool AttackEnabled { get { return _attackEnabled; } set { _attackEnabled = value; } }
    public float AttackTimer { get { return _attackTimer; } set { _attackTimer = value; } }
    public bool ChasePlayer { get { return _chasePlayer; } set { _chasePlayer = value; } }

    void Start()
    {
        //SetBaseStats();
        InitializeEnemy();
    }

    private void Update()
    {
        if (_playerObject == null)
        {
            _playerObject = PlayerManager.Instance?.Player;
        }
    }

    void FixedUpdate()
    {
        if (_chasePlayer && _playerObject != null)
        {
            MoveToPlayer();
        }
    }

    void OnEnable() // how an enemy subscribes to the OnPlayerRegistered event
    {
        PlayerManager.OnPlayerRegistered += UpdatePlayerReference;
    }

    void OnDisable()
    {
        PlayerManager.OnPlayerRegistered -= UpdatePlayerReference;
    }

    private void UpdatePlayerReference(GameObject player) //Updating the player object to the new player on registration
    {
        _playerObject = player;
    }

    private void InitializeEnemy()
    {
        // get player first
        _playerObject = PlayerManager.Instance.Player; //only garaunteed to be available due to preload manager!  See playercontroller adn playermanager

        // get for moving and animating
        thisRb = GetComponent<Rigidbody2D>();
        eAnimator = GetComponentInChildren<Animator>();
        
        Debug.Log($"Player: {_playerObject != null}, thisRb: {thisRb != null}, eAnimator: {eAnimator != null}");
    }

    public virtual void MoveToPlayer()
    {
        if (_playerObject == null) return;

        playerPos = _playerObject.transform.position;
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 moveDirection = (playerPos - thisPos).normalized;

        //thisRb.AddForce(moveDirection * _moveSpeed); adding force which causes them to fly right by
        thisRb.linearVelocity = moveDirection * _moveSpeed;

        if (thisRb.linearVelocity.magnitude > 0)
        {
            eAnimator.SetBool("isMoving", true);
        } 
        else
        {
            eAnimator.SetBool("isMoving", false);
        }
    }

    public virtual void BaseAttack(int number)
    {
        if (_playerObject == null) return;

        string attackCall = "attack" + (number).ToString();
        Debug.Log($"attack call : {attackCall}");
        switch (attackCall)
        {
            case ("attack1"):
                Debug.Log("Attack1");
                Attack1(attackCall);
                break;
            case ("attack2"):
                break;
            default:
                Debug.LogError($"unsupported attack case for BaseAttack called [{attackCall}]");
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision: {collision.transform.root.tag} and {collision.gameObject.name}");
        if (collision.transform.root.tag == "Player" || collision.gameObject.name == "Player")
        {
            if (!_attackingPlayer)
            {
                Debug.Log($"Base Attack 1 {_attackingPlayer}");
                BaseAttack(1);
                StartCoroutine(AttackWait());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 repulsion = transform.position -collision.transform.position;
        //Debug.Log($"Repulsion: {repulsion}");
        if (thisRb != null)
        {
            thisRb.AddForce(repulsion.normalized * 1f);
        }
    }

    public virtual void Attack1(string attackCall)
    {
        //TODO: this won't work I'll need to fire on connection with player so on trigger entered and then call the attack timer, but let's test this first
        Debug.Log("Attacking player");
        if (_playerObject != null)
        {
            PlayerController pController = _playerObject.GetComponent<PlayerController>();
            pController.TakeDamage(BaseDamage, _damageType);
            eAnimator.SetTrigger(attackCall);
        }
    }

    private IEnumerator AttackWait()
    {
        //BaseAttack(1);
        _attackingPlayer = true;
        yield return new WaitForSeconds(AttackTimer);
        _attackingPlayer = false;
    }
}
