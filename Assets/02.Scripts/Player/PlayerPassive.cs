using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    #region Variable (변수)

    [SerializeField] private PlayerManager player;             // 플레이어 스크립트를 담고 있는 변수
    [SerializeField] private ParticleSystem PassiveEffect;     // 패시브 이펙트 프리팹을 담고 있는 변수

    [SerializeField] private WizardType wizardType;

    #endregion

    #region Function (함수)

    public void Call()
    {
        if (player == null || PassiveEffect == null)            // 플레이어 스크립트 혹은 패시브 이펙트가 비어있을 때
        {
            return; // 종료
        }

        if (player.HP <= 50)                                    // 체력이 50%보다 작아질 때
        {
            PassiveEffect.Play();                               // 패시브 마법 실행

            switch (wizardType)
            {
                case WizardType.Fire:
                    player.ATK += 0.5f;         // 공격력 증가
                    break;
                case WizardType.Water:
                    player.DP += 0.5f;          // 방어력 증가
                    break;
                case WizardType.Grass:
                    player.decreaseCT += 0.5f;  // 쿨타임 감소
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}
