using System.Collections.Generic;
using UnityEngine;

// To optimize performance and avoid having lots of gameobjects, the game reuses the ones the player "destroyed"
public class MeteorPool : MonoBehaviour
{
    public static MeteorPool Instance;                                      // A singleton

    // There's a list for each type of meteor and each state
    public List<MeteorController> ActiveBigMeteorPool;                      // List of active big meteors
    public List<MeteorController> InactiveBigMeteorPool;                    // List of inactive big meteors
    public List<MeteorController> ActiveSmallMeteorPool;                    // List of active small meteors
    public List<MeteorController> InactiveSmallMeteorPool;                  // List of inactive small meteors

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    // This makes it so when a meteor is hit is moved to the inactive list, and when is spawned is moved to the active list
    void Start()
    {
        EventManager.Instance.OnMeteorHit.AddListener(SetInactive);
        EventManager.Instance.OnMeteorSpawn.AddListener(SetActive);
    }

    void OnDestroy()
    {
        EventManager.Instance.OnMeteorHit.RemoveListener(SetInactive);
        EventManager.Instance.OnMeteorSpawn.RemoveListener(SetActive);
    }

    // Checks if the meteor is big or small, then sets it inactive and moves it to the inactive list for its type
    void SetInactive(MeteorController meteor)
    {
        meteor.gameObject.SetActive(false);
        if (meteor.isBig)
        {
            ActiveBigMeteorPool.Remove(meteor);
            InactiveBigMeteorPool.Add(meteor);
        }
        else
        {
            ActiveSmallMeteorPool.Remove(meteor);
            InactiveSmallMeteorPool.Add(meteor);
        }
    }

    // Checks if the meteor is big or small, then sets it active and moves it to the active list for its type
    void SetActive(MeteorController meteor)
    {
        meteor.gameObject.SetActive(true);
        if (meteor.isBig)
        {
            InactiveBigMeteorPool.Remove(meteor);
            ActiveBigMeteorPool.Add(meteor);
        }
        else
        {
            InactiveSmallMeteorPool.Remove(meteor);
            ActiveSmallMeteorPool.Add(meteor);

        }
    }

    // Returns the first element of the inactive small meteors and makes it active
    public MeteorController PopSmallInactive()
    {
        // Returns null if there aren't any inactive meteors
        if (InactiveSmallMeteorPool.Count <= 0)
        {
            return null;
        }
        var meteor = InactiveSmallMeteorPool[0];
        SetActive(meteor);
        return meteor;
    }

    // Returns the first element of the inactive big meteors and makes it active
    public MeteorController PopBigInactive()
    {
        // Returns null if there aren't any inactive meteors
        if (InactiveBigMeteorPool.Count <= 0)
        {
            return null;
        }
        var meteor = InactiveBigMeteorPool[0];
        SetActive(meteor);
        return meteor;
    }

    // Hides all active meteors and move them thier respective inactive list
    public void SetAllInactive()
    {
        int initialCountBig = ActiveBigMeteorPool.Count;
        int initialCountSmall = ActiveSmallMeteorPool.Count;

        for (int i = 0; i < initialCountBig; i++)
        {
            SetInactive(ActiveBigMeteorPool[ActiveBigMeteorPool.Count - 1]);
        }
        for (int i = 0; i < initialCountSmall; i++)
        {
            SetInactive(ActiveSmallMeteorPool[ActiveSmallMeteorPool.Count - 1]);
        }
    }
}
