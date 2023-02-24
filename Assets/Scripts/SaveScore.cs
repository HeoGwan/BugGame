using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveScore : MonoBehaviour
{
    [SerializeField] private TMP_InputField nicknameField;

    public void Save()
    {
        GameManager.instance.SaveScore(nicknameField.text);
        nicknameField.text = "";
        gameObject.SetActive(false);
    }
}
