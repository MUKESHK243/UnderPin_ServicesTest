using System;
using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public int currentFloorIndex = 0;
    public Direction currentMoveDirection;
    public float speed = 15f;

    private Coroutine moveCoroutine;
    public RectTransform rectTransform;
    public bool isInUse = false;
    public void SetDirection(Direction direction)
    {
        currentMoveDirection = direction;
    }
    public void MoveToFloor(int floorIndex,Direction movingDirection,Vector3 floorPosition,Action onComplete)
    {
        if(rectTransform == null)
        {
            Debug.LogError("RectTransform is not assigned in Elevator script.");
            return;
        }
        Debug.Log("Moving elevator to floor position: " + floorPosition.y,this);
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        isInUse = true;
        currentFloorIndex = floorIndex;
        SetDirection(movingDirection);
        moveCoroutine = StartCoroutine(MoveRoutine(floorPosition,onComplete));
    }
    IEnumerator MoveRoutine(Vector3 targetPosition, Action onComplete)
    {
        Vector3 startPosition = rectTransform.localPosition;

        float distance = Vector3.Distance(startPosition, targetPosition);

        float speed = 200f; 
        float duration = distance / speed;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            t = Mathf.Clamp01(t);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, smoothT);

            yield return null;
        }

        rectTransform.localPosition = targetPosition;
        isInUse = false;
        onComplete?.Invoke();
    }
}
