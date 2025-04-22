using UnityEngine;

public class Player : MonoBehaviour
{

    public static GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake() {
        if (player == null) {
            player = gameObject;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
