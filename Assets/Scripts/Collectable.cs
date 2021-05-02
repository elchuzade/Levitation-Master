using System.Collections;
using UnityEngine;
using static GlobalVariables;

public class Collectable : MonoBehaviour
{
    LevelStatus levelStatus;

    [SerializeField] Rewards reward;

    // Speed with which the item will go to canvas location
    [SerializeField] int collectSpeed;
    [SerializeField] int dropSpeed;

    bool moveToCollectPosition;
    bool moveToDropPosition;

    // In case the item is dropped from the box
    private Vector3 dropPosition;

    #region Unity methods
    void Awake()
    {
        levelStatus = FindObjectOfType<LevelStatus>();
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
        if (other.gameObject.tag == "Ball")
        {
            levelStatus.CollectReward(reward);
            transform.SetParent(levelStatus.transform);
            moveToCollectPosition = true;

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
    IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(3);

        Destroy(transform.gameObject);
    }
    #endregion
}
