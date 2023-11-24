using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItems : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemsSO passiveItemsData;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApplyModifier();
    }

    protected virtual void ApplyModifier() { }
}
