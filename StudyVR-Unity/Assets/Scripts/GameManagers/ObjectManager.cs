using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreloadObject
{
    public string ObjectRid;
    public GameObject GObject;

    public PreloadObject(string rid, GameObject gobj)
    {
        ObjectRid = rid;
        GObject = gobj;
    }
}

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance;

    private List<string> RIDs = new List<string>();
    private List<PreloadObject> PreloadObjects = new List<PreloadObject>();

    private int CurrentPreloadIndex = 0;

    private bool IsPreloaded = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    public void StartPreload()
    {
        if (IsPreloaded)
        {
            UIManager.Instance.UISystemMessage($"[System]: All objects are preloaded.");
            return;
        }

        foreach(string questionKey in DataManager.Instance.QuestionTable.LocalTable.itable.Keys)
        {
            string asset0 = null;
            if (DataManager.Instance.QuestionTable.LocalTable.itable[questionKey].TryGetValue("Asset(0)",out asset0))
            {
                bool preloaded = false;
                foreach(string id in RIDs)
                {
                    if (id == asset0)
                    {
                        preloaded = true;
                        break;
                    }
                }

                if(!preloaded)
                {
                    RIDs.Add(asset0);
                }
            }

            string asset1 = null;
            if (DataManager.Instance.QuestionTable.LocalTable.itable[questionKey].TryGetValue("Asset(1)", out asset1)
                && DataManager.Instance.QuestionTable.LocalTable.itable[questionKey].ContainsKey("AssetResponseType"))
            {
                if (DataManager.Instance.QuestionTable.LocalTable.itable[questionKey]["AssetResponseType"] == "Comparison")
                {
                    bool preloaded = false;
                    foreach (string id in RIDs)
                    {
                        if (id == asset1)
                        {
                            preloaded = true;
                            break;
                        }
                    }

                    if (!preloaded)
                    {
                        RIDs.Add(asset1);
                    }
                }
            }
        }

        Preload();
    }

    private void Preload()
    {
        if(CurrentPreloadIndex >= RIDs.Count)
        {
            UIManager.Instance.UISystemMessage($"[System]: All objects are preloaded.");
            IsPreloaded = true;
            return;
        }

        DataManager.Instance.UABTable.GetAsset(
            RIDs[CurrentPreloadIndex].Replace("[", "").Replace("]", ""),
            OnObjectPreloaded,
            OnProgressReturned
            );
    }

    private void OnObjectPreloaded(GameObject gobj, string gobjName)
    {
        PreloadObjects.Add(new PreloadObject(RIDs[CurrentPreloadIndex], gobj));
        CurrentPreloadIndex++;
        Preload();
    }

    private void OnProgressReturned(float progressPerc)
    {
        UIManager.Instance.UISystemMessage($"[System]: Preloading objects [{CurrentPreloadIndex + 1}/{RIDs.Count}] - {(int)(progressPerc * 100)}%");
    }
    

    private bool TryGetPreloadedObject(string rid, out GameObject gameObject)
    {
        foreach(PreloadObject preloadObject in PreloadObjects)
        {
            if(preloadObject.ObjectRid == rid)
            {
                gameObject = preloadObject.GObject;
                return true;
            }
        }

        gameObject = null;
        return false;
    }

    public IEnumerator GetObject(string rid, System.Action<GameObject> callback)
    {
        while (true)
        {
            if (TryGetPreloadedObject(rid, out GameObject gameObject))
            {
                callback?.Invoke(gameObject);
                break;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }

        yield return null;
    }
}
