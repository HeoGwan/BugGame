using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform stick;

    private Vector3 movePosition;
    public Vector3 MovePosition { get { return movePosition; } }
    private float radius;
    private bool canMove = false;
    private bool isTouch = false;
    public bool IsTouch
    {
        get { return isTouch; }
    }

    void Start()
    {
        radius = background.rect.width * 0.5f;
    }

    public void Drag(Vector2 position)
    {
        if (canMove == true)
        {
            Vector2 value = position - (Vector2)background.position;

            value = Vector2.ClampMagnitude(value, radius);
            stick.localPosition = value;

            float distance = Vector2.Distance(background.position, stick.position) / radius;
            value = value.normalized;
            movePosition = new Vector3(value.x * distance, value.y * distance);
        }
    }

    public void OnUp()
    {
        isTouch = false;
        canMove = false;
        movePosition = Vector3.zero;
    }

    public void OnDown(Vector2 position)
    {
        isTouch = true;
        canMove = true;
        background.position = position;
        stick.position = position;
    }
}
