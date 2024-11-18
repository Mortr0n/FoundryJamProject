using UnityEngine;
using System.Collections;

public class ProjectileBehavior : MonoBehaviour
{
    //private GameObject _target;
    //private ICombatEntity _owner;
    //private float _damage;
    //private DamageType _damageType;
    //public float Speed = 10f;

    //// Initialize the projectile 
    //public void Initialize(GameObject target, ICombatEntity owner, float damage, DamageType damageType)
    //{
    //    _target = target;
    //    _owner = owner;
    //    _damage = damage;
    //    _damageType = damageType;
    //}

    //private void Update()
    //{
    //    if (_target == null)
    //    {
    //        Destroy(gameObject); 
    //        return;
    //    }

    //    // Move toward the target
    //    Vector2 direction = (_target.transform.position - transform.position).normalized;
    //    //Debug.Log($"target pos: {_target.transform.position} am I moving? {transform.position} "); 
    //    transform.position += (Vector3)(direction * Speed * Time.deltaTime);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject == _target)
    //    {
    //        ICombatEntity targetEntity = collision.GetComponent<ICombatEntity>();
    //        if (targetEntity != null)
    //        {
    //            targetEntity.TakeDamage(_damage, _damageType);
    //            Debug.Log($"Projectile hit! Damage: {_damage}, Type: {_damageType}");
    //        }

    //        StartCoroutine(DestroyAfterDelay() );
    //    }
    //}

    //private IEnumerator DestroyAfterDelay()
    //{
    //    yield return new WaitForEndOfFrame();
    //    Destroy(gameObject);
    //}
}
