using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CESCO;
using TMPro;

public struct ScoreData
{
    public ScoreData(string nickname, string date, string score)
    {
        Nickname = nickname;
        Date = date;
        Score = score;
    }

    public string Nickname { get; set; }
    public string Date { get; set; }
    public string Score { get; set; }
}

public class GameManager : MonoBehaviour
{

    // �̱���
    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject target;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private GameObject inputNicknameObj;
    [SerializeField] private TMP_InputField nicknameField;

    private bool isSave = false;

    public Player CurrentPlayer
    {
        get { return player.GetComponent<Player>(); }
    }

    public Target CurrentTarget
    {
        get { return target.GetComponent<Target>(); }
    }

    public float CameraSize;

    // ���� ���� ����
    private GAME_STATE gameState = GAME_STATE.START;
    public GAME_STATE GameState { get { return gameState; } }
    private int level = 0;
    [SerializeField] private int levelStep = 3; // �ش� ������ ���� ������ ü���� ������
    public int Level { get { return level; } }
    public int LevelStep { get { return levelStep; } }

    int gameScore = 0;

    //private float minDelay = 0.6f;
    //private float maxDelay = 1.9f;
    [SerializeField] private uint payTime = 90;

    [Header("�� Managers")]
    #region Managers
    public ToolManager toolManager;
    public TimeManager timeManager;
    public UIManager uiManager;
    public PrefabManager prefabManager;
    public SoundManager soundManager;
    public SettingManager settingManager;
    public MouseManager mouseManager;
    public KeyboardManager keyboardManager;
    public ScoreManager scoreManager;
    public ScreenManager screenManager;
    public ShopManager shopManager;
    public ReinforceManager reinforceManager;
    public StageManager stageManager;
    public SpawnManager spawnManager;
    public InputManager inputManager;
    public PayManager payManager;
    public BackgroundManager backgroundManager;
    public DatabaseManager databaseManager;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("���� �Ŵ��� ����");
            Destroy(gameObject);
        }

        mouseManager.Visible();
        player.SetActive(false);
        target.SetActive(false);
        inputNicknameObj.SetActive(false);
        CameraSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if (gameState == GAME_STATE.RUNNING)
        {
            if (target.GetComponent<Target>().HP <= 0)
            {
                GameOver();
            }

            if (timeManager.RunningTime >= payTime)
            {
                // ���� �Ͻ����� �� ���� ui ���
                gameState = GAME_STATE.PAY;
                GamePause();
            }
        }
    }

    public void GameInit()
    {
        // ���� ���� �ʱ�ȭ
        // �ð�, ���ھ� �ʱ�ȭ
        timeManager.Init();
        scoreManager.Init();
        gameState = GAME_STATE.RUNNING;
        Time.timeScale = 1;
        level = 0;
        gameScore = 0;
        isSave = false;
    }

    public void GameStart()
    {
        GameInit();

        // ���콺 �����
        //mouseManager.InVisible();

        // Ÿ�� ����
        target.SetActive(true);

        // ����(��) ����
        //BugSpawn();
        //spawnManager.SpawnBugs();
        spawnManager.StartSpawnBug();

        // Ÿ�̸� ����
        timeManager.TimerStart();

        // �÷��̾� ����
        player.SetActive(true);

        // ȭ�� ��ȯ
        screenManager.ChangeScreen(SCREEN.INGAME);
    }

    public void GameOver()
    {
        databaseManager.GetData(false);
        screenManager.ChangeScreen(SCREEN.GAMEOVER);
        // ���� ���� ����
        GameEnd();
    }

    public void GameEnd()
    {
        // ���� ���� NONE���� ����
        gameState = GAME_STATE.START;
        Time.timeScale = 0;
        mouseManager.Visible();
        CancelInvoke("BugSpawn");

        // �÷��̾�, Ÿ��, ���� ��� ����
        player.SetActive(false);
        target.SetActive(false);
        spawnManager.RemoveBug();

        // ��ȭ, ������ �� �ʱ�ȭ �ؾ� ��
        // �Ŵ��� �ʱ�ȭ
        reinforceManager.Init();
        shopManager.Init();
        spawnManager.Init();
        //screenManager.GoMain();

        // ���� ���ھ� ���� �� �ʱ�ȭ
        gameScore = scoreManager.ScoreInit();
        finalScore.text = gameScore + "����";
    }

    public void GameRule()
    {
        // ���� ��� ȭ���� ������
    }

    public void GameSetting()
    {
        // ����� �ٲٱ�

        // ���� ���� �� ���� ����
        Time.timeScale = 0;

        // uiManager�� �̿��Ͽ� ȭ�� ��ȯ
        screenManager.ChangeScreen(SCREEN.SETTING);
        settingManager.Enable();
    }

    public void GamePause()
    {
        if (gameState == GAME_STATE.RUNNING)
        {
            Pause();
        }
        else if (gameState == GAME_STATE.PAUSE)
        {
            Resume();
        }
        else if (gameState == GAME_STATE.PAY)
        {
            Pay();
        }
    }

    public void Resume()
    {
        // ȭ�� ��ȯ(Pause->InGame) ���ư���
        screenManager.PrevScreen();

        // ���콺 Ŀ�� �����
        //mouseManager.InVisible();

        player.SetActive(true);

        // �ð� ����
        Time.timeScale = 1;

        spawnManager.StartSpawnBug();

        // Ȱ��ȭ �� �������� �Ҹ��� ��
        Stack<GameObject> bugs = prefabManager.GetActiveBugs(spawnManager.BugType);
        while (bugs.Count > 0)
        {
            Bug bug = bugs.Pop().GetComponent<Bug>();
            bug.AudioPlay();
        }

        // ���� �Ͻ����� Ǯ��
        gameState = GAME_STATE.RUNNING;
    }

    public void Restart()
    {
        // ���� �����
        // ���� ���� �� �ٽ� ����
        GameEnd();
        GameStart();
    }

    public void Pause()
    {
        // �Ͻ�����
        // ȭ�� ��ȯ
        screenManager.ChangeScreen(SCREEN.PAUSE);

        // ���콺 Ŀ�� ���̱�
        mouseManager.Visible();

        // �ð� ����
        Time.timeScale = 0;

        // ���� �Ͻ�����
        gameState = GAME_STATE.PAUSE;

        // ���� ���� �ߴ�
        spawnManager.StopSpawnBug();

        // Ȱ��ȭ �� �������� �Ҹ��� ��
        Stack<GameObject> bugs = prefabManager.GetActiveBugs(spawnManager.BugType);
        while (bugs.Count > 0)
        {
            Bug bug = bugs.Pop().GetComponent<Bug>();
            bug.AudioPause();
        }
    }

    public void Pay()
    {
        // ���� �� ��ȭ �� ���� ����
        // ���� �Ͻ�����
        Time.timeScale = 0;

        // ���콺 ���̵���
        //mouseManager.Visible();

        player.SetActive(false);

        // ���� ȭ�� ���
        screenManager.ChangeScreen(SCREEN.PAY);

        payManager.PayMoney();

        // ���� ���� �ߴ�
        spawnManager.StopSpawnBug();

        // Ȱ��ȭ �� �������� �Ҹ��� ��
        Stack<GameObject> bugs = prefabManager.GetActiveBugs(spawnManager.BugType);
        while (bugs.Count > 0)
        {
            // Ȱ��ȭ �� �������� �Ҹ��� ��
            Bug bug = bugs.Pop().GetComponent<Bug>();
            bug.AudioPause();
        }
    }

    public void NextGame()
    {
        // ���� �� ��ȭ, ���� ���� �� ���� ���� ����
        ++level;
        stageManager.NextStage();
        player.SetActive(true);
        gameState = GAME_STATE.RUNNING;
        spawnManager.StartSpawnBug();
        //mouseManager.InVisible();
        screenManager.PrevScreen();
    }

    public void GameMain()
    {
        // ������ �����Ű�� ���� ȭ������ �̵��Ѵ�.
        GameEnd();
        screenManager.GoMain();
    }

    public void GameExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();  
        #endif
    }

    public void ChangeGameState(GAME_STATE gameState)
    {
        this.gameState = gameState;
    }

    public void ShowInputNickname()
    {
        if (!isSave)
        {
            inputNicknameObj.SetActive(true);
        }
    }

    public void SaveScore()
    {
        string nickname = nicknameField.text;

        if (nickname == "")
        {
            return;
        }

        // ���ھ� ���� ���
        if (databaseManager.WriteData(nickname, gameScore))
        {
            print("������ ���� �Ϸ�");
        }
        else
        {
            print("������ ���� ����");
        }
        nicknameField.text = "";

        // �����͸� ���������� ������ �ִ� ���ھ� ����� �������ش�.
        scoreManager.InitScoreData();
        inputNicknameObj.SetActive(false);
        isSave = true;

        databaseManager.GetData(true);
    }

    public void CancelInputNickname()
    {
        inputNicknameObj.SetActive(false);
    }
}
