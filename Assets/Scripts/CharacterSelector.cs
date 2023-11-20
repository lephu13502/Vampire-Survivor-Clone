using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance;
    public CharacterSO characterData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("extra" + this + "deleted");
            Destroy(gameObject);
        }
    }

    public static CharacterSO GetData()
    {
        return Instance.characterData;
    }

    public void SelectCharacter(CharacterSO character)
    {
        characterData = character;
    }

    public void DestroySingleton()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
