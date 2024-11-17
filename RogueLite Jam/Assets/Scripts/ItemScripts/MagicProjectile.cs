using UnityEngine;

public class MagicProjectile : Weapon
{
    [SerializeField] private GameObject _projectilePrefab;

    public override void Attack(GameObject target)
    {
        if (_projectilePrefab == null)
        {
            Debug.LogError("projectile prefab is not assigned");
            return;
        }

        GameObject projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity) as GameObject;

        MagicProjectile projectileController = projectile.GetComponent<MagicProjectile>();
        if (projectileController != null)
        {
            projectileController.Initialize(Owner, 15f, 1.5f, 10f, DamageType.Magic);
        }
    }
}
