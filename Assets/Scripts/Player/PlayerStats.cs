using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterSO characterData;

    private float currentHealth;
    private float currentRecovery;
    private float currentMoveSpeed;
    private float currentMight;
    private float currentProjectileSpeed;

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

    private void Awake()
    {
        currentHealth = characterData.MaxHealth;
        currentRecovery = characterData.Recovery;
        currentMoveSpeed = characterData.MoveSpeed;
        currentMight = characterData.Might;
        currentProjectileSpeed = characterData.ProjectileSpeed;
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
}
