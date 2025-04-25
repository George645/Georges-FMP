using UnityEngine;

public class UnderlyingPiece : MonoBehaviour{
    [HideInInspector]
    public bool selected;
    [HideInInspector]
    public bool firstFrameSelected = true;
    [SerializeField] public Piece thisPiece;
}
