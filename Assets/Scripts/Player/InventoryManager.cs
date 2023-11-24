using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<PassiveItems> passiveItemSlots = new List<PassiveItems>(6);
    public int[] passiveItemLevels = new int[6];

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
    }
    public void AddPassiveItem(int slotIndex, PassiveItems passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
    }
    public void LevelUpWeapon(int slotIndex)
    {

    }
    public void LevelUpPassiveItem(int slotIndex)
    {

    }
}
