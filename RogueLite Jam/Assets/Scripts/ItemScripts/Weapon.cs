using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected ICombatEntity Owner { get;  set; }

    private float _range = 5f;
    private float _cooldown = 1f;
    private float _damageAdd = 2f;
    private bool _canAttack = true;
    private DamageType _damageType = DamageType.Physical;
    private TargetType _targetType = TargetType.Closest;


    

    public virtual void Initialize(ICombatEntity owner, float attackPower, float cooldown, float range, DamageType damageType)
    {
        Owner = owner;
        _range = range;
        _cooldown = cooldown;
        _damageAdd = attackPower;
        _damageType = damageType;

        if (_canAttack)
        {
            StartCoroutine(AutoAttack());
        }
    }


    public IEnumerator AutoAttack()
    {
        Debug.Log("running autoattack");
        GameObject target = GetTarget(_targetType);
        if (target != null)
        {
            Attack(target);
        }
        yield return new WaitForSeconds(_cooldown);
    }

    public virtual void Attack(GameObject target)
    {
        float modifiedDamage = Owner.BaseDamage;
        if (Owner != null)  
        { 
            modifiedDamage += _damageAdd;
            Debug.Log($"modDmg: {modifiedDamage}");
        }

        if (DidItCrit()) 
        { 
            modifiedDamage = AddCritDamage(modifiedDamage);
            Debug.Log($"modDmg: {modifiedDamage}");
        }

        
        Owner.TakeDamage(modifiedDamage, _damageType);
    }

    public virtual bool DidItCrit()
    {
        return Random.Range(0, 1000) < Owner.CritChance;
    }

    public virtual float AddCritDamage(float incDamage)
    {
        return incDamage *= Owner.CritMultiple;
    }

    public virtual GameObject GetTarget(TargetType targetModifier)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, _range);
        GameObject acquiredTarget = null;

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
        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;
        foreach (var target in targets)
        {
            if (target.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.gameObject;
                }
            }
        }
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
