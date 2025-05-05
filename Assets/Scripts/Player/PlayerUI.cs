using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private StaminaBar staminaBar;
    [SerializeField] private PlayerMotor playerMotor;
    

    // Start is called before the first frame update
    void Start()
    {
        staminaBar.SetMaxStamina(playerMotor.GetMaxStamina());
    }


    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
        staminaBar.SetStamina(playerMotor.GetStamina());
    }
}
