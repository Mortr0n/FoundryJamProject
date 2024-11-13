using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private GameObject playerObject;
    private Vector2 playerPos;
    private Rigidbody2D thisRb;


    private float _moveSpeed = 5f;
    private float _baseDamage = 5f;
    private float _maxHealth = 100f;
    private float _defense = 5f;
    private float _critChance = .05f;
    private float _hitPercent = .5f;

    private bool _attackEnabled = true;
    private bool _chasePlayer = true;

    public float MoveSpeed {  get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float BaseDamage { get { return _baseDamage; } set { _baseDamage = value; } }
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public float Defense { get { return _defense; } set { _defense = value; } }
    public float CritChance { get { return _critChance; } set { _critChance = value; } }
    public float HitPercent { get { return _hitPercent; } set { _hitPercent = value; } }
    public bool AttackEnabled {  get { return _attackEnabled; } set { _attackEnabled = value; } }
    public bool ChasePlayer {  get { return _chasePlayer; } set {  _chasePlayer = value; } }

    void Start()
    {
        SetBaseStats();
        playerObject = GameObject.Find("Player");
        thisRb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (_chasePlayer)
        {
            MoveToPlayer();    
        }
        
    }


    public void SetBaseStats()
    {
        Debug.Log("Implement Set Base Stats");
    }

    public void MoveToPlayer()
    {
        playerPos = playerObject.transform.position;
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 moveDirection = (playerPos - thisPos).normalized;
        thisRb.AddForce(moveDirection * _moveSpeed);
    }
}
