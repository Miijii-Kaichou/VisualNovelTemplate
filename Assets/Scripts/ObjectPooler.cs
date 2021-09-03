using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    private static ObjectPooler Instance;

    [System.Serializable]
    public class ObjectPoolItem
    {
        public string name;
        public int size;
        public GameObject prefab;
        public bool expandPool;

        ObjectPoolItem(string initName, int initSize, GameObject initPrefab, bool allowExpand)
        {
            name = initName;
            size = initSize;
            prefab = initPrefab;
            expandPool = allowExpand;
        }

        public static ObjectPoolItem Create(string initName, int initSize, GameObject initPrefab, bool allowExpand)
        {
            return new ObjectPoolItem(initName, initSize, initPrefab, allowExpand);
        }
    }

    public List<ObjectPoolItem> itemsToPool;



    private List<GameObject> pooledObjects;

    public bool spawnItemsInParent = false;

    // Start is called before the first frame update

    public int poolIndex;
    void Awake()
    {
        Instance = this;
    }

    private void DeactivateAllPooledObjects()
    {
        foreach(GameObject obj in pooledObjects)
        {
            obj.SetActive(false);
        }
    }

    private void Start()
    {
        InitObjectPooler();
    }

    public void AddToPool(ObjectPoolItem poolItem)
    {
        itemsToPool.Add(poolItem);
    }

    public void AddToPool(string name, GameObject obj, int size = 1, bool expand = false)
    {
        itemsToPool.Add(ObjectPoolItem.Create(name, size, obj, expand));
    }

    public GameObject[] GetPooledObjects() => pooledObjects.ToArray();


    void InitObjectPooler()
    {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            for (int i = 0; i < item.size; i++)
            {
                GameObject newMember = Instantiate(item.prefab, spawnItemsInParent ? transform : null);
                newMember.SetActive(false);
                item.prefab.name = item.name;
                pooledObjects.Add(newMember);
            }
        }
    }


    public GameObject GetMember<T>(string name, out T result) where T : Component
    {
        #region Iteration
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            GameObject pooledObject = pooledObjects[i];

            if (pooledObject != null &&
                !pooledObject.activeInHierarchy &&
                (name + "(Clone)") == pooledObject.name &&
                pooledObject.GetComponent(typeof(T)) != null)
            {
                result = (T)pooledObject.GetComponent(typeof(T));
                return pooledObject;
            }
        }
        #endregion

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (name == item.prefab.name && item.expandPool)
            {

                GameObject newMember = Instantiate(item.prefab);
                newMember.SetActive(false);
                pooledObjects.Add(newMember);
                if (newMember.GetComponent(typeof(T)) != null)
                {
                    result = (T)newMember.GetComponent(typeof(T));
                    return newMember;
                }
            }
        }
        Debug.LogWarning("We couldn't find a prefab of this name " + name);
        result = null;
        return null;
    }

    public  GameObject GetMember(string name)
    {

        #region Iteration
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            GameObject pooledObject = pooledObjects[i];

            if (pooledObject != null &&
                !pooledObject.activeInHierarchy &&
                (name + "(Clone)") == pooledObject.name)
            {
                return pooledObject;
            }
        }
        #endregion

        foreach (ObjectPoolItem item in itemsToPool)
        {
            if (name == item.prefab.name && item.expandPool)
            {

                GameObject newMember = Instantiate(item.prefab);
                newMember.SetActive(false);
                pooledObjects.Add(newMember);
                return newMember;
            }
        }
#if UNITY_EDITOR
        Debug.LogWarning("We couldn't find a prefab of this name " + name);
#endif //UNITY_EDITOR
        return null;
    }
}
