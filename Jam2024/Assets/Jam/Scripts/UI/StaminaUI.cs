using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _dangerColor;
    [SerializeField] private Color _rechargeColor;
    [SerializeField] private float _dangerThreshold = 0.3f;

    [SerializeField] private Image _staminaBar;

    [SerializeField] private CharacterMovement _character;
    private void Start()
    {
        _character.OnStaminaChange += UpdateStamina;

        UpdateStamina(1, false);
    }

    private void UpdateStamina(float stamina, bool recharging)
    {
        _staminaBar.transform.localScale = new Vector3(stamina, 1, 1);

        if (recharging && stamina != 1)
        {
            _staminaBar.color = _rechargeColor;
        }
        else if (stamina <= _dangerThreshold)
        {
            _staminaBar.color = _dangerColor;
        }
        else
        {
            _staminaBar.color = _defaultColor;
        }
        
    }
}
