using CESCO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    // 벌레, 플레이어
    [SerializeField] private GameObject[] bugPrefabs;
    [SerializeField] private GameObject[] hitPrefabs;
    [SerializeField] private GameObject playerHasTool;

    [Header("▼ HP Bar")]
    [SerializeField] private GameObject hpImagePrefab;
    [SerializeField] private GameObject hpBackgroundImagePrefab;

    [Header("▼ Tool Gauge")]
    [SerializeField] private GameObject toolGaugeImagePrefab;
    [SerializeField] private GameObject toolGaugeBackgroundImagePrefab;

    [Header("▼ Score")]
    [SerializeField] private GameObject scorePrefab;

    private List<GameObject> objs;
    private List<GameObject>[] bugPool;
    private List<GameObject>[] hitPool;
    private List<GameObject> scorePool;

    public List<GameObject>[] BugPool { get { return bugPool; } }

    private void Awake()
    {
        objs = new List<GameObject>();
        objs.Insert((int)OBJ_TYPE.PLAYER_HAS, playerHasTool);
        objs.Insert((int)OBJ_TYPE.HP_GAUGE_IMAGE, hpImagePrefab);
        objs.Insert((int)OBJ_TYPE.HP_GAUGE_BG_IMAGE, hpBackgroundImagePrefab);
        objs.Insert((int)OBJ_TYPE.TOOL_GAUGE_IMAGE, toolGaugeImagePrefab);
        objs.Insert((int)OBJ_TYPE.TOOL_GAUGE_BG_IMAGE, toolGaugeBackgroundImagePrefab);
        objs.Insert((int)OBJ_TYPE.SCORE, scorePrefab);

        bugPool = new List<GameObject>[bugPrefabs.Length];
        for (int i = 0; i < bugPool.Length; ++i)
        {
            bugPool[i] = new List<GameObject>();
        }
        InitializeBug(30);

        hitPool = new List<GameObject>[hitPrefabs.Length];
        for (int i = 0; i < hitPool.Length; ++i)
        {
            hitPool[i] = new List<GameObject>();
        }
        InitializeHit(10);

        scorePool = new List<GameObject>();
        InitializeScore(100);
    }

    private void InitializeBug(int count)
    {
        for (int type = 0; type < bugPrefabs.Length; ++type)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject newBug = Instantiate(bugPrefabs[type], transform);

                newBug.GetComponent<Bug>().SetHPCanvas();
                newBug.GetComponent<Bug>().SetHPBar(
                    RequestInstantiate(OBJ_TYPE.HP_GAUGE_BG_IMAGE, newBug.GetComponent<Bug>().HpCanvas.transform),
                    RequestInstantiate(OBJ_TYPE.HP_GAUGE_IMAGE, newBug.GetComponent<Bug>().HpCanvas.transform));

                newBug.SetActive(false);
                bugPool[type].Add(newBug);
            }
        }
    }
    
    private void InitializeHit(int count)
    {
        for (int type = 0; type < hitPool.Length; ++type)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject newHit = Instantiate(hitPrefabs[type], transform);
                newHit.SetActive(false);
                hitPool[type].Add(newHit);
            }
        }
    }

    private void InitializeScore(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newScore = Instantiate(scorePrefab, transform);
            newScore.SetActive(false);
            scorePool.Add(newScore);
        }
    }

    public void InitBug(BUG_TYPE type)
    {
        foreach (GameObject bug in bugPool[(int)type])
        {
            if (bug.activeSelf)
            {
                bug.SetActive(false);
            }
        }
    }

    public GameObject RequestInstantiate(OBJ_TYPE objType)
    {
        return Instantiate(objs[(int)objType]);
    }
    
    public GameObject RequestInstantiate(OBJ_TYPE objType, Transform parent)
    {
        return Instantiate(objs[(int)objType], parent);
    }


    public GameObject GetBug(BUG_TYPE type)
    {
        GameObject selectBug = null;

        // 선택한 풀의 비활성화 된 게임오브젝트 접근
        foreach(GameObject bug in bugPool[(int)type])
        {
            if (!bug.activeSelf)
            {
                // 발견하면 selectBug에 할당
                selectBug = bug;
                //selectBug.SetActive(true);
                break;
            }
        }

        // 못 찾았으면
        if (!selectBug)
        {
            // 새롭게 생성하고 selectBug에 할당
            selectBug = Instantiate(bugPrefabs[(int)type], transform);
            bugPool[(int)type].Add(selectBug);
        }

        return selectBug;
    }

    public GameObject GetHit(HIT_OBJ_TYPE type)
    {
        GameObject selectHit = null;

        // 선택한 풀의 비활성화 된 게임오브젝트 접근
        foreach(GameObject hit in hitPool[(int)type])
        {
            if (!hit.activeSelf)
            {
                // 발견하면 selectBug에 할당
                selectHit = hit;
                selectHit.SetActive(true);
                break;
            }
        }

        // 못 찾았으면
        if (!selectHit)
        {
            // 새롭게 생성하고 selectBug에 할당
            selectHit = Instantiate(hitPrefabs[(int)type], transform);
            hitPool[(int)type].Add(selectHit);
        }

        return selectHit;
    }

    public GameObject GetScoreObj()
    {
        GameObject selectHit = null;

        // 선택한 풀의 비활성화 된 게임오브젝트 접근
        foreach (GameObject score in scorePool)
        {
            if (!score.activeSelf)
            {
                // 발견하면 selectBug에 할당
                selectHit = score;
                selectHit.SetActive(true);
                break;
            }
        }

        // 못 찾았으면
        if (!selectHit)
        {
            // 새롭게 생성하고 selectBug에 할당
            selectHit = Instantiate(scorePrefab, transform);
            scorePool.Add(selectHit);
        }

        return selectHit;
    }

    public Stack<GameObject> GetActiveBugs(BUG_TYPE bugType)
    {
        Stack<GameObject> selectBug = new Stack<GameObject>();

        // 활성화 된 벌레들을 가져옴
        foreach (GameObject bug in bugPool[(int)bugType])
        {
            if (bug.activeSelf)
            {
                selectBug.Push(bug);
            }
        }

        return selectBug;
    }

    public float GetBugSound()
    {
        return bugPool[0][0].GetComponent<Bug>().GetVolume();
    }
}
