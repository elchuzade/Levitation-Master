using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

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

    [SerializeField] GameObject prizeCount;
    [SerializeField] GameObject collectButton;
    [SerializeField] GameObject allPrizes;

    #region Public Methods
    // @access from ChestWindow canvas
    public void CollectPrize()
    {
        for (int i = 0; i < allPrizes.transform.childCount; i++)
        {
            allPrizes.transform.GetChild(i).GetComponent<ChestDropItem>().lastPrize = false;
            allPrizes.transform.GetChild(i).GetComponent<ChestDropItem>().MoveUp();
        }
    }

    // @access from Chest animation
    public void CreatePrize()
    {
        List<GameObject> allCoins = new List<GameObject>();
        List<GameObject> allDiamonds = new List<GameObject>();
        List<GameObject> allBoxes = new List<GameObject>();
        List<GameObject> allballs = new List<GameObject>();

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

        StartCoroutine(CreatePrize(0, 0.01f, allCoins, false));
        StartCoroutine(CreatePrize(2.5f, 0.01f, allDiamonds, false));
        StartCoroutine(CreatePrize(4, 0.4f, allBoxes, true));
    }
    #endregion

    #region Coroutines
    IEnumerator CreatePrize(float delay, float interval, List<GameObject> prefabs, bool lastPrize)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(InstantiateItem(interval, prefabs, lastPrize));
    }

    IEnumerator InstantiateItem(float interval, List<GameObject> prefabs, bool lastPrize)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            yield return new WaitForSeconds(interval);
            GameObject prize = Instantiate(prefabs[i], transform.position + new Vector3(0, 0, -10), Quaternion.identity);
            prize.GetComponent<ChestDropItem>().lastPrize = lastPrize;
            prize.transform.SetParent(allPrizes.transform);
        }
        prizeCount.GetComponent<Text>().text = "123";
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