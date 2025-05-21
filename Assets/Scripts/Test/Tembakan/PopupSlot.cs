using UnityEngine;

public class PopupSlot : MonoBehaviour
{
    public bool IsOccupied { get; private set; }

    public void SetOccupied(bool value)
    {
        IsOccupied = value;
    }
}
