using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextScript : MonoBehaviour{
    public static List<GameObject> textList;
    public UnityEvent OnSelected;
    bool knightIsAbove = false;
    void Awake(){
        try {
            textList.Add(this.gameObject);
        }catch(NullReferenceException e) {
            textList = new() {
                this.gameObject
            };
        }
    }

    void Update(){
        RaycastHit Info;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.down, out Info)){
            if (Info.collider.gameObject.name.Contains("Knight")) {
                if (!knightIsAbove) {
                    OnSelected.Invoke();
                    knightIsAbove = !knightIsAbove;
                }
            }
            else {
                knightIsAbove=true;
            }
        }
        else {
            knightIsAbove = false;
        }
    }
}
