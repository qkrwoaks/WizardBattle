using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    [SerializeField] private WizardType wizardType;
    public WizardType WizardType { get { return wizardType; } }

    [SerializeField] private float wizardHp;    // 체력
    public float WizardHp { get { return wizardHp; } }


    [SerializeField] private float wizardShield;    // 방어막
    public float WizardShield { get { return wizardShield; } }


    [SerializeField] private float wizardDp;    // 방어력
    public float WizardDp { get { return wizardDp; } }


    [SerializeField] private float wizardtATK;    // 공격력
    public float WizardtATK { get { return wizardtATK; } }


    [SerializeField] private float decreaseCT; // 쿨타임 감소량
    public float DecreaseCT { get { return decreaseCT; } }


    [SerializeField] private float speed; // 속도
    public float Speed { get { return speed; } }


    [SerializeField] private float attackSpeed; // 공격속도
    public float AttackSpeed { get { return attackSpeed; } }
    
}