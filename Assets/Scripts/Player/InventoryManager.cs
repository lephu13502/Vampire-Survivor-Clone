using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);
    public List<PassiveItems> passiveItemSlots = new List<PassiveItems>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);

    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject initialWeapon;
        public WeaponSO weaponData;
    }
    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public GameObject initialPassiveItem;
        public PassiveItemsSO passiveItemsData;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public Text upgradeNameDisplay;
        public Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>();
    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();

    PlayerStats player;

    private void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon)
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        if (GameManager.Instance != null && GameManager.Instance.choosingUpgrade)
        {
            GameManager.Instance.EndLevelUp();
        }
    }
    public void AddPassiveItem(int slotIndex, PassiveItems passiveItem)
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemsData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemsData.Icon;

        if (GameManager.Instance != null && GameManager.Instance.choosingUpgrade)
        {
            GameManager.Instance.EndLevelUp();
        }
    }
    public void LevelUpWeapon(int slotIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("No next level for" + weapon.name);
                return;
            }
            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform);
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;
            if (GameManager.Instance != null && GameManager.Instance.choosingUpgrade)
            {
                GameManager.Instance.EndLevelUp();
            }
        }
    }
    public void LevelUpPassiveItem(int slotIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItems passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemsData.NextLevelPrefab)
            {
                Debug.LogError("No next level for" + passiveItem.name);
                return;
            }
            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemsData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform);
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItems>());
            Destroy(passiveItem.gameObject);
            passiveItemLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItems>().passiveItemsData.Level;
            if (GameManager.Instance != null && GameManager.Instance.choosingUpgrade)
            {
                GameManager.Instance.EndLevelUp();
            }
        }
    }
    private void ApplyUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            int upgradeType = Random.Range(1, 3);
            if (upgradeType == 1)
            {
                WeaponUpgrade chosenWeaponUpgrade = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];
                if (chosenWeaponUpgrade != null)
                {
                    bool newWeapon = false;
                    for (int i = 0; i < weaponSlots.Count; i++)
                    {
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false;
                            if (!newWeapon)
                            {
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i));
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newWeapon = true;
                        }
                    }
                    if (newWeapon)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                }
            }
            else if (upgradeType == 2)
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = passiveItemUpgradeOptions[Random.Range(0, passiveItemUpgradeOptions.Count)];
                if (chosenPassiveItemUpgrade != null)
                {
                    bool newPassiveItem = false;
                    for (int i = 0; i < passiveItemSlots.Count; i++)
                    {
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemsData == chosenPassiveItemUpgrade.passiveItemsData)
                        {
                            newPassiveItem = false;
                            if (!newPassiveItem)
                            {
                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i));
                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemsData.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemsData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemsData.NextLevelPrefab.GetComponent<PassiveItems>().passiveItemsData.Name;
                            }
                            break;
                        }
                        else
                        {
                            newPassiveItem = true;
                        }
                    }
                    if (newPassiveItem)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem));
                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemsData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemsData.Name;
                    }
                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemsData.Icon;
                }
            }
        }
    }
    private void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
        }
    }

    public void RemoveAndApplyUpgrade()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }
}
