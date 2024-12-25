using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    [SerializeField] private Text _amountText;
    [SerializeField] private Image _cooldownImage;

    public void UpdateAmountText(int amount) => _amountText.text = amount.ToString();
    public void UpdateCooldownImage(float fillAmount) => _cooldownImage.fillAmount = fillAmount;
}