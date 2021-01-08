using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pool", menuName = "Tools/Pool", order = 0)]
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
