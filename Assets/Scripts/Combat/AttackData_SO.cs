using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������
[CreateAssetMenu(fileName = "New Data", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    public float shortRange; //��ս��������
    public float longRanges; //Զ�̹�������
    public float coolDown; //����CD
    public float minDamge; //��С������ֵ
    public float maxDamge; //��󹥻���ֵ

    public float criticalMultiplier; //�����ӳɰٷֱ�
    public float criticalChance; //������
}
