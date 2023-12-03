using UnityEngine;


[CreateAssetMenu(fileName = "WeaponEvolutionBlueprint", menuName = "ScriptableObjects/WeaponEvolutionBlueprint")]
public class WeaponEvolutionBlueprint : ScriptableObject
{
    public WeaponSO baseWeaponData;
    public PassiveItemsSO catalystPassiveItemData;
    public WeaponSO evolvedWeaponData;
    public GameObject evolvedWeapon;
}
