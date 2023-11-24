using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterSO characterData;

    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;

    [SerializeField] private int exp = 0;
    [SerializeField] private int level = 1;
    [SerializeField] private int expCap = 100;

    [Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int expCapIncrease;
    }

    public float invincibleDur;
    float invincibleTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    public GameObject secondWeaponTest;
    public GameObject firstPITest;
    public GameObject secondPITest;

    private void Awake()
    {
        characterData = CharacterSelector.GetData();
        CharacterSelector.Instance.DestroySingleton();
        inventory = GetComponent<InventoryManager>();

        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
        currentMagnet = characterData.Magnet;

        SpawnWeapon(characterData.StartingWeapon);
        SpawnWeapon(secondWeaponTest);
        SpawnPassiveItem(firstPITest);
        SpawnPassiveItem(secondPITest);
    }

    private void Start()
    {
        expCap = levelRanges[0].expCapIncrease;
    }

    private void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
        Recover();
    }
    public void IncreaseExp(int amount)
    {
        exp += amount;
        LevelUpChecker();
    }
    private void LevelUpChecker()
    {
        if (exp >= expCap)
        {
            level++;
            exp -= expCap;
            int expCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    expCapIncrease = range.expCapIncrease;
                    break;
                }
            }
            expCap += expCapIncrease;
        }
    }
    public void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            invincibleTimer = invincibleDur;
            isInvincible = true;
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }
    public void Kill()
    {
        Debug.Log("player is dead");
    }
    public void RestoreHealth(float amount)
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
    private void Recover()
    {
        if (currentHealth < characterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
            if (currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }
        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Inventory slots are full");
            return;
        }
        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItems>());
        passiveItemIndex++;
    }
}
