using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class ShopItem : MonoBehaviour
{
    [SerializeField] int price;
    [SerializeField] int index;

    [SerializeField] Currency currency;

    [SerializeField] GameObject priceTag;
    [SerializeField] Text priceText;
    [SerializeField] GameObject diamondIcon;
    [SerializeField] GameObject coinIcon;
    [SerializeField] GameObject select;

    void Start()
    {
        if (currency == Currency.Coin)
        {
            diamondIcon.SetActive(false);
        } else
        {
            coinIcon.SetActive(false);
        }
        priceText.text = price.ToString();
    }

    public void SelectItem()
    {
        select.SetActive(true);
    }

    public void BuyItem()
    {
        priceTag.SetActive(false);
    }

    public void DeselectItem()
    {
        select.SetActive(false);
    }

    public Currency GetCurrency()
    {
        return currency;
    }

    public int GetPrice()
    {
        return price;
    }
}
