using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TC_Menu {
    public class TextScript : MonoBehaviour {
        public static List<GameObject> textList;
        public UnityEvent OnSelected;
        bool knightIsAbove = false;
        void Awake() {
            try {
                textList.Add(this.gameObject);
            }
            catch (NullReferenceException) {
                textList = new() {
                this.gameObject
            };
            }
        }

        void FixedUpdate() {
            Debug.DrawRay(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.down, Color.red, 10);
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Vector3.down, out RaycastHit Info)) {
                if (Info.collider.gameObject.name.Contains("Knight")) {
                    if (!knightIsAbove) {
                        OnSelected.Invoke();
                        knightIsAbove = !knightIsAbove;
                    }
                }
                else {
                    knightIsAbove = false;
                }
            }
            else {
                knightIsAbove = false;
            }
        }
    }
}
