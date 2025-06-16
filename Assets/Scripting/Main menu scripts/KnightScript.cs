using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
namespace TC_Menu {
    public class KnightScript : MonoBehaviour {
        [SerializeField]
        bool selected = false;
        readonly List<GameObject> displayedMovementList = new();

        void Start() {
            SetAllPositions();
        }

        void Update() {
            CheckIfSelected();
            ShowMovablePositions();
        }

        void ShowMovablePositions() {
            if (selected && displayedMovementList.Count == 0) {
                int count = 0;
                for (int x = -1; x <= 1; x += 2) {
                    for (int z = -1; z <= 1; z += 2) {
                        if (CubeBase.GetSquareInDirection(transform.position.x + 2 * x, transform.position.z + 1 * z)) {
                            displayedMovementList.Add(MoveableDisplays.Instance.GetObject());
                            displayedMovementList[count].SetActive(true);
                            displayedMovementList[count].transform.position = new Vector3(transform.position.x + 2 * x, 1.1f, transform.position.z + 1 * z);
                            displayedMovementList[count].GetComponent<TC.StartingBoardScript>().OriginalObject = gameObject;
                            count++;
                        }
                        if (CubeBase.GetSquareInDirection(transform.position.x + 1 * x, transform.position.z + 2 * z)) {
                            displayedMovementList.Add(MoveableDisplays.Instance.GetObject());
                            displayedMovementList[count].SetActive(true);
                            displayedMovementList[count].transform.position = new Vector3(transform.position.x + 1 * x, 1.1f, transform.position.z + 2 * z);
                            displayedMovementList[count].GetComponent<TC.StartingBoardScript>().OriginalObject = gameObject;
                            count++;
                        }
                    }
                }
            }
        }

        RaycastHit hitInfo;
        void CheckIfSelected() {
            if (Input.GetMouseButtonUp(0)) {
                selected = false;
                int count = displayedMovementList.Count;
                for (int i = 0; i < count; i++) {
                    displayedMovementList[0].SetActive(false);
                    displayedMovementList.RemoveAt(0);
                }
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)) {
                    if (hitInfo.collider.gameObject == gameObject) {
                        selected = true;
                    }
                }
            }
        }

        bool variable;
        void SetAllPositions() {
            transform.position = new Vector3(UnityEngine.Random.Range(-4, 3), 1.05f, UnityEngine.Random.Range(-4, 3));
            foreach (GameObject text in TextScript.textList.Where(text => text != null)) {
                do {
                    variable = false;
                    text.transform.position = new Vector3(UnityEngine.Random.Range(-4, 3) + 0.15f, 1.125f, UnityEngine.Random.Range(-3, 2) + 0.05f);
                    if (Mathf.Floor(text.transform.position.x) == Mathf.Floor(transform.position.x) && Mathf.Floor(text.transform.position.z) == Mathf.Floor(transform.position.z)) {
                        variable = true;
                    }
                    foreach (GameObject text1 in TextScript.textList.Where(text => text != null)) {
                        if (text1 == text) {
                            break;
                        }
                        if (Mathf.Floor(text.transform.position.x) == Mathf.Floor(text1.transform.position.x) && Mathf.Floor(text.transform.position.z) == Mathf.Floor(text1.transform.position.z)) {
                            variable = true;
                        }
                    }
                } while (variable);
                if ((Mathf.Abs(Mathf.Round(text.transform.position.x) + Mathf.Round(text.transform.position.z)) % 2) == 1) {
                    try {
                        text.GetComponent<TMP_Text>().color = Color.black;
                    }
                    catch (NullReferenceException) {
                        text.transform.GetChild(0).GetComponent<TMP_Text>().color = Color.black;
                    }
                }
            }
        }
    }
}
