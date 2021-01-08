using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Pooler{

    class PoolChecker : MonoBehaviour {
        public string poolTag;
    }

    static Dictionary<string, Pool.PoolPrefab> poolDictionary;
    static Scene poolScene;

    public static void CreatePools(Pool pool){
        poolDictionary = new Dictionary<string, Pool.PoolPrefab>();
        poolScene = SceneManager.CreateScene("PoolScene");

        foreach(Pool.PoolPrefab p in pool.pools){
            if (!p.undetermined){
                p.determinedPool = new Queue<GameObject>();
                for (int i = 0; i < p.size; i++){
                    GameObject obj = Object.Instantiate(p.prefab);
                    obj.SetActive(false);
                    obj.AddComponent<PoolChecker>().poolTag = p.tag;
                    SceneManager.MoveGameObjectToScene(obj, poolScene);
                    p.determinedPool.Enqueue(obj);
                }
            }else{
                p.undeterminedPool = new List<GameObject>();
            }
            poolDictionary.Add(p.tag, p);
        }
    }
    public static void CreatePools(Pool[] pools){
        poolDictionary = new Dictionary<string, Pool.PoolPrefab>();
        poolScene = SceneManager.CreateScene("PoolScene");

        for (int i = 0; i < pools.Length; i++){
            foreach (Pool.PoolPrefab p in pools[i].pools){
                if (!p.undetermined){
                    p.determinedPool = new Queue<GameObject>();
                    for (int j = 0; j < p.size; j++){
                        GameObject obj = Object.Instantiate(p.prefab);
                        obj.SetActive(false);
                        obj.AddComponent<PoolChecker>().poolTag = p.tag;
                        SceneManager.MoveGameObjectToScene(obj, poolScene);
                        p.determinedPool.Enqueue(obj);
                    }
                }else{
                    p.undeterminedPool = new List<GameObject>();
                }
                poolDictionary.Add(p.tag, p);
            }
        }
    }
    public static void Destroy(GameObject gameObject){
        PoolChecker poolChecker = gameObject.GetComponent<PoolChecker>();
        if (poolChecker == null){
            Debug.LogWarning("GameObject: " + gameObject + " isn't from a pool", gameObject);
            return;
        }

        gameObject.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(gameObject, poolScene);

        if (poolDictionary.ContainsKey(poolChecker.poolTag)){
            Pool.PoolPrefab pool = poolDictionary[poolChecker.poolTag];
            if (pool.undetermined){
                gameObject.SetActive(false);
                pool.undeterminedPool.Remove(gameObject);
            }else{
                gameObject.SetActive(false);
            }
        }
    }

    public static GameObject SpawnFromPool(string tag, Vector3 position){
        if (!poolDictionary.ContainsKey(tag)){
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Pool.PoolPrefab pool = poolDictionary[tag];
        GameObject objectToSpawn;
        if (!pool.undetermined){
            objectToSpawn = pool.determinedPool.Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = Quaternion.identity;
            objectToSpawn.SetActive(true);

            pool.determinedPool.Enqueue(objectToSpawn);
        }else{
            if(pool.undeterminedPool.Count != 0){
                int lastIndex = pool.undeterminedPool.Count - 1;
                objectToSpawn = pool.undeterminedPool[lastIndex];

                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = Quaternion.identity;
                objectToSpawn.SetActive(true);
            }else{
                objectToSpawn = Object.Instantiate(pool.prefab, position, Quaternion.identity);
                SceneManager.MoveGameObjectToScene(objectToSpawn, poolScene);
                objectToSpawn.AddComponent<PoolChecker>().poolTag = tag;
            }
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if(pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }
    public static GameObject SpawnFromPool(string tag, Vector3 position, Transform parent){
        if (!poolDictionary.ContainsKey(tag)){
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Pool.PoolPrefab pool = poolDictionary[tag];
        GameObject objectToSpawn;
        if (!pool.undetermined){
            objectToSpawn = pool.determinedPool.Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = Quaternion.identity;
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.SetActive(true);

            pool.determinedPool.Enqueue(objectToSpawn);
        }else{
            if (pool.undeterminedPool.Count != 0){
                int lastIndex = pool.undeterminedPool.Count - 1;
                objectToSpawn = pool.undeterminedPool[lastIndex];

                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = Quaternion.identity;
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.SetActive(true);
            }else{
                objectToSpawn = Object.Instantiate(pool.prefab, position, Quaternion.identity, parent);
                objectToSpawn.AddComponent<PoolChecker>().poolTag = tag;
            }
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }
    public static GameObject SpawnFromPool(string tag, Vector3 position, Transform parent, bool instantiateInWorldSpace){
        if (!poolDictionary.ContainsKey(tag)){
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (!instantiateInWorldSpace){
            SpawnFromPool(tag, position, parent);
        }

        Pool.PoolPrefab pool = poolDictionary[tag];
        GameObject objectToSpawn;
        if (!pool.undetermined){
            objectToSpawn = pool.determinedPool.Dequeue();

            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = Quaternion.identity;
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.SetActive(true);

            pool.determinedPool.Enqueue(objectToSpawn);
        }else{
            if (pool.undeterminedPool.Count != 0){
                int lastIndex = pool.undeterminedPool.Count - 1;
                objectToSpawn = pool.undeterminedPool[lastIndex];

                objectToSpawn.transform.localPosition = position;
                objectToSpawn.transform.localRotation = Quaternion.identity;
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.SetActive(true);
            }else{
                objectToSpawn = Object.Instantiate(pool.prefab);
                objectToSpawn.transform.localPosition = position;
                objectToSpawn.transform.localRotation = Quaternion.identity;
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.AddComponent<PoolChecker>().poolTag = tag;
            }
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }
    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation){
        if (!poolDictionary.ContainsKey(tag)){
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Pool.PoolPrefab pool = poolDictionary[tag];
        GameObject objectToSpawn;
        if (!pool.undetermined){
            objectToSpawn = pool.determinedPool.Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.SetActive(true);

            pool.determinedPool.Enqueue(objectToSpawn);
        }else{
            if (pool.undeterminedPool.Count != 0){
                int lastIndex = pool.undeterminedPool.Count - 1;
                objectToSpawn = pool.undeterminedPool[lastIndex];

                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                objectToSpawn.SetActive(true);
            }else{
                objectToSpawn = Object.Instantiate(pool.prefab, position, rotation);
                SceneManager.MoveGameObjectToScene(objectToSpawn, poolScene);
                objectToSpawn.AddComponent<PoolChecker>().poolTag = tag;
            }
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }
    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent){
        if (!poolDictionary.ContainsKey(tag)){
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        Pool.PoolPrefab pool = poolDictionary[tag];
        GameObject objectToSpawn;
        if (!pool.undetermined){
            objectToSpawn = pool.determinedPool.Dequeue();

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.SetActive(true);

            pool.determinedPool.Enqueue(objectToSpawn);
        }else{
            if (pool.undeterminedPool.Count != 0){
                int lastIndex = pool.undeterminedPool.Count - 1;
                objectToSpawn = pool.undeterminedPool[lastIndex];

                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.SetActive(true);
            }else{
                objectToSpawn = Object.Instantiate(pool.prefab, position, rotation, parent);
                objectToSpawn.AddComponent<PoolChecker>().poolTag = tag;
            }
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }
    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent, bool instantiateInWorldSpace){
        if (!poolDictionary.ContainsKey(tag)){
            Debug.Log("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        if (!instantiateInWorldSpace){
            SpawnFromPool(tag, position, rotation, parent);
        }

        Pool.PoolPrefab pool = poolDictionary[tag];
        GameObject objectToSpawn;
        if (!pool.undetermined){
            objectToSpawn = pool.determinedPool.Dequeue();

            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.SetActive(true);

            pool.determinedPool.Enqueue(objectToSpawn);
        }else{
            if (pool.undeterminedPool.Count != 0){
                int lastIndex = pool.undeterminedPool.Count - 1;
                objectToSpawn = pool.undeterminedPool[lastIndex];

                objectToSpawn.transform.localPosition = position;
                objectToSpawn.transform.localRotation = rotation;
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.SetActive(true);
            }else{
                objectToSpawn = Object.Instantiate(pool.prefab);
                objectToSpawn.transform.localPosition = position;
                objectToSpawn.transform.localRotation = rotation;
                objectToSpawn.transform.SetParent(parent);
                objectToSpawn.AddComponent<PoolChecker>().poolTag = tag;
            }
        }

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null){
            pooledObj.OnObjectSpawn();
        }
        return objectToSpawn;
    }
}
