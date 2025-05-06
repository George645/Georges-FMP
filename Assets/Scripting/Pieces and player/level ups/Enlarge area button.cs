using UnityEngine;

public class EnlargeAreaButton : MonoBehaviour {
    public GameObject upgradingObject;
    public void ButtonPushed() {
        upgradingObject.GetComponent<UnderlyingPiece>().thisPiece.ExpandSize();
        upgradingObject.GetComponent<UnderlyingPiece>().RemoveNumbersOfCapturedPieces();
    }
}