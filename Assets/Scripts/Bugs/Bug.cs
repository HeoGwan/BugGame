using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CESCO;
using UnityEngine.UI;

public class Bug : MonoBehaviour
{
    [Header("Bug Infos")]
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float backSpeed = 1.5f;
    [SerializeField] private float size = 1f;
    [SerializeField] protected float cycle;
    [SerializeField] protected float height;
    [SerializeField] private BUG_TYPE bugType;
    [SerializeField] private float deathDelay = 0.5f;
    [SerializeField] private Animator anim;
    [SerializeField] private float[] hp = { 10, 20, 30, 40, 50 };
    [SerializeField] private float ouchDelay;
    [SerializeField] private GameObject hpCanvas;

    public GameObject HpCanvas { get { return hpCanvas; } }

    private bool isCollision = false;
    private bool isMoving = true;
    protected SpriteRenderer sprite;
    GameObject hpImage;
    GameObject hpBackgroundImage;

    ParticleSystem ps;
    protected Vector3 direction;
    protected Vector3 dirVec;
    protected float angle;
    protected float speed;
    protected float prevSpeed;
    protected float speedDelay;

    private float healthPoint;
    public float HP { get { return healthPoint; } }

    public BUG_TYPE BugType
    {
        get { return bugType; }
    }

    //protected virtual void Init()
    //{
    //    //// 카메라에 해당하는 좌표 얻어오기
    //    //float yPos = Camera.main.orthographicSize;
    //    //float xPos = yPos * Camera.main.aspect;

    //    //// 카메라 내에서 랜덤한 좌표로 벌레 생성
    //    //float randomX = Random.Range(-xPos, xPos);
    //    //float randomY = Random.Range(-yPos, yPos);
    //    //transform.position = new Vector2(randomX, randomY);
    //    transform.localScale = new Vector2(this.size, this.size);

    //    // 필요한 변수 초기화
    //    isCollision = false;
    //    isMoving = true;
    //    ps = GetComponent<ParticleSystem>();
    //    direction = transform.position - target.position;
    //    dirVec = direction.normalized;
    //    angle = 0.0f;
    //}

    public GameObject SetBug(Vector2 position)
    {
        // 벌레 소환 시 설정하는 부분
        transform.position = position;
        transform.localScale = new Vector2(this.size, this.size);

        direction = transform.position - GameManager.instance.CurrentTarget.transform.position;
        dirVec = direction.normalized;

        healthPoint = hp[GameManager.instance.Level - 1];

        return gameObject;
    }

    private void Awake()
    {
        // 필요한 변수 초기화
        isCollision = false;
        isMoving = true;
        ps = GetComponent<ParticleSystem>();
        angle = 0.0f;
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        prevSpeed = speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        if (!isMoving) { return; }

        direction = transform.position - GameManager.instance.CurrentTarget.transform.position;
        dirVec = direction.normalized;

        sprite.flipY = transform.position.x < GameManager.instance.CurrentTarget.transform.position.x ? true : false;

        // 충돌 검사 및 움직임
        if (isCollision)
        {
            // 타겟에 부딪혔을 경우
            Vector2 _target = transform.position + direction;
            transform.position = Vector2.MoveTowards(transform.position, _target, speed * backSpeed * Time.deltaTime);
        }
        else
        {
            // 움직임
            Move();
        }

        hpBackgroundImage.transform.position = hpImage.transform.position =
            (new Vector2(transform.position.x, transform.position.y + 0.8f));
    }

    protected virtual void Move()
    {
        transform.position =
            Vector2.MoveTowards(transform.position,
            GameManager.instance.CurrentTarget.transform.position, speed * Time.deltaTime);
        LookTarget();
    }

    protected virtual void LookTarget()
    {
        angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 1);
        transform.rotation = rotation;
    }

    private void OnEnable()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        isCollision = false;
        isMoving = true;
        anim.SetBool("Death", false);
        prevSpeed = speed = Random.Range(minSpeed, maxSpeed);
        if (hpImage != null) hpImage.GetComponent<Image>().fillAmount = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            isCollision = true;
            StartCoroutine(Collision());
        }
    }

    public void HitDamage(float damage, TOOL toolType, float speedDelay)
    {
        this.speedDelay = damage == 0 ? speedDelay : ouchDelay;

        if (toolType == TOOL.TRAP)
        {
            StartCoroutine(Stop());
            return;
        }

        ps.Play();
        healthPoint -= damage;
        hpImage.GetComponent<Image>().fillAmount = healthPoint / hp[GameManager.instance.Level - 1];

        if (healthPoint <= 0)
        {
            // 체력이 다 떨어져 사망 시 
            isMoving = false;
            GameManager.instance.scoreManager.PlusScore();
            GetComponent<BoxCollider2D>().enabled = false;
            anim.SetBool("Death", true);
            StartCoroutine(Death());
        }
        else
        {
            StartCoroutine(Ouch());
        }
    }

    public void SetHPCanvas()
    {
        // 체력바 설정
        hpCanvas = transform.GetChild(0).gameObject;
        hpCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        hpCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    public void SetHPBar(GameObject hpBGObj, GameObject hpObj)
    {
        hpBackgroundImage = hpBGObj;
        hpImage = hpObj;
    }

    IEnumerator Collision()
    {
        yield return new WaitForSeconds(0.5f);
        isCollision = false;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(deathDelay);
        gameObject.SetActive(false);
    }

    IEnumerator Ouch()
    {
        speed /= 2;

        yield return new WaitForSeconds(speedDelay);

        speed = prevSpeed;
    }

    IEnumerator Stop()
    {
        speed = 0;

        yield return new WaitForSeconds(speedDelay);

        speed = prevSpeed;
    }
}
