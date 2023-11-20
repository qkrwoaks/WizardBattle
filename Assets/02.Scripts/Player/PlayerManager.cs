using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    #region Variable(����)
    public PlayerData playerData;

    public PhotonView PV;
    public SkillController skillController;

    public WizardType playerType;                   // �÷��̾� Ÿ��

    public float HP;                                // ü��
    public float shield;                            // ��
    public float DP;                                // ����
    public float ATK;                               // ���ݷ�
    public float decreaseCT;                        // ��Ÿ�� ���ҷ�
    public float attackSpeed;                       // ���ݼӵ�
    public float speed;                             // �ӵ�

    public bool isGodMode;                          // �������� Ȯ���ϴ� ����

    public UnityEvent HitEvent;                     // �¾��� �� �̺�Ʈ
    public UnityEvent DeadEvent;                    // �׾��� �� �̺�Ʈ

    [Header("Soling")]
    public bool isSolingAttack;                     // �ָ� ������ ���� ��Ȳ

    public Transform weaponTr;                      // ���� Transform

    [SerializeField] private GameObject[] playerModel;

    [Header("Sound")]
    [SerializeField] private AudioClip hitSound;
    #endregion

    #region Function (�Լ�)
    private void Awake()
    {
        if (skillController == null)
        {
            skillController = GetComponent<SkillController>();
        }
        if (PV == null)
        {
            PV = GetComponent<PhotonView>();
        }

        InitPlayer();   // �÷��̾� �ʱ�ȭ
    }

    private void Start()
    {
        InitData();

        if (PV.IsMine == true)
        {
            transform.GetComponent<SkillController>().InitSkillCoolTime();
            GameSystem.Instance.skillCoolTimeUI.Init_UI();
        }
    }

    /// <summary>
    /// �÷��̾� �ʱ�ȭ
    /// </summary>
    private void InitPlayer()
    {
        if (PV.IsMine == true)
        {
            GameSystem.Instance.player = this.gameObject;
            GameSystem.Instance.plyerWeaponTr = this.weaponTr;
            GameSystem.Instance.playerManager = this;
        }
        else
        {
            GameSystem.Instance.enemy = this.gameObject;
            GameSystem.Instance.enemyWeaponTr = this.weaponTr;
            GameSystem.Instance.enemyManager = this;
        }
    }

    /// <summary>
    /// �÷��̾��� �����Ͱ��� �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    private void InitData()
    {
        playerType = playerData.WizardType;  // �̸�
        HP = playerData.WizardHp;            // ü��
        shield = playerData.WizardShield;    // ��
        DP = playerData.WizardDp;            // ����
        ATK = playerData.WizardtATK;         // ������
        decreaseCT = playerData.DecreaseCT;  // ��Ÿ�� ���ҷ�
        attackSpeed = playerData.AttackSpeed;// ���ݼӵ�
        speed = playerData.Speed;            // �ӵ�
    }

    /// <summary>
    /// �¾��� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    public void Hit(float damage)
    {
        if (HP <= 0) return;

        SoundManager.Instance.SFXPlay(hitSound.name, hitSound);

        PV.RPC("HitRPC", RpcTarget.All, damage);
    }

    [PunRPC]
    public void HitRPC(float damage)
    {
        float dp = 0;
        // ���� ����ȭ �� �� !
        if (PV.IsMine)
        {
            StartCoroutine(GameSystem.Instance.ShowBloodScreen());
            dp = GameSystem.Instance.playerManager.DP;
        }
        else
        {
            dp = GameSystem.Instance.enemyManager.DP;
        }

        damage *= (100 / (100 + dp));   // ���� ���

        if ((PV.IsMine == true ? isGodMode : GameSystem.Instance.enemyManager.isGodMode) == false)     // ������ �ƴҶ�
        {
            HP -= damage;           // ������ ����ŭ ü�� ����

            HitEvent?.Invoke();     // HitEvent ����

            if (HP <= 0)
            {
                GameSystem.Instance.outcomeUIController.ShowOutcomeUI(PV.IsMine == true ? false : true);
            }
        }
    }

    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
