using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private ElevatorDataSO elevatorDataSO;
    [SerializeField] private ElevatorUI elevatorUIPrefab;
    [SerializeField] private Elevator elevatorPrefab;

    private List<Elevator> elevators = new List<Elevator>();
    private List<ElevatorUI> elevatorUIs = new List<ElevatorUI>();

    [SerializeField] private float floorHeight = 264f;

    private Queue<int> requestQueue = new Queue<int>();

    private int floorCount;

    public void SetFloorCount(int count)
    {
        floorCount = count;
    }
    public void CreateElevatorUI(RectTransform elevator)
    {
        for (int i = 0; i < elevatorDataSO.elevatorCount; i++)
        {
            ElevatorUI elevatorUI = Instantiate(elevatorUIPrefab, elevator);
            elevatorUIs.Add(elevatorUI);
        }
    }

    public List<int> GetNeighbors(int index, int count)
    {
        List<int> neighbors = new List<int>();

        if (index - 1 >= 0)
            neighbors.Add(index - 1);

        if (index + 1 < count)
            neighbors.Add(index + 1);

        return neighbors;
    }

    public void CreateElevator(RectTransform elevatorContainer)
    {
        for (int i = 0; i < elevatorDataSO.elevatorCount; i++)
        {
            Elevator elevator = Instantiate(elevatorPrefab, elevatorContainer);
            elevators.Add(elevator);
        }
    }

    public void RequestElevator(int floorIndex, Direction direction)
    {
        var elevator = GetNearestElevator(floorIndex, direction);

        if (elevator == null)
            return;

        if (elevator.isInUse)
            return;

        Vector3 target = new Vector3(
            elevator.rectTransform.localPosition.x,
            floorIndex * floorHeight,
            elevator.rectTransform.localPosition.z
        );

        elevator.SetDirection(direction);
        elevator.MoveToFloor(floorIndex, direction, target, null);
    }

    public List<Elevator> freeLifts = new List<Elevator>();
    private Elevator GetNearestElevator(int floorIndex, Direction direction)
    {
        Elevator bestElevator = null;
        float bestDistance = float.MaxValue;

        foreach (var elevator in elevators)
        {
            if (elevator.isInUse) continue;

            float distance = Mathf.Abs(elevator.currentFloorIndex - floorIndex);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestElevator = elevator;
            }
        }

        if (bestElevator != null)
            return bestElevator;

        bestDistance = float.MaxValue;

        foreach (var elevator in elevators)
        {
            float distance = Mathf.Abs(elevator.currentFloorIndex - floorIndex);

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestElevator = elevator;
            }
        }

        return bestElevator;
    }
}
