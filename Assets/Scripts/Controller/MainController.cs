using System.Collections;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] private FloorManager floorManager;
    [SerializeField] private ElevatorManager elevatorManager;
    private IEnumerator Start()
    {
        if (floorManager == null)
        {
            floorManager = FindFirstObjectByType<FloorManager>();
        }
        if (elevatorManager == null)
        {
            elevatorManager = FindFirstObjectByType<ElevatorManager>();
        }

        floorManager.Init();

        while (!floorManager.IsInitialized())
        {
            yield return null;
        }
        foreach (var floorUI in floorManager.FloorUIs)
        {
            if (floorUI.IsGroundFloor())
            {
                elevatorManager.CreateElevator(floorUI.elevatorContainer);
            }
            elevatorManager.CreateElevatorUI(floorUI.elevatorUIContainer);
        }
        elevatorManager.SetFloorCount(floorManager.FloorUIs.Count);
        foreach (var floor in floorManager.FloorUIs)
        {
            floor.OnUpClicked += HandleUpClicked;
            floor.OnDownClicked += HandleDownClicked;
        }

    }

    private void OnDestroy()
    {
        foreach (var floor in floorManager.FloorUIs)
        {
            floor.OnUpClicked -= HandleUpClicked;
            floor.OnDownClicked -= HandleDownClicked;
        }
    }

    private void HandleUpClicked(int floor)
    {
        Debug.Log("Up clicked on floor: " + floor);

        elevatorManager.RequestElevator(floor, Direction.Up);
    }

    private void HandleDownClicked(int floor)
    {
        Debug.Log("Down clicked on floor: " + floor);

        elevatorManager.RequestElevator(floor, Direction.Down);
    }
}
