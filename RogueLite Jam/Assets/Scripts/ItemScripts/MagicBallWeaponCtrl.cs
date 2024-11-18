using UnityEngine;

public class MagicBallWeaponCtrl : Weapon
{


    protected override ProjectileBase RangedAttack()
    {
        ProjectileBase projectile = base.RangedAttack();
        //Debug.Log($"Projectile inside RangedAttack: {projectile}");
        return projectile;
    }
}
