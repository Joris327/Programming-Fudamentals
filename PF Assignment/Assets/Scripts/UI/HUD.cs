using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pickupText;

    void Update()
    {
        pickupText.text = GameManager.Instance.PickupsCount.ToString();
    }
}
