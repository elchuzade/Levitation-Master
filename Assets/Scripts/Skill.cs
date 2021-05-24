using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour
{
    [SerializeField] Image loader;
    [SerializeField] Text count;

    bool reloading;
    float reloadTime;

    float time;

    bool infinity;

    int skillCount;

    void Update()
    {
        if (reloading && !infinity)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                loader.fillAmount = time / reloadTime;
            } else
            {
                ReloadSkill();
            }
        }
    }

    #region Private Methods
    void CheckSkillDisable()
    {
        if (skillCount > 0)
        {
            if (!infinity)
            {
               loader.fillAmount = 0;
            }
        }
        else
        {
            loader.fillAmount = 1;
        }
    }
    #endregion

    #region Public Methods
    // @access from Level Status script
    public void ClickSkill(float _reloadTime)
    {
        if (_reloadTime < 0 && skillCount > 0)
        {
            infinity = true;
        }
        else
        {
            reloadTime = _reloadTime;
            time = _reloadTime;
        }

        GetComponent<AnimationTrigger>().Trigger("Start");
        // Disable button until reload is complete
        GetComponent<Button>().interactable = false;
        loader.fillAmount = 1;
        reloading = true;
    }

    // @access from Ball script when hit with a trap or enemy
    public void ReloadSkill()
    {
        reloading = false;
        loader.fillAmount = 0;
        GetComponent<Button>().interactable = true;
        CheckSkillDisable();
    }

    // @access from Level Status script
    public void SetSkillCount(int _count)
    {
        count.text = _count.ToString();
        skillCount = _count;
        CheckSkillDisable();
    }
    #endregion
}
