using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Spinner : MonoBehaviour
{
    [SerializeField] List<Item> itemObjects = new List<Item>();
    [SerializeField] GameObject items;

    [SerializeField] GameObject giftWindow;
    [SerializeField] GameObject giftCount;

    [SerializeField] int stopSpinnerTimer;

    bool spinning;

    string giftName;

    int count;

    #region Private Methods
    void CreateItems()
    {
        for (int i = 0; i < itemObjects.Count; i++)
        {
            GameObject item = Instantiate(itemObjects[i].itemPrefab, items.transform.position, Quaternion.identity);
            item.transform.SetParent(items.transform);
            item.name = itemObjects[i].itemName;
            item.SetActive(false);
        }
        giftWindow.GetComponent<GiftWindow>().ReadySpinner();
    }

    void ShowGiftPrefab(string giftName)
    {
        for (int i = 0; i < items.transform.childCount; i++)
        {
            if (items.transform.GetChild(i).gameObject.name == giftName)
            {
                items.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                items.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Public Methods
    public void InitializeSpinner()
    {
        CreateItems();
    }

    public string GetGiftName()
    {
        return giftName;
    }

    public void StartSpinning()
    {
        spinning = true;

        StartCoroutine(TurnSpinner());
        StartCoroutine(StopSpinner());
    }

    public void ResetSpinner()
    {
        for (int i = 0; i < items.transform.childCount; i++)
        {
            Destroy(items.transform.GetChild(i).gameObject);
        }

        giftCount.SetActive(false);
        count = 0;
    }

    // Generate gift when spinning is complete
    public void GenerateGift()
    {
        List<string> allGifts = new List<string>();

        // Add all gifts to the pool and withdraw one, based on the chance
        for (int i = 0; i < itemObjects.Count; i++)
        {
            for (int j = 0; j < itemObjects[i].chanceCount; j++)
            {
                allGifts.Add(itemObjects[i].itemName);
            }
        }

        int randomGiftIndex = Random.Range(0, allGifts.Count);
        giftName = allGifts[randomGiftIndex];
        
        ShowGiftPrefab(giftName);
        giftWindow.GetComponent<GiftWindow>().CompleteSpinner(gameObject);
    }

    public void SetGiftCount(int _count)
    {
        count = _count;
        giftCount.SetActive(true);
        giftCount.GetComponent<Text>().text = count.ToString();
    }

    public int GetGiftCount()
    {
        return count;
    }
    #endregion

    #region Coroutines
    IEnumerator StopSpinner()
    {
        yield return new WaitForSeconds(stopSpinnerTimer);

        GenerateGift();
        spinning = false;
    }

    IEnumerator TurnSpinner()
    {
        int i = Random.Range(0, items.transform.childCount);

        if (spinning)
        {
            items.transform.GetChild(i).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);

        if (spinning)
        {
            items.transform.GetChild(i).gameObject.SetActive(false);
            StartCoroutine(TurnSpinner());
        }
    }
    #endregion
}
