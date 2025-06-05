using System.Collections.Generic;
using UnityEngine;
namespace TC {
    public class MoveableDisplays : MonoBehaviour {
        public static MoveableDisplays Instance;
        public static MoveableDisplays Instance2;
        public static MoveableDisplays Instance3;
        public GameObject objectToPool;
        public List<GameObject> ObjectPool;
        [SerializeField]
        int amountToPool;

        void Start() {
            ObjectPool.Clear();
            if (name == "Movement circle pool") {
                Instance = this;
            }
            else if (name == "Upgrade circle pool") {
                Instance2 = this;
            }
            else if (name == "Capturing cylinder pool") {
                Instance3 = this;
            }
            else {
                Destroy(gameObject);
            }
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++) {
                tmp = Instantiate(objectToPool, transform);
                tmp.SetActive(false);
                ObjectPool.Add(tmp);
            }
        }
        public GameObject GetObject() {
            foreach (GameObject obj in ObjectPool) {
                if (!obj.activeSelf) {
                    return obj;
                }
            }
            GameObject tmp = Instantiate(objectToPool, transform);
            ObjectPool.Add(tmp);
            return tmp;
        }
    }
}
