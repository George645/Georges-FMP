using UnityEngine;

public class LevelUpOriginCubeIdentifier : MonoBehaviour {
    public static GameObject instance;

    void Start() {
        instance = gameObject;
    }
}
