using UnityEngine;

public interface ICombatEntity
{
    float Health { get; set; }
    float BaseDamage {  get; set; }
    float CritChance {  get; set; }
    float CritMultiple { get; set; }

    public void TakeDamage(float amount, DamageType damageType);
}
