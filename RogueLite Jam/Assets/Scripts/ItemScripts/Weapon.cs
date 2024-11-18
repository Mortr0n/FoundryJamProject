using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _attackPrefab;
    [SerializeField] private Transform _firePoint;
    private GameObject _target;
    private float _damageAdd = 10f;
    private float _coolDown = 1f;
    protected float _range = 10f;
    private bool _isRanged = true;
    private bool _isEquipped = true;
    protected float Range { get { return _range; } set { _range = value; } }

    private void Start()
    {
        Debug.Log("Starting AutoAttack Coroutine");
        StartCoroutine(AutoAttack());
    }

    protected virtual ProjectileBase RangedAttack()
    {
        ProjectileBase projectile = new ProjectileBase();
        _target = GetTarget(TargetType.Closest);
        if (_target != null)
        {
           projectile = Instantiate(_attackPrefab, _firePoint.position, Quaternion.identity).GetComponent<ProjectileBase>();


            projectile.SetTarget(_target);
            Debug.Log($"projectile: {projectile} and Target: {_target}");
        }

        return projectile;
    }
   

    protected virtual IEnumerator  AutoAttack()
    {
        Debug.Log($"Auto Attack inside {_isEquipped}");
        while (_isEquipped)
        {
            Debug.Log("Auto Attacking");
            if (_isRanged)
            {
                ProjectileBase thisProjectile = RangedAttack();
                Debug.Log($"thisProjectile: {thisProjectile}");
            }
            yield return new WaitForSeconds(_coolDown);
        
            Debug.Log("Renewing AutoAttack");
            AutoAttack();
         }
    }

    public void Equip()
    {
        _isEquipped = true;
        Debug.Log($"Is it equipped? {_isEquipped}");
    }


    //protected ICombatEntity Owner { get;  set; }

    //private float _range = 20f;
    //private float _cooldown = 1f;
    //private float _damageAdd = 2f;
    //private bool _canAttack = true;
    //private DamageType _damageType = DamageType.Physical;
    //private TargetType _targetType = TargetType.Closest;
    //private GameObject _target;
    //public bool CanAttack { get { return _canAttack; } set { _canAttack = value; } }
    //public float CoolDown { get { return _cooldown; } set { _cooldown = value; } }
    //public float DamageAdd { get { return _damageAdd; } set { _damageAdd = value; } }



    //public virtual void Initialize(ICombatEntity owner, float attackPower, float cooldown, float range, DamageType damageType)
    //{
    //    Owner = owner;
    //    _range = range;
    //    _cooldown = cooldown;
    //    _damageAdd = attackPower;
    //    _damageType = damageType;

    //    if (_canAttack)
    //    {
    //        StartCoroutine(AutoAttack());
    //    }
    //}


    //public virtual IEnumerator AutoAttack()
    //{
    //    while (_canAttack)
    //    {
    //        Debug.Log("running autoattack");
    //        if (_target == null)
    //        {
    //            _target = GetTarget(_targetType);
    //        }

    //        if (_target != null)
    //        {
    //            Attack(_target);
    //        }
    //        yield return new WaitForSeconds(_cooldown);
    //    }
    //}

    //public virtual void Attack(GameObject target)
    //{
    //    float modifiedDamage = Owner.BaseDamage;
    //    if (Owner != null)  
    //    { 
    //        modifiedDamage += _damageAdd;
    //        Debug.Log($"modDmg: {modifiedDamage}");
    //    }

    //    if (DidItCrit()) 
    //    { 
    //        modifiedDamage = AddCritDamage(modifiedDamage);
    //        Debug.Log($"modDmg: {modifiedDamage}");
    //    }

    //    if (target == null)
    //    {
    //        Debug.LogWarning("Attack Called with a null target");
    //        return;
    //    }
    //    target.GetComponent<ICombatEntity>()?.TakeDamage(modifiedDamage, _damageType);
    //}

    //public virtual bool DidItCrit()
    //{
    //    return Random.Range(0, 1000) < Owner.CritChance;
    //}

    //public virtual float AddCritDamage(float incDamage)
    //{
    //    return incDamage *= Owner.CritMultiple;
    //}

    public virtual GameObject GetTarget(TargetType targetModifier)
    {
        //LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _range);
        GameObject acquiredTarget = null;
        //Debug.Log($"targets: {targets}");
        switch (targetModifier)
        {
            case TargetType.Closest:
                acquiredTarget = GetClosestTarget(targets);
                Debug.Log($"Acquiring {targetModifier} target = {acquiredTarget}");
                break;
            case TargetType.MostCurrentHealth:
                acquiredTarget = GetMostCurrentHealthTarget(targets);
                Debug.Log($"Acquiring {targetModifier} target = {acquiredTarget}");
                break;
            //TODO: Implement the rest!!!
            case TargetType.LeastCurrentHealth:
                Debug.Log($"Acquiring {targetModifier} target =  {acquiredTarget}");
                break;
            case TargetType.MostMaxHealth:
                Debug.Log($"Acquiring {targetModifier} target =  {acquiredTarget}");
                break;
            case TargetType.HighestDamage:
                Debug.Log($"Acquiring {targetModifier} target =  {acquiredTarget}");
                break;
            default:
                Debug.Log($"targetModifier: {targetModifier} not implemented for GetTarget");
                break;
        }

        return acquiredTarget;
    }

    private GameObject GetClosestTarget(Collider2D[] targets)
    {
        //Debug.Log($"Targeting: {targets[0]}");
        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (var target in targets)
        {
            if (target == null)
            {
                Debug.LogWarning("null target in overlapcircle");
                continue;
            }
            Debug.Log($"Target: {target.gameObject}");

            if (target.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                //Debug.Log($"Checking target {target.name}, distance = {distance}");
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.gameObject;
                }
            }
        }
        if (closestTarget == null)
        {
            Debug.LogWarning("No valid targets found in GetClosestTarget!");
        }
        //Debug.Log($"Closest: {closestTarget}");
        return closestTarget;
    }

    private GameObject GetMostCurrentHealthTarget(Collider2D[] targets)
    {
        GameObject healthiestTarget = null;
        float mostHealth = float.MinValue;

        foreach (var target in targets)
        {
            if (target.CompareTag("Enemy"))
            {
                ICombatEntity combatEntity = target.GetComponent<ICombatEntity>();
                if (combatEntity != null && combatEntity.Health > mostHealth)
                {
                    healthiestTarget = target.gameObject;
                    mostHealth = combatEntity.Health;
                }
            }
        }
        return healthiestTarget;
    }
}
