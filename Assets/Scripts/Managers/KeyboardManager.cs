using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CESCO;

public class KeyboardManager : MonoBehaviour
{
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.GamePause();
        }
#endif
    }
}
