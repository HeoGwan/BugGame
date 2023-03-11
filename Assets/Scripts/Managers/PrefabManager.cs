using CESCO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefabManager : MonoBehaviour
{
    // 벌레, 플레이어
    [SerializeField] private GameObject[] bugPrefabs;
    [SerializeField] private GameObject[] hitPrefabs;
    [SerializeField] private GameObject playerHasTool;

    [Space(20)]
    [Header("▼ Reinforce and Shop")]
    [Header("Reinforce")]
    [SerializeField] private GameObject showPlayerHasTool;
    [Header("Shop")]
    [SerializeField] private GameObject shopItemPrefab;
    [SerializeField] private GameObject lockPrefab;
    [SerializeField] private Sprite lockImage;

    [Space(20)]
    [Header("▼ HP Bar")]
    [SerializeField] private GameObject hpImagePrefab;
    [SerializeField] private GameObject hpBackgroundImagePrefab;

    [Space(20)]
    [Header("▼ Tool Gauge")]
    [SerializeField] private GameObject toolGaugeImagePrefab;
    [SerializeField] private GameObject toolGaugeBackgroundImagePrefab;

    [Space(20)]
    [Header("▼ Score")]
    [SerializeField] private GameObject scorePrefab;

    private List<GameObject> objs;
    private List<Bug>[] bugPool;
    private List<GameObject>[] hitPool;
    private List<GameObject> scorePool;
    private List<GameObject> playerHasToolPool;
    private List<GameObject> shopItemPool;
    private List<GameObject> lockPool;
    private List<GameObject> selectToolPool;

    public List<Bug>[] BugPool { get { return bugPool; } }

    private void Awake()
    {
        objs = new List<GameObject>();
        objs.Insert((int)OBJ_TYPE.HP_GAUGE_IMAGE, hpImagePrefab);
        objs.Insert((int)OBJ_TYPE.HP_GAUGE_BG_IMAGE, hpBackgroundImagePrefab);
        objs.Insert((int)OBJ_TYPE.TOOL_GAUGE_IMAGE, toolGaugeImagePrefab);
        objs.Insert((int)OBJ_TYPE.TOOL_GAUGE_BG_IMAGE, toolGaugeBackgroundImagePrefab);
        objs.Insert((int)OBJ_TYPE.SCORE, scorePrefab);

        bugPool = new List<Bug>[bugPrefabs.Length];
        for (int i = 0; i < bugPool.Length; ++i)
        {
            bugPool[i] = new List<Bug>();
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

        playerHasToolPool = new List<GameObject>();
        InitializeHasTool(6);

        shopItemPool = new List<GameObject>();
        lockPool = new List<GameObject>();
        InitializeShopItem(6);

        selectToolPool = new List<GameObject>();
        InitializeSelectTool(6);
    }

    // Init 함수
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
                bugPool[type].Add(newBug.GetComponent<Bug>());
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

    public void InitializeHasTool(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newHasTool = Instantiate(showPlayerHasTool, transform);
            newHasTool.SetActive(false);
            playerHasToolPool.Add(newHasTool);
        }
    }

    public void InitBug(BUG_TYPE type)
    {
        foreach (Bug bug in bugPool[(int)type])
        {
            if (bug.gameObject.activeSelf)
            {
                bug.gameObject.SetActive(false);
            }
        }
    }

    public void InitializeShopItem(int count)
    {
        for (int index = 0; index < count; ++index)
        {
            // 상점 아이템 보여주는 오브젝트 추가
            GameObject newShopItem = Instantiate(shopItemPrefab, transform);
            newShopItem.SetActive(false);
            shopItemPool.Add(newShopItem);

            // 자물쇠 이미지를 가진 오브젝트 추가
            GameObject newLock = Instantiate(lockPrefab, transform);
            newLock.transform.SetParent(newShopItem.transform);
            newLock.GetComponent<Image>().sprite = lockImage;
            lockPool.Add(newLock);
        }
    }

    public void InitializeSelectTool(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newHasTool = Instantiate(playerHasTool, transform);
            newHasTool.SetActive(false);
            newHasTool.name = i.ToString();
            selectToolPool.Add(newHasTool);
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


    // Get 함수
    public Bug GetBug(BUG_TYPE type)
    {
        Bug selectBug = null;

        // 선택한 풀의 비활성화 된 게임오브젝트 접근
        foreach(Bug bug in bugPool[(int)type])
        {
            if (!bug.gameObject.activeSelf)
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
            selectBug = Instantiate(bugPrefabs[(int)type], transform).GetComponent<Bug>();
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
        GameObject selectScore = null;

        // 선택한 풀의 비활성화 된 게임오브젝트 접근
        foreach (GameObject score in scorePool)
        {
            if (!score.activeSelf)
            {
                // 발견하면 selectBug에 할당
                selectScore = score;
                selectScore.SetActive(true);
                break;
            }
        }

        // 못 찾았으면
        if (!selectScore)
        {
            // 새롭게 생성하고 selectBug에 할당
            selectScore = Instantiate(scorePrefab, transform);
            scorePool.Add(selectScore);
        }

        return selectScore;
    }

    public GameObject GetHasTool()
    {
        GameObject selectTool = null;

        foreach (GameObject hasTool in playerHasToolPool)
        {
            if (!hasTool.activeSelf)
            {
                selectTool = hasTool;
                selectTool.SetActive(true);
                break;
            }
        }

        if (!selectTool)
        {
            selectTool = Instantiate(showPlayerHasTool, transform);
            playerHasToolPool.Add(selectTool);
        }

        return selectTool;
    }

    public GameObject GetShopItem(ShopItem shopItemInfo)
    {
        GameObject selectShopItem = null;

        foreach (GameObject shopItem in shopItemPool)
        {
            if (!shopItem.activeSelf)
            {
                selectShopItem = shopItem;
                selectShopItem.SetActive(true);
                selectShopItem.GetComponent<ShopItem>().SetItemInfo(shopItemInfo);
                break;
            }
        }

        if (!selectShopItem)
        {
            // 조건에 맞는 오브젝트 못 찾았을 시
            // 상점 아이템 보여주는 오브젝트 추가
            selectShopItem = Instantiate(shopItemPrefab, transform);
            selectShopItem.GetComponent<ShopItem>().SetItemInfo(shopItemInfo);
            shopItemPool.Add(selectShopItem);

            // 자물쇠 이미지를 가진 오브젝트 추가
            GameObject newLock = Instantiate(lockPrefab, transform);
            newLock.transform.SetParent(selectShopItem.transform);
            lockPool.Add(newLock);
        }

        return selectShopItem;
    }

    public GameObject GetSelectTool()
    {
        GameObject selectTool = null;

        foreach (GameObject tool in selectToolPool)
        {
            if (!tool.activeSelf)
            {
                selectTool = tool;
                selectTool.SetActive(true);
                break;
            }
        }

        if (!selectTool)
        {
            selectTool = Instantiate(playerHasTool, transform);
            selectToolPool.Add(selectTool);
        }

        return selectTool;
    }


    public void PutBackObj(GameObject backObj)
    {
        backObj.SetActive(false);
        backObj.transform.SetParent(transform);
    }

    public Stack<Bug> GetActiveBugs(BUG_TYPE bugType)
    {
        Stack<Bug> selectBug = new Stack<Bug>();

        // 활성화 된 벌레들을 가져옴
        foreach (Bug bug in bugPool[(int)bugType])
        {
            if (bug.gameObject.activeSelf)
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
