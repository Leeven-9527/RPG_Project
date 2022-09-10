using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateData;
    public AttackData_SO attackData;

    [HideInInspector]
    public CharacterData_SO characterData;
    public bool isCritical;

    public event Action<int, int> UpdateHealthBarOnAttack;

    void Awake() {
        if(templateData != null) {
            characterData = Instantiate(templateData);
        }
    }

    #region Read form Data_SO
    public int MaxHealth
    {
        get{ if (characterData != null) return characterData.maxHealth; else return 0;}
        set{ characterData.maxHealth = value; }
    }

    public int CurrHealth
    {
        get { if (characterData != null) return characterData.currHealth; else return 0; }
        set { characterData.currHealth = value; }
    }

    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }

    public int currDefence
    {
        get { if (characterData != null) return characterData.currDefence; else return 0; }
        set { characterData.currDefence = value; }
    }
    #endregion

    #region
    public void TakeDamage(CharacterStats attacter, CharacterStats defener)
    {
        int damage = Mathf.Max(attacter.CurrentDamage() - defener.currDefence, 0);
        CurrHealth = Mathf.Max(CurrHealth - damage, 0);

        if(attacter.isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //Debug.Log("�Ե�������˺���" + damage);
        //TODO: Updete UI ����Update
        UpdateHealthBarOnAttack?.Invoke(CurrHealth, MaxHealth);
    }

    public void TakeDamage(int damage, CharacterStats defener)
    {
        int currDamage = Mathf.Max(damage - defener.currDefence, 0);
        CurrHealth = Mathf.Max(CurrHealth  - damage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrHealth, MaxHealth);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        if(isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }
    #endregion
}
