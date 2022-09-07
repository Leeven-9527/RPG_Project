using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//数据类
[CreateAssetMenu(fileName = "New Data", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float shortRange; //近战攻击距离
    public float longRanges; //远程攻击距离
    public float coolDown; //攻击CD
    public float minDamge; //最小攻击数值
    public float maxDamge; //最大攻击数值

    public float criticalMultiplier; //暴击加成百分比
    public float criticalChance; //暴击率
}
