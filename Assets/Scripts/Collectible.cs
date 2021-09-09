using System.Collections;
using UnityEngine;
using static GlobalVariables;

public class Collectible : MonoBehaviour
{
    LevelStatus levelStatus;

    [SerializeField] Rewards reward;
    [SerializeField] GameObject collectParticles;
    [SerializeField] BoxCollider col;

    // Speed with which the item will go to canvas location
    [SerializeField] int collectSpeed;
    [SerializeField] int dropSpeed;

    // for collectibles
    public int amount = 1;

    bool moveToCollectPosition;
    bool moveToDropPosition;
    bool collected;

    // In case the item is dropped from the box
    private Vector3 dropPosition;

    Player player;

    #region Unity methods
    void Awake()
    {
        levelStatus = FindObjectOfType<LevelStatus>();
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.LoadPlayer();
        if (player.maxLevelReached && (reward == Rewards.RedKey || reward == Rewards.SilverKey || reward == Rewards.GoldKey))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (moveToCollectPosition)
        {
            transform.position += new Vector3(0, collectSpeed, 0);
        }
        else if (moveToDropPosition)
        {
            MoveToDropPosition();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball" && !collected)
        {
            collected = true;
            levelStatus.CollectReward(reward, amount);
            transform.SetParent(levelStatus.transform);
            // This runs 1 second, so start moving after 1 second
            collectParticles.SetActive(true);

            StartCoroutine(RunCollectParticles());

            StartCoroutine(DestroyItem());
        }
    }
    #endregion

    #region Private methods
    void MoveToDropPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, dropPosition, dropSpeed * Time.deltaTime);

        if (transform.position == dropPosition)
        {
            moveToCollectPosition = true;
            if (!collected)
            {
                levelStatus.CollectReward(reward, amount);
            }
            StartCoroutine(DestroyItem());
        }
    }
    #endregion

    #region Public methods
    // @access from box script when it is opened by a ball
    public void SetDropPosition(Vector3 pos)
    {
        dropPosition = pos;
        moveToDropPosition = true;
    }
    #endregion

    #region Coroutine
    IEnumerator RunCollectParticles()
    {
        col.enabled = false;

        yield return new WaitForSeconds(0.2f);
        moveToCollectPosition = true;
    }

    IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    #endregion
}
