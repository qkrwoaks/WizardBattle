using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 0)]

public class PlayerData : ScriptableObject
{
    [SerializeField] private WizardType wizardType;
    public WizardType WizardType { get { return wizardType; } }

    [SerializeField] private float wizardHp;    // ü��
    public float WizardHp { get { return wizardHp; } }


    [SerializeField] private float wizardShield;    // ��
    public float WizardShield { get { return wizardShield; } }


    [SerializeField] private float wizardDp;    // ����
    public float WizardDp { get { return wizardDp; } }


    [SerializeField] private float wizardtATK;    // ���ݷ�
    public float WizardtATK { get { return wizardtATK; } }


    [SerializeField] private float decreaseCT; // ��Ÿ�� ���ҷ�
    public float DecreaseCT { get { return decreaseCT; } }


    [SerializeField] private float speed; // �ӵ�
    public float Speed { get { return speed; } }


    [SerializeField] private float attackSpeed; // ���ݼӵ�
    public float AttackSpeed { get { return attackSpeed; } }
    
}