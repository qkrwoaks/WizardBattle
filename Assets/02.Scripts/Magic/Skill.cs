using Photon.Pun;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected PhotonView PV;
    public SkillData _skillStats;

    protected float _ATK;
    public float ATK
    {
        get { return _ATK; }
        private set { }
    }

    protected float _speed;
    protected float _duration;
    public AudioClip _SFX;
    public Transform _SFXTransform;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PV = GetComponent<PhotonView>(); 
    }

    protected virtual void Start()
    {
        InitStats();

        if (PV.IsMine == false) 
        {
            return;
        }

        UseSkill();
    }

    private void InitStats()
    {
        if( _skillStats != null)
        {
            _ATK = _skillStats.SkillATK;
            _speed = _skillStats.SkillSpeed;
            _duration = _skillStats.SkillDuration;
            //_SFX = _skillStats.SkillSFX[0];
        }
    }

    public Transform PlayerTrInit()
    {
        return PV.IsMine? GameSystem.Instance.player.transform : GameSystem.Instance.enemy.transform;
    }

    protected abstract void UseSkill();

    protected void DestroyObject()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}