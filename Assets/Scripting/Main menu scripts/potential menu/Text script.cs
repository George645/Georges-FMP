using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NewMonoBehaviourScript : MonoBehaviour{
    public UnityEvent OnSelected;
    void Start(){
        SetRandomPosition();
    }

    void Update(){
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Vector3.up)){
            OnSelected.Invoke();
        }
    }
    public void SetRandomPosition(){
        transform.position = new Vector3(Random.Range(-4, 3) + 0.15f, 1, Random.Range(-4, 3) + 0.05f);
        Debug.Log((Mathf.Round(transform.position.x) + Mathf.Round(transform.position.z)) % 2);
        if ((transform.position.x + transform.position.z) % 2 == 1){
            //try{
                GetComponent<TMP_Text>().color = Color.black;
            //}
            //catch()
        }
    }
}
