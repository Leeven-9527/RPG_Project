using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    public int maxHealth; //����Ѫ��
    public int currHealth; //����Ѫ��
    public int baseDefence; //��������
    public int currDefence; //��������
}
