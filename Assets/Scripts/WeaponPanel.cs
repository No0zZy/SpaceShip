using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPanel : MonoBehaviour
{
    [SerializeField] private Text m_NumberText;
    [SerializeField] private Image m_WeaponImage;

    public void ActivatePanel(string number, Sprite weaponImage)
    {
        m_NumberText.text = number;
        m_WeaponImage.sprite = weaponImage;
    }
}
