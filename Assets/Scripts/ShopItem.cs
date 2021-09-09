using UnityEngine;
using UnityEngine.UI;
using static GlobalVariables;

public class ShopItem : MonoBehaviour
{
    [SerializeField] int price;
    [SerializeField] int index;
    [SerializeField] int powerUp;


    [SerializeField] Currency currency;

    [SerializeField] GameObject priceTag;
    [SerializeField] Text priceText;
    [SerializeField] Text powerUpText;
    [SerializeField] GameObject diamondIcon;
    [SerializeField] GameObject coinIcon;
    [SerializeField] GameObject selectedText;
    [SerializeField] GameObject selectButton;
    [SerializeField] GameObject unlockButton;

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
        selectedText.SetActive(false);
        selectButton.SetActive(false);
    }

    #region Public Methods
    // @access from ShopStatus
    public void SelectItem()
    {
        selectedText.SetActive(true);
        selectButton.SetActive(false);
        unlockButton.SetActive(false);
    }

    // @access from ShopStatus
    public void UnlockItem()
    {
        selectedText.SetActive(false);
        selectButton.SetActive(true);
        priceTag.SetActive(false);
        unlockButton.SetActive(false);
    }

    // @access from ShopStatus
    public void DeselectItem()
    {
        selectedText.SetActive(false);
        selectButton.SetActive(true);
    }

    // @access from ShopStatus
    public Currency GetCurrency()
    {
        return currency;
    }

    // @access from ShopStatus
    public int GetPrice()
    {
        return price;
    }

    // @access from ShopStatus
    public int GetIndex()
    {
        return index;
    }
    #endregion
}
