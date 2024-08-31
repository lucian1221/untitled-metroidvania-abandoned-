using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public GameObject spellPrefab;
    public int damage;
    public float energyCost;
    public float speed;
    public float lifetime;
    public float cooldown;
}
