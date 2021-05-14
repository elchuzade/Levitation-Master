using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static GlobalVariables;

public class ChestWindow : MonoBehaviour
{
    [SerializeField] GameObject redChest;
    [SerializeField] GameObject goldChest;
    [SerializeField] GameObject silverChest;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject diamondPrefab;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject speedPrefab;
    [SerializeField] GameObject[] allBallPrefabs;

    [SerializeField] GameObject prizeCount;
    [SerializeField] GameObject collectButton;
    [SerializeField] GameObject allPrizes;

    int coinsAmount;
    int diamondsAmount;

    #region Public Methods
    // @access from ChestWindow canvas
    public void CollectPrize()
    { 
        for (int i = 0; i < allPrizes.transform.childCount; i++)
        {
            ChestDropItem prizeItemScript = allPrizes.transform.GetChild(i).GetComponent<ChestDropItem>();
            if (prizeItemScript != null)
            {
                prizeItemScript.lastPrize = false;
                prizeItemScript.MoveUp();
            }
            ChestBallItem ballItemScript = allPrizes.transform.GetChild(i).GetComponent<ChestBallItem>();
            if (ballItemScript != null)
            {
                ballItemScript.MoveUp();
            }
        }
    }

    // @access from Chest animation
    public void CreatePrize()
    {
        List<GameObject> allCoins = new List<GameObject>();
        List<GameObject> allDiamonds = new List<GameObject>();
        List<GameObject> allBoxes = new List<GameObject>();
        List<GameObject> allBalls = new List<GameObject>();

        coinsAmount = 55;
        diamondsAmount = 15;

        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);
        allCoins.Add(coinPrefab);

        allDiamonds.Add(diamondPrefab);
        allDiamonds.Add(diamondPrefab);
        allDiamonds.Add(diamondPrefab);
        allDiamonds.Add(diamondPrefab);
        allDiamonds.Add(diamondPrefab);
        allDiamonds.Add(diamondPrefab);

        allBoxes.Add(speedPrefab);
        allBoxes.Add(bulletPrefab);
        allBoxes.Add(lightningPrefab);
        allBoxes.Add(shieldPrefab);

        allBalls.Add(allBallPrefabs[0]);

        StartCoroutine(CreatePrize(0, 0.01f, allCoins, false, ChestPrizeTypes.Coin));
        StartCoroutine(CreatePrize(2.5f, 0.01f, allDiamonds, false, ChestPrizeTypes.Diamond));
        StartCoroutine(CreatePrize(4, 0.4f, allBoxes, false, ChestPrizeTypes.Box));
        StartCoroutine(CreatePrize(8, 0, allBalls, true, ChestPrizeTypes.Ball));
    }
    #endregion

    #region Coroutines
    IEnumerator CreatePrize(float delay, float interval, List<GameObject> prefabs, bool lastPrize, ChestPrizeTypes prizeType)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(InstantiateItem(interval, prefabs, lastPrize, prizeType));
    }

    IEnumerator InstantiateItem(float interval, List<GameObject> prefabs, bool lastPrize, ChestPrizeTypes prizeType)
    {
        if (prizeType == ChestPrizeTypes.Ball)
        {
            yield return new WaitForSeconds(interval);
            GameObject prize = Instantiate(prefabs[0], transform.position + new Vector3(0, 0, -50), Quaternion.identity);
            prize.transform.localScale *= 0.4f;
            prize.transform.SetParent(allPrizes.transform);
            prize.GetComponent<ChestBallItem>().MoveUp();
        }
        else
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                yield return new WaitForSeconds(interval);
                GameObject prize = Instantiate(prefabs[i], transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                prize.transform.SetParent(allPrizes.transform);
                prize.GetComponent<ChestDropItem>().lastPrize = lastPrize;
            }
            if (prizeType == ChestPrizeTypes.Coin)
            {
                prizeCount.SetActive(true);
                prizeCount.GetComponent<Text>().text = coinsAmount.ToString();
            }
            else if (prizeType == ChestPrizeTypes.Diamond)
            {
                prizeCount.SetActive(true);
                prizeCount.GetComponent<Text>().text = diamondsAmount.ToString();
            }
            else
            {
                prizeCount.SetActive(false);
            }
        }

        if (lastPrize)
        {
            StartCoroutine(ActivateCollectButton());
        }
    }

    IEnumerator ActivateCollectButton()
    {
        yield return new WaitForSeconds(1);
        collectButton.SetActive(true);
    }
    #endregion
}