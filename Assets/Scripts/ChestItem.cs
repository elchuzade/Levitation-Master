using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static GlobalVariables;

public class ChestItem : MonoBehaviour
{
    [SerializeField] ChestColors chestColor;
    [SerializeField] Text chestCount;
    [SerializeField] Text chestPrice;

    [Header("Diamond reward")]
    [SerializeField] int diamondChance;
    [SerializeField] int diamondMin;
    [SerializeField] int diamondMax;
    [Header("Coin reward")]
    [SerializeField] int coinChance;
    [SerializeField] int coinMin;
    [SerializeField] int coinMax;

    List<Rewards> allRewards = new List<Rewards>();

    void Start()
    {
        BuildAllRewards();
    }

    #region Private Methods
    void BuildAllRewards()
    {
        // Add all the possible rewards into the basket of all rewards
        for (int i = 0; i < diamondChance; i++)
        {
            allRewards.Add(Rewards.Diamond);
        }
        for (int i = 0; i < coinChance; i++)
        {
            allRewards.Add(Rewards.Coin);
        }
    }
    #endregion

    #region Public Methods
    // @access from ChestsStatus
    public void DeselectChest()
    {
        transform.Find("ChestSelect").GetComponent<AnimationTrigger>().Trigger("Deselect");
    }

    // @access from ChestsStatus
    public void SetCount(int count)
    {
        chestCount.text = count.ToString();
        // Hide price tag if there are some chests
        if (count > 0)
        {
            chestPrice.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            chestPrice.transform.parent.gameObject.SetActive(true);
        }
    }

    // @access from ChestsStatus
    public void SetPrice(int price)
    {
        chestPrice.text = price.ToString();
    }

    // @access from OpenedChestView
    public List<Rewards> GetAllRewards()
    {
        return allRewards;
    }
    // @access from OpenedChestView
    public int GetRewardCount(Rewards reward)
    {
        int result = 0;
        switch (reward)
        {
            case Rewards.Diamond:
                result = Random.Range(diamondMin, diamondMax);
                break;
            case Rewards.Coin:
                result = Random.Range(coinMin, coinMax);
                break;
        }
        return result;
    }
    #endregion
}
