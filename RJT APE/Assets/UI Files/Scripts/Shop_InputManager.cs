using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_InputManager : MonoBehaviour
{
    public GameObject buyingAnItemButton;
    public GameObject shopByItemButton;
    public GameObject shopByItemButton_BuyingAnItem;


    private bool buyingAnItemOn;
    private bool shopByItemOn;
    private bool shopByItemBuyingAnItemOn;


    private void Start()
    {
        buyingAnItemButton.SetActive(false);
        shopByItemButton.SetActive(false);
        //shopByItemButton_BuyingAnItem.SetActive(false);     (temporarily commented)

        buyingAnItemOn = false;
        shopByItemOn = false;
        shopByItemBuyingAnItemOn = false;
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && buyingAnItemOn == true)
        {
            buyingAnItemButton.SetActive(false);
            buyingAnItemOn = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && shopByItemBuyingAnItemOn == true)
        {
            shopByItemButton_BuyingAnItem.SetActive(false);
            shopByItemBuyingAnItemOn = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && shopByItemOn)
        {
            shopByItemButton.SetActive(false);
            shopByItemOn = false;
        }
    }

    public void ToggleOnOff_BuyingAnItem()
    {
        buyingAnItemButton.SetActive(true);

        buyingAnItemOn = true;
    }

    public void ToggleOnOff_ShopByItem()
    {
        shopByItemButton.SetActive(true);

        shopByItemOn = true;
    }

    public void ToggleOnOff_ShopByItem_BuyingAnItem()
    {
        shopByItemButton_BuyingAnItem.SetActive(true);
        shopByItemBuyingAnItemOn = true;
    }
}
