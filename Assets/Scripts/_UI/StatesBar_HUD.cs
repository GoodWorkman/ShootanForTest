using UnityEngine;
using UnityEngine.UI;

public class StatesBar_HUD : StatesBar
{
    [SerializeField] protected Text percentText;

    private void SetPercentText()
    {
        percentText.text = targetFillAmount.ToString("P0");
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPercentText();
    }

    public override void UpdateStates(float currentValue, float maxValue)
    {
        base.UpdateStates(currentValue, maxValue);
        SetPercentText();
    }
}