using CESCO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject joyStickObj;
    private JoyStick joyStick;

    private void Awake()
    {
        joyStick = joyStickObj.GetComponent<JoyStick>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && GameManager.instance.GameState == CESCO.GAME_STATE.RUNNING)
        {
            Touch firstTouch = Input.GetTouch(0);

#if UNITY_EDITOR
            print("touch.position.x: " + firstTouch.position.x);
            print("Screen.width: " + Screen.width);
            print("touch.position.x < Screen.width: " + (firstTouch.position.x < Screen.width));
#endif

            switch(firstTouch.phase)
            {
                case TouchPhase.Began:
                    if (firstTouch.position.x < Screen.width / 2)
                    {
#if UNITY_EDITOR
                        print("터치 다운");
#endif
                        joyStickObj.SetActive(true);
                        joyStick.OnDown(firstTouch.position);
                    }
                    break;
                case TouchPhase.Moved:
                    joyStick.Drag(firstTouch.position);
                    break;
                case TouchPhase.Ended:
#if UNITY_EDITOR
                    print("터치 업");
#endif
                    joyStickObj.SetActive(false);
                    joyStick.OnUp();
                    break;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_EDITOR
        print("터치 업");
#endif
        joyStickObj.SetActive(false);
        joyStick.OnUp();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
#if UNITY_EDITOR
        print("터치 다운");
#endif
        joyStickObj.SetActive(true);
        joyStick.OnDown(eventData.position);
    }
}
