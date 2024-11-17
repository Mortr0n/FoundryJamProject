using UnityEngine;

public class MagicProjectileStaff : Weapon
{
    [SerializeField] private GameObject _projectilePrefab;

    public override void Attack(GameObject target)
    {
        if (_projectilePrefab == null)
        {
            Debug.LogError("projectile prefab is not assigned");
            return;
        }
        if (target == null)
        {
            Debug.LogWarning("No target found to attack!");
            return;
        }


        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity) as GameObject;

        MagicProjectile projectileController = projectile.GetComponent<MagicProjectile>();
        if (projectileController != null)
        {
            projectileController.Initialize(target, Owner);
        }
    }
}
