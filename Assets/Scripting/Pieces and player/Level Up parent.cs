using UnityEngine;

public class LevelUpbuttonParent: MonoBehaviour{
    public static GameObject instance;
    void Start(){
        instance = gameObject;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update(){
        
    }
}
