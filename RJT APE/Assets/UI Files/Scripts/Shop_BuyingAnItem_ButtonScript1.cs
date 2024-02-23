using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_BuyingAnItem_ButtonScript1 : MonoBehaviour
{
    public GameObject buyingAnItemButton;

    private void Start()
    {
        buyingAnItemButton.SetActive(false);
    }

    public void ToggleOnOff()
    {
        buyingAnItemButton.SetActive(true);
    }
}
