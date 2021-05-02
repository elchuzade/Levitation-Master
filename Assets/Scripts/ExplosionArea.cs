using System.Collections.Generic;
using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    List<GameObject> detectedEnemies = new List<GameObject>();
    List<GameObject> detectedTraps = new List<GameObject>();
    List<GameObject> detectedBarriers = new List<GameObject>();

    bool ballDetected;

    #region Unity Methods
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            ballDetected = true;
        }
        else if (other.gameObject.tag == "Trap")
        {
            bool detected = false;
            for (int i = 0; i < detectedTraps.Count; i++)
            {
                if (detectedTraps[i].GetHashCode() == other.gameObject.GetHashCode())
                {
                    detected = true;
                }
            }

            if (!detected)
            {
                detectedTraps.Add(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "Enemy")
        {
            bool detected = false;
            for (int i = 0; i < detectedEnemies.Count; i++)
            {
                if (detectedEnemies[i].GetHashCode() == other.gameObject.GetHashCode())
                {
                    detected = true;
                }
            }

            if (!detected)
            {
                detectedEnemies.Add(other.gameObject);
            }
        }
        else if (other.gameObject.tag == "Barrier")
        {
            bool detected = false;
            for (int i = 0; i < detectedBarriers.Count; i++)
            {
                if (detectedBarriers[i].GetHashCode() == other.gameObject.GetHashCode())
                {
                    detected = true;
                }
            }

            if (!detected)
            {
                detectedBarriers.Add(other.gameObject);
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            ballDetected = false;
        }
        else if (other.gameObject.tag == "Trap")
        {
            for (int i = 0; i < detectedTraps.Count; i++)
            {
                if (detectedTraps[i].GetHashCode() == other.gameObject.GetHashCode())
                {
                    detectedTraps.Remove(detectedTraps[i]);
                }
            }
        }
        else if (other.gameObject.tag == "Enemy")
        {
            for (int i = 0; i < detectedEnemies.Count; i++)
            {
                if (detectedEnemies[i].GetHashCode() == other.gameObject.GetHashCode())
                {
                    detectedEnemies.Remove(detectedEnemies[i]);
                }
            }
        }
        else if (other.gameObject.tag == "Barrier")
        {
            for (int i = 0; i < detectedBarriers.Count; i++)
            {
                if (detectedBarriers[i].GetHashCode() == other.gameObject.GetHashCode())
                {
                    detectedBarriers.Remove(detectedBarriers[i]);
                }
            }
        }
    }
    #endregion

    #region Public Methods
    // @access from Dynamite's Trap script
    public bool IsBallDetected()
    {
        return ballDetected;
    }

    // @access from Ball script
    public List<GameObject> GetAllDetectedEnemies()
    {
        return detectedEnemies;
    }

    // @access from Ball script
    public List<GameObject> GetAllDetectedTraps()
    {
        return detectedTraps;
    }

    // @access from Ball script
    public List<GameObject> GetAllDetectedBarriers()
    {
        return detectedBarriers;
    }

    // @access from Ball script
    public void ClearAllDetectedEnemies()
    {
        detectedEnemies.Clear();
    }

    // @access from Ball script
    public void ClearAllDetectedTraps()
    {
        detectedTraps.Clear();
    }

    // @access from Ball script
    public void ClearAllDetectedBarriers()
    {
        detectedBarriers.Clear();
    }
    #endregion
}
