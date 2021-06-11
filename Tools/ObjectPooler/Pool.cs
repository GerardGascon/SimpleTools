using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pool", menuName = "Simple Tools/Pool", order = 11)]
public class Pool : ScriptableObject{

    public List<PoolPrefab> pools;
    [System.Serializable]
    public class PoolPrefab{
        public string tag;
        public GameObject prefab;
        public bool undetermined;
        public int size;

        [HideInInspector] public Queue<GameObject> determinedPool;
        [HideInInspector] public List<GameObject> undeterminedPool;
    }
}
