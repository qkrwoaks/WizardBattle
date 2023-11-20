using UnityEngine;

public class PlayerPassive : MonoBehaviour
{
    #region Variable (����)

    [SerializeField] private PlayerManager player;             // �÷��̾� ��ũ��Ʈ�� ��� �ִ� ����
    [SerializeField] private ParticleSystem PassiveEffect;     // �нú� ����Ʈ �������� ��� �ִ� ����

    [SerializeField] private WizardType wizardType;

    #endregion

    #region Function (�Լ�)

    public void Call()
    {
        if (player == null || PassiveEffect == null)            // �÷��̾� ��ũ��Ʈ Ȥ�� �нú� ����Ʈ�� ������� ��
        {
            return; // ����
        }

        if (player.HP <= 50)                                    // ü���� 50%���� �۾��� ��
        {
            PassiveEffect.Play();                               // �нú� ���� ����

            switch (wizardType)
            {
                case WizardType.Fire:
                    player.ATK += 0.5f;         // ���ݷ� ����
                    break;
                case WizardType.Water:
                    player.DP += 0.5f;          // ���� ����
                    break;
                case WizardType.Grass:
                    player.decreaseCT += 0.5f;  // ��Ÿ�� ����
                    break;
                default:
                    break;
            }
        }
    }
    #endregion
}
