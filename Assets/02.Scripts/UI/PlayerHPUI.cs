using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider;
    [SerializeField]
    private TMP_Text hpText;

    private float maxHp;

    private void Start()
    {
        maxHp = GameSystem.Instance.playerManager.playerData.WizardHp;
    }

    private void Update()
    {
        hpSlider.value = Mathf.Lerp(hpSlider.value, GameSystem.Instance.playerManager.HP / maxHp, Time.deltaTime * 5f);
        hpText.text = Mathf.Round(GameSystem.Instance.playerManager.HP).ToString() + "/100";
    }
}