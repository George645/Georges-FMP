using UnityEngine;

public class Enlargeareabutton : MonoBehaviour {
    public GameObject upgradingObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPushed() {
        upgradingObject.GetComponent<UnderlyingPiece>().thisPiece.ExpandSize();
        upgradingObject.GetComponent<UnderlyingPiece>().RemoveNumbersOfCapturedPieces();
    }
}
