using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//数据类
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    public int maxHealth; //基础血量
    public int currHealth; //基础血量
    public int baseDefence; //基础防御
    public int currDefence; //基础防御
}
