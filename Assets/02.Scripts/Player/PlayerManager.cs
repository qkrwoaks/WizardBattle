using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    #region Variable(변수)
    public PlayerData playerData;

    public PhotonView PV;
    public SkillController skillController;

    public WizardType playerType;                   // 플레이어 타입

    public float HP;                                // 체력
    public float shield;                            // 방어막
    public float DP;                                // 방어력
    public float ATK;                               // 공격력
    public float decreaseCT;                        // 쿨타임 감소량
    public float attackSpeed;                       // 공격속도
    public float speed;                             // 속도

    public bool isGodMode;                          // 무적인지 확인하는 변수

    public UnityEvent HitEvent;                     // 맞았을 때 이벤트
    public UnityEvent DeadEvent;                    // 죽었을 때 이벤트

    [Header("Soling")]
    public bool isSolingAttack;                     // 솔링 공격을 받은 상황

    public Transform weaponTr;                      // 무기 Transform

    [SerializeField] private GameObject[] playerModel;

    [Header("Sound")]
    [SerializeField] private AudioClip hitSound;
    #endregion

    #region Function (함수)
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

        InitPlayer();   // 플레이어 초기화
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
    /// 플레이어 초기화
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
    /// 플레이어의 데이터값을 초기화하는 함수
    /// </summary>
    private void InitData()
    {
        playerType = playerData.WizardType;  // 이름
        HP = playerData.WizardHp;            // 체력
        shield = playerData.WizardShield;    // 방어막
        DP = playerData.WizardDp;            // 방어력
        ATK = playerData.WizardtATK;         // 마법사
        decreaseCT = playerData.DecreaseCT;  // 쿨타임 감소량
        attackSpeed = playerData.AttackSpeed;// 공격속도
        speed = playerData.Speed;            // 속도
    }

    /// <summary>
    /// 맞았을 때 실행되는 함수
    /// </summary>
    /// <param name="damage">맞은 데미지 값</param>
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
        // 방어력 동기화 할 것 !
        if (PV.IsMine)
        {
            StartCoroutine(GameSystem.Instance.ShowBloodScreen());
            dp = GameSystem.Instance.playerManager.DP;
        }
        else
        {
            dp = GameSystem.Instance.enemyManager.DP;
        }

        damage *= (100 / (100 + dp));   // 방어력 계산

        if ((PV.IsMine == true ? isGodMode : GameSystem.Instance.enemyManager.isGodMode) == false)     // 무적이 아닐때
        {
            HP -= damage;           // 데미지 값만큼 체력 감소

            HitEvent?.Invoke();     // HitEvent 실행

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
