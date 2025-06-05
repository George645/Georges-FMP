using UnityEngine;

namespace TC{
    public class EnlargeAreaButton : MonoBehaviour {
        public static GameObject thisGameObject;
        public GameObject upgradingObject;
        private void Start() {
            thisGameObject = gameObject;
        }
        public void ButtonPushed() {
            upgradingObject.GetComponent<UnderlyingPiece>().thisPiece.ExpandSize();
            upgradingObject.GetComponent<UnderlyingPiece>().RemoveNumbersOfCapturedPieces();
        }
    }
}