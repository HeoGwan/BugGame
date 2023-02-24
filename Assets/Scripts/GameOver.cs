using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject showScores;

    private Stack<ScoreData> scoreDatas;

    public void GetScores()
    {
        scoreDatas = GameManager.instance.scoreManager.scoreDatas;

        while(scoreDatas.Count > 0)
        {
            GameObject scoreObj = GameManager.instance.prefabManager.GetScoreObj();
            ScoreData data = scoreDatas.Pop();

            // nickname
            scoreObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = data.Nickname;
            // date
            scoreObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = data.Date;
            // score
            scoreObj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = data.Score;

            scoreObj.transform.SetParent(showScores.transform);
            scoreObj.transform.localScale = Vector3.one;
            scoreObj.SetActive(true);
        }
    }
}
