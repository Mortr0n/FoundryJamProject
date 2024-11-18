using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    protected GameObject _target;
    protected Rigidbody2D thisRb;
    protected float _moveSpeed = 10f;
    protected float _damageAmount = 10f;
    protected DamageType _damageType = DamageType.Magic;
    public virtual void SetTarget(GameObject target)
    {
        _target = target;
        //Debug.Log($"target set: {_target}, target: {target}");
    }

    private void Update()
    {
        MoveToTarget();
    }

    private void Start()
    {
        thisRb = GetComponent<Rigidbody2D>();
        //Debug.Log($"ThisRB: {thisRb}");
    }

    protected virtual void MoveToTarget()
    {
        if (_target == null) return;
        
        

        Vector2 targetPos = _target.transform.position;
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 moveDirection = (targetPos - thisPos).normalized;
        
        //thisRb.AddForce(moveDirection * _moveSpeed); adding force which causes them to fly right by
        thisRb.linearVelocity = moveDirection * _moveSpeed;
        //Debug.Log($"linearVel: {thisRb.linearVelocity} and targetPOS: {targetPos} and thisPOS: {thisPos} and moveDir: {moveDirection}");
    }

    protected virtual void OnTriggerEnter2D(Collider2D coll)
    {
        ICombatEntity cEnt;
        cEnt = _target.GetComponent<ICombatEntity>();
        Debug.Log($"cEnt {cEnt}");

        if (cEnt == null)
        {
            Debug.LogWarning("unable to get entity from target");
            cEnt = coll.gameObject.GetComponent<ICombatEntity>() as ICombatEntity;
            if (cEnt == null)
            {
                Debug.LogError("completely unable to get combat entity for damage");
            }
        }

        cEnt.TakeDamage(_damageAmount, _damageType);
        Destroy(gameObject);
    }
}
