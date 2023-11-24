using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemsSO", menuName = "ScriptableObjects/Passive Items")]
public class PassiveItemsSO : ScriptableObject
{
    [SerializeField] private float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }
}
