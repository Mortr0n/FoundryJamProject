using UnityEngine;

public class MagicProjectile : Weapon
{
    private GameObject _target;
    private float _speed = 10f;

    public void Initialize(GameObject target, ICombatEntity owner)
    {
        Owner = owner; // Set the weapon's owner (e.g., player)
        _target = target; // Set the projectile's target
    }

    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject); // Destroy the projectile if the target is gone
            return;
        }

        // Move toward the target
        transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

        // Check if close enough to hit
        if (Vector2.Distance(transform.position, _target.transform.position) < 0.1f)
        {
            Attack(_target); // Call the Attack method from Weapon
            Destroy(gameObject); // Destroy the projectile after attacking
        }
    }

    public override void Attack(GameObject target)
    {
        
            Attack(target);
            
        
    }
}
