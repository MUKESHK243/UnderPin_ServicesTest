using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorUI : MonoBehaviour
{
    [SerializeField] private TMP_Text floorText;
    [SerializeField] private FloorRequestButton[] floorRequestButtons;

    public RectTransform elevatorUIContainer;
    public RectTransform elevatorContainer;

    public event Action<int> OnUpClicked;
    public event Action<int> OnDownClicked;

    private FloorType floorType;

    public bool IsGroundFloor()
    {
        return floorType == FloorType.Ground;
    }

    public void AddListeners()
    {
        foreach (var floorRequestButton in floorRequestButtons)
        {
            floorRequestButton.floorButton.onClick.RemoveAllListeners();
            floorRequestButton.floorButton.onClick.AddListener(() =>
            {
                OnClickAction(floorRequestButton);
            });
        }
    }

    private void OnClickAction(FloorRequestButton floorRequestButton)
    {
        if (floorRequestButton.direction == Direction.Up)
        {
            OnUpClicked?.Invoke((int)floorType);

        }
        else
        {
            OnDownClicked?.Invoke((int)floorType);
        }
    }

    public void SetFloorNumber(string _floortext)
    {
        floorText.text = $"{_floortext} Floor";
    }

    public void SetFloorType(FloorType _floorType)
    {
        floorType = _floorType;
    }

}

[Serializable]
public class FloorRequestButton
{
    public Direction direction;
    public Button floorButton;
}