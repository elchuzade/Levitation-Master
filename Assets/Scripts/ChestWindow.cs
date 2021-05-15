using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static GlobalVariables;

public class ChestWindow : MonoBehaviour
{
    Player player;

    [SerializeField] ParticleSystem prizeParticles;

    [SerializeField] GameObject redChest;
    [SerializeField] GameObject goldChest;
    [SerializeField] GameObject silverChest;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject diamondPrefab;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject lightningPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject speedPrefab;

    [SerializeField] GameObject[] allCommonBallPrefabs;
    [SerializeField] GameObject[] allRareBallPrefabs;
    [SerializeField] GameObject[] allLegendaryBallPrefabs;

    [SerializeField] GameObject prizeCount;
    [SerializeField] GameObject collectButton;
    [SerializeField] GameObject allPrizes;

    GameObject openedChest;

    int coinAmount;
    int diamondAmount;
    int speedAmount;
    int bulletAmount;
    int lightningAmount;
    int shieldAmount;

    void Start()
    {
        player = FindObjectOfType<Player>();

        player.LoadPlayer();
    }

    #region Public Methods
    // @access from ChestStatus script
    public void OpenChest(ChestColors color)
    {
        switch (color)
        {
            case ChestColors.Red:
                redChest.SetActive(true);
                openedChest = redChest;
                break;
            case ChestColors.Gold:
                goldChest.SetActive(true);
                openedChest = goldChest;
                break;
            case ChestColors.Silver:
                silverChest.SetActive(true);
                openedChest = silverChest;
                break;
        }
    }

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
            BallItem ballItemScript = allPrizes.transform.GetChild(i).GetComponent<BallItem>();
            if (ballItemScript != null)
            {
                ballItemScript.MoveUp();
            }
        }

        player.coins += coinAmount;
        player.diamonds += diamondAmount;
        player.bulletCount += bulletAmount;
        player.speedCount += speedAmount;
        player.lightningCount += lightningAmount;
        player.shieldCount += shieldAmount;

        player.SavePlayer();
        StartCoroutine(HideChestWindow());
    }

    // @access from Chest animation
    public void CreatePrize()
    {
        List<GameObject> allCoins = new List<GameObject>();
        List<GameObject> allDiamonds = new List<GameObject>();
        List<GameObject> allSkills = new List<GameObject>();
        List<GameObject> allBalls = new List<GameObject>();

        ChestUnlock chestUnlock = openedChest.GetComponent<ChestUnlock>();
        int spawnCoinAmount = Random.Range(chestUnlock.coinMin, chestUnlock.coinMax);
        int spawnDiamondAmount = Random.Range(chestUnlock.diamondMin, chestUnlock.diamondMax);
        int spawnSkillAmount = Random.Range(chestUnlock.skillMin, chestUnlock.skillMax);
        int spawnBallAmount = Random.Range(chestUnlock.ballMin, chestUnlock.ballMax);

        coinAmount = spawnCoinAmount;
        diamondAmount = spawnDiamondAmount;

        for (int i = 0; i < Mathf.Clamp(spawnCoinAmount, 0, 12); i++)
        {
            allCoins.Add(coinPrefab);
        }
        for (int i = 0; i < Mathf.Clamp(spawnDiamondAmount, 0, 8); i++)
        {
            allDiamonds.Add(diamondPrefab);
        }
        for (int i = 0; i < spawnSkillAmount; i++)
        {
            GameObject[] skillPool = new GameObject[] { speedPrefab, bulletPrefab, lightningPrefab, shieldPrefab };
            int randomSkillIndex = Random.Range(0, 3);
            allSkills.Add(skillPool[randomSkillIndex]);
            // Add counts to player data
            if (randomSkillIndex == 0)
            {
                speedAmount++;
            } else if (randomSkillIndex == 1)
            {
                bulletAmount++;
            } else if (randomSkillIndex == 2)
            {
                lightningAmount++;
            } else
            {
                shieldAmount++;
            }
        }
        for (int i = 0; i < spawnBallAmount; i++)
        {
            // Make three lists out of balls that player does not own based on their rarity
            List<GameObject> possibleCommonBalls = new List<GameObject>();
            List<GameObject> possibleRareBalls = new List<GameObject>();
            List<GameObject> possibleLegendaryBalls = new List<GameObject>();

            for (int j = 0; j < player.allBalls.Count; j++)
            {
                if (player.allBalls[j] == 0)
                {
                    // Add this ball as possible gift, as player doesnt have it
                    for (int k = 0; k < allCommonBallPrefabs.Length; k++)
                    {
                        if (j == allCommonBallPrefabs[i].GetComponent<BallItem>().GetBallIndex() &&
                            allCommonBallPrefabs[i].GetComponent<BallItem>().GetBallType() == BallTypes.Common)
                        {
                            // Add this ball to common list
                            possibleCommonBalls.Add(allCommonBallPrefabs[k]);
                        }
                    }
                    // Add this ball as possible gift, as player doesnt have it
                    for (int k = 0; k < allRareBallPrefabs.Length; k++)
                    {
                        if (j == allRareBallPrefabs[i].GetComponent<BallItem>().GetBallIndex() &&
                            allRareBallPrefabs[i].GetComponent<BallItem>().GetBallType() == BallTypes.Rare)
                        {
                            // Add this ball to common list
                            possibleRareBalls.Add(allRareBallPrefabs[k]);
                        }
                    }
                    // Add this ball as possible gift, as player doesnt have it
                    for (int k = 0; k < allLegendaryBallPrefabs.Length; k++)
                    {
                        if (j == allLegendaryBallPrefabs[i].GetComponent<BallItem>().GetBallIndex() &&
                            allLegendaryBallPrefabs[i].GetComponent<BallItem>().GetBallType() == BallTypes.Legendary)
                        {
                            // Add this ball to common list
                            possibleLegendaryBalls.Add(allLegendaryBallPrefabs[k]);
                        }
                    }
                }
            }

            // Fill a new list with balls picked randomly from one of the rarity types based on chest chance values
            List<BallTypes> ballPrizePool = new List<BallTypes>();

            // If player has not unlocked all common balls
            if (possibleCommonBalls.Count > 0)
            {
                for (int j = 0; j < chestUnlock.commonChance; j++)
                {
                    ballPrizePool.Add(BallTypes.Common);
                }
            }
            // If player has not unlocked all rare balls
            if (possibleRareBalls.Count > 0)
            {
                for (int j = 0; j < chestUnlock.rareChance; j++)
                {
                    ballPrizePool.Add(BallTypes.Rare);
                }
            }
            // If player has not unlocked all legendary balls
            if (possibleLegendaryBalls.Count > 0)
            {
                for (int j = 0; j < chestUnlock.legendaryChance; j++)
                {
                    ballPrizePool.Add(BallTypes.Legendary);
                }
            }
            // Pick one ball randomly from the selected ball type list and add it to allBalls
            BallTypes randomBallType = ballPrizePool[Random.Range(0, ballPrizePool.Count - 1)];
            if (randomBallType == BallTypes.Common)
            {
                GameObject randomCommonBall = possibleCommonBalls[Random.Range(0, possibleCommonBalls.Count - 1)];
                possibleCommonBalls.Remove(randomCommonBall);
                allBalls.Add(randomCommonBall);
            } else if (randomBallType == BallTypes.Rare)
            {
                GameObject randomRareBall = possibleRareBalls[Random.Range(0, possibleRareBalls.Count - 1)];
                possibleRareBalls.Remove(randomRareBall);
                allBalls.Add(randomRareBall);
            } else
            {
                GameObject randomLegendaryBall = possibleLegendaryBalls[Random.Range(0, possibleLegendaryBalls.Count - 1)];
                possibleLegendaryBalls.Remove(randomLegendaryBall);
                allBalls.Add(randomLegendaryBall);
            }
        }

        float coinsDelay = 0;
        float diamondsDelay = Mathf.Clamp(spawnCoinAmount, 0, 12) * 0.25f + coinsDelay;
        float skillsDelay = Mathf.Clamp(spawnDiamondAmount, 0, 8) * 0.3f + diamondsDelay;
        float ballsDelay = allSkills.Count * 0.75f + skillsDelay;

        // Count from end of prizes to find the last prize to show Collect button
        ChestPrizeTypes lastPrize = ChestPrizeTypes.Ball;
        if (allBalls.Count == 0)
        {
            lastPrize = ChestPrizeTypes.Skill;
        }
        if (allSkills.Count == 0)
        {
            lastPrize = ChestPrizeTypes.Diamond;
        }
        if (allDiamonds.Count == 0)
        {
            lastPrize = ChestPrizeTypes.Coin;
        }

        // Instantiate all prizes
        StartCoroutine(CreatePrize(coinsDelay, 0.01f, allCoins, lastPrize == ChestPrizeTypes.Coin, ChestPrizeTypes.Coin));
        StartCoroutine(CreatePrize(diamondsDelay, 0.01f, allDiamonds, lastPrize == ChestPrizeTypes.Diamond, ChestPrizeTypes.Diamond));
        if (allSkills.Count > 0)
        {
            StartCoroutine(CreatePrize(skillsDelay, 0.4f, allSkills, lastPrize == ChestPrizeTypes.Skill, ChestPrizeTypes.Skill));
        }
        if (allBalls.Count > 0)
        {
            StartCoroutine(CreatePrize(ballsDelay, 0, allBalls, lastPrize == ChestPrizeTypes.Ball, ChestPrizeTypes.Ball));
        }
    }
    #endregion

    #region Private Methods
    void EmitPrizeParticles(ChestPrizeTypes prizeType)
    {
        ParticleSystem.EmitParams emitOverride = new ParticleSystem.EmitParams();

        if (prizeType == ChestPrizeTypes.Skill)
        {
            prizeParticles.Emit(emitOverride, 50);
        }
        else if (prizeType == ChestPrizeTypes.Ball)
        {
            prizeParticles.Emit(emitOverride, 100);
        } else
        {
            prizeParticles.Emit(emitOverride, 20);
        }
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
            EmitPrizeParticles(prizeType);

            GameObject prize = Instantiate(prefabs[0], transform.position + new Vector3(0, 0, -50), Quaternion.identity);
            prize.transform.localScale *= 0.4f;
            prize.transform.SetParent(allPrizes.transform);
            prize.GetComponent<BallItem>().MoveUp();

            prizeCount.SetActive(false);
        }
        else
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                yield return new WaitForSeconds(interval);
                EmitPrizeParticles(prizeType);

                GameObject prize = Instantiate(prefabs[i], transform.position + new Vector3(0, 0, -10), Quaternion.identity);
                prize.transform.SetParent(allPrizes.transform);
                prize.GetComponent<ChestDropItem>().lastPrize = lastPrize;
            }
            if (prizeType == ChestPrizeTypes.Coin)
            {
                prizeCount.SetActive(true);
                prizeCount.GetComponent<Text>().text = coinAmount.ToString();
            }
            else if (prizeType == ChestPrizeTypes.Diamond)
            {
                prizeCount.SetActive(true);
                prizeCount.GetComponent<Text>().text = diamondAmount.ToString();
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
        openedChest.SetActive(false);
        collectButton.SetActive(true);
    }

    IEnumerator HideChestWindow()
    {
        prizeCount.SetActive(false);
        collectButton.SetActive(false);

        yield return new WaitForSeconds(1);
        allPrizes.SetActive(false);
        gameObject.SetActive(false);
    }
    #endregion
}