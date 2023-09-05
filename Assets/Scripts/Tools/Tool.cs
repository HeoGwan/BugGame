using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CESCO;
using UnityEditor;

public class Tool : MonoBehaviour
{
    #region 도구 설정
    // 연사 속도
    // 매우 느림, 느림, 보통, 빠름, 매우 빠름
    //     1,     0.7,  0.5,  0.3,     0.1
    [SerializeField] protected float hitDelay = .5f;
    public float Delay { get { return hitDelay; } }
    [SerializeField] private TOOL_RATE toolRate;
    public TOOL_RATE ToolRate
    {
        get { return toolRate; }
        set { toolRate = value; }
    }

    [SerializeField] private int ratePrice;
    public int RatePrice { get { return ratePrice; } }

    // 범위
    // 작음, 보통, 큼
    //  0.5,  1,   1.5
    [SerializeField] protected float radius = 1f;
    public float Radius { get { return radius; } }
    [SerializeField] private TOOL_RADIUS toolRadius;
    public TOOL_RADIUS ToolRadius
    {
        get { return toolRadius; }
        set { toolRadius = value; }
    }

    [SerializeField] private int radiusPrice;
    public int RadiusPrice { get { return radiusPrice; } }

    // 이동 속도
    // 매우 느림, 느림, 보통, 빠름, 매우 빠름
    //     0.5,    0.7,   1,   1.3,    1.5
    [SerializeField] protected float speed = 10f;
    public float Speed { get { return speed; } }
    [SerializeField] private TOOL_SPEED toolSpeed;
    public TOOL_SPEED ToolSpeed
    {
        get { return toolSpeed; }
        set { toolSpeed = value; }
    }

    [SerializeField] private int speedPrice;
    public int SpeedPrice { get { return speedPrice; } }

    [SerializeField] protected float damage;
    public float Damage { get { return damage; } }
    [SerializeField] private TOOL_DAMAGE toolDamage;
    public TOOL_DAMAGE ToolDamage
    { 
        get { return toolDamage; }
        set { toolDamage = value; }
    }
    // 데미지 최대 단계
    [SerializeField] private int maxDamageLevel;

    [SerializeField] private int damagePrice;
    public int DamagePrice { get { return damagePrice; } }


    // 매우 느림, 느림, 보통, 빠름, 매우 빠름
    private float[] rates = { 1f, 0.7f, 0.5f, 0.3f, 0.1f };
    // 작음, 보통, 큼
    private float[] radiuses = { 0.8f, 1.2f, 1.6f };
    // 매우 느림, 느림, 보통, 빠름, 매우 빠름
    private float[] speeds = { 3f, 5f, 8f, 10f, 12f };


    [TextArea]
    [SerializeField] private string toolInfo;
    public string ToolInfo { get { return toolInfo; } }
    #endregion

    [SerializeField] protected TOOL tool;
    public TOOL ToolType
    {
        get { return tool; }
    }

    public bool hasPlayer = false;

    protected bool isPressed = false;
    protected bool canHit;
    public bool CanHit { get { return canHit; } }

    [SerializeField] private string toolName;
    public string ToolName
    {
        get { return toolName; }
    }
    [SerializeField] protected Sprite toolImage;
    public Sprite ToolImage { get { return toolImage; } }

    private WaitForSeconds waitHitDelay;


    #region 속성 getter, setter
    public string GetRateText()
    {
        switch (toolRate)
        {
            case TOOL_RATE.SUPER_SLOW:
                return "매우 느림";
            case TOOL_RATE.SLOW:
                return "느림";
            case TOOL_RATE.NORMAL:
                return "보통";
            case TOOL_RATE.FAST:
                return "빠름";
            case TOOL_RATE.SUPER_FAST:
                return "매우 빠름";
            default:
                return "";
        }
    }

    public string GetRadiusText()
    {
        switch (toolRadius)
        {
            case TOOL_RADIUS.SMALL:
                return "작음";
            case TOOL_RADIUS.MEDIUM:
                return "보통";
            case TOOL_RADIUS.LARGE:
                return "큼";
            default:
                return "";
        }
    }
    
    public string GetSpeedText()
    {
        switch (toolSpeed)
        {
            case TOOL_SPEED.SUPER_SLOW:
                return "매우 느림";
            case TOOL_SPEED.SLOW:
                return "느림";
            case TOOL_SPEED.NORMAL:
                return "보통";
            case TOOL_SPEED.FAST:
                return "빠름";
            case TOOL_SPEED.SUPER_FAST:
                return "매우 빠름";
            default:
                return "";
        }
    }

    public string GetDamageText()
    {
        switch (toolDamage)
        {
            case TOOL_DAMAGE.VERY_WEAK:
                return "매우 약함";
            case TOOL_DAMAGE.WEAK:
                return "약함";
            case TOOL_DAMAGE.NORMAL:
                return "보통";
            case TOOL_DAMAGE.STRONG:
                return "강함";
            case TOOL_DAMAGE.VERY_STRONG:
                return "매우 강함";
            default:
                return "";
        }
    }
    
    public void SetRate()
    {
        hitDelay = rates[(int)toolRate];
    }

    private void SetRate(TOOL_RATE attr)
    {
        hitDelay = rates[(int)attr];
    }

    public void SetRadius()
    {
        radius = radiuses[(int)toolRadius];
        transform.localScale = new Vector3(radius, radius, radius);
    }

    private void SetRadius(TOOL_RADIUS attr)
    {
        radius = radiuses[(int)attr];
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public void SetSpeed()
    {
        speed = speeds[(int)toolSpeed];
    }

    private void SetSpeed(TOOL_SPEED attr)
    {
        speed = speeds[(int)attr];
    }
    
    public void SetDamage()
    {
        damage *= (int)toolDamage + 1;
        //damageLevel = damageLevel < maxDamageLevel ? ++damageLevel : damageLevel;
    }
    #endregion

    protected void SetRateValue(float[] values)
    {
        if (values.Length != rates.Length) {
#if UNITY_EDITOR
            print("속성 값은 " + rates.Length + "개가 되어야 합니다.");
#endif
            return;
        }

        rates = values;
    }

    protected void SetRadiusValue(float[] values)
    {
        if (values.Length != radiuses.Length)
        {
#if UNITY_EDITOR
            print("속성 값은 " + radiuses.Length + "개가 되어야 합니다.");
#endif
            return;
        }

        radiuses = values;
    }

    protected void SetSpeedValue(float[] values)
    {
        if (values.Length != speeds.Length)
        {
#if UNITY_EDITOR
            print("속성 값은 " + speeds.Length + "개가 되어야 합니다.");
#endif
            return;
        }

        speeds = values;
    }

    private void Init()
    {
        transform.localScale = new Vector3(radius, radius, radius);
        canHit = true;
    }

    protected void Awake()
    {
        waitHitDelay = new WaitForSeconds(hitDelay);
        SetRate();
        SetRadius();
        SetSpeed();
        SetDamage();
        Init();
    }

    protected void FixedUpdate()
    {
        // 움직임 (모든 도구의 공통된 부분)
        if (GameManager.instance.screenManager.CurrentScreen() != SCREEN.INGAME) return;

        if (GameManager.instance.CurrentPlayer.JoyStick.IsTouch)
        {
            Move(GameManager.instance.CurrentPlayer.JoyStick.MovePosition);
        }

        // PC 테스트 전용
#if UNITY_EDITOR
        Vector2 movePos = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move(movePos.normalized);
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        // 도구마다 기능이 달라진다.
        if (Input.GetKeyDown(KeyCode.Space)) Hit();
    }
#endif

    public virtual void HitButtonDown()
    {
        // 모든 도구가 공통으로 가짐
        Hit();

#if UNITY_EDITOR
        print("Hit Button Down");
#endif
    }
    
    public virtual void HitButtonUp()
    {
        // 모든 도구가 공통으로 가짐
    }

    public virtual void SetTool()
    {
        GameManager.instance.CurrentPlayer.CurrentHitPos.GetComponent<SpriteRenderer>().sprite = toolImage;
    }

    protected void OnEnable()
    {
        Init();
    }

    public virtual void Move(Vector3 movePosition)
    {
        //// 마우스 좌표를 월드 좌표로 변환
        //Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //// 마우스 좌표에 따라 오브젝트 이동
        //transform.position = new Vector3(mPos.x * speed, mPos.y * speed, 5);

        // 스크린 벗어나지 못하게 막음
        if (transform.position.x < -(GameManager.instance.CameraSize * 2))
        {
            transform.position = new Vector2(-(GameManager.instance.CameraSize * 2), transform.position.y);
        }
        else if (transform.position.x > (GameManager.instance.CameraSize * 2))
        {
            transform.position = new Vector2((GameManager.instance.CameraSize * 2), transform.position.y);
        }

        if (transform.position.y < -GameManager.instance.CameraSize)
        {
            transform.position = new Vector2(transform.position.x, -GameManager.instance.CameraSize);
        }
        else if (transform.position.y > GameManager.instance.CameraSize)
        {
            transform.position = new Vector2(transform.position.x, GameManager.instance.CameraSize);
        }

        transform.position += new Vector3(movePosition.x * speed * Time.deltaTime,
            movePosition.y * speed * Time.deltaTime);
    }

    public void HasPlayer()
    {
        hasPlayer = true;
    }

    public virtual void Hit()
    {
        if (!canHit) return;

        // 때린 장소 표시 오브젝트 생성
        GameObject showHitObj = GameManager.instance.prefabManager.GetHit(HIT_OBJ_TYPE.SHOW_HIT);
        HitObjScript showHit = showHitObj.GetComponent<HitObjScript>();
        // 벌레 잡았는지 확인하는 오브젝트 생성
        GameObject checkHitObj = GameManager.instance.prefabManager.GetHit(HIT_OBJ_TYPE.CHECK_HIT);
        HitCheckScript checkHit = checkHitObj.GetComponent<HitCheckScript>();

        // 오브젝트 크기 변경
        showHit.ChangeInfo(radius, hitDelay);
        checkHit.ChangeInfo(radius, damage, tool);

        showHit.ChangeImage();

        // 도구 위치로 오브젝트 이동
        showHit.Show(transform.position);
        checkHit.Show(transform.position);

        // 때리기 쿨타임
        canHit = false;
        StartCoroutine(HitDelay());

        // 때리는 애니메이션 재생
        GameManager.instance.CurrentPlayer.CurrentHitPos.GetComponent<ShowHitPos>().PlayHitAnimation();

        // 때리는 소리 재생
        GameManager.instance.soundManager.EffectPlay(tool);
    }
    
    protected IEnumerator HitDelay()
    {
        yield return waitHitDelay;
        canHit = !canHit;
    }

    public void ChangePos(Vector3 pos)
    {
        transform.position = pos;
    }
}