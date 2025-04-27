using System.Collections.Generic;
using UnityEngine;

public class Moveabledisplays : MonoBehaviour
{
    public static Moveabledisplays Instance;
    [SerializeField]
    GameObject objectToPool;
    public List<GameObject> ObjectPool;
    [SerializeField]
    int amountToPool;

    void Start(){
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
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
