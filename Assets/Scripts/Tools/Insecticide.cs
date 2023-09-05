using CESCO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

/*
 * 살충제: 뿌린 곳에 일정 시간동안 연기를 머물게 하고
 * 그곳을 지나가는 벌레는 데미지를 입게된다.
 * HitObj: 공격 시 뜨는 공격 이미지
 * HitCheckObj: 공격을 감지하기 위한 오브젝트
 * GameManager.instance.CurrentPlayer.CurrentHitPos.GetComponent<ShowHitPos>()
 *  ㄴ 플레이어 위에 뜨는 도구 이미지
*/

public class Insecticide : Tool
{
    [SerializeField] private Sprite HitObjImage; // 연기 이미지
    [SerializeField] private Sprite ShowHitObjImage; // 연기가 설치되는 곳을 알려주는 이미지
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackDelay;

    [Header("▼ 사용량 정보")]
    [SerializeField] private int maxCount;
    [SerializeField] private GameObject showUseCanvas;
    [SerializeField] private TextMeshProUGUI showUseText;
    [SerializeField] private TextMeshProUGUI showMaxUseText;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;

    private int count = 1;

    private WaitForSeconds waitCantDelay;

    private new void Awake()
    {
        base.Awake();

        waitCantDelay = new WaitForSeconds(0.2f);

        // 남은 사용량 알려주는 텍스트 초기화
        showUseCanvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        showUseCanvas.GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        showMaxUseText.text = "/ " + maxCount;
        showUseText.text = (maxCount - count + 1).ToString();

        float[] radiusValue = { 1.4f, 1.8f, 2.2f };
        SetRadiusValue(radiusValue);
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = ShowHitObjImage;
    }

    public override void Move(Vector3 movePosition)
    {
        base.Move(movePosition);

        showUseText.transform.position = new Vector2(transform.position.x - xOffset, transform.position.y + yOffset);
        showMaxUseText.transform.position = new Vector2(transform.position.x + xOffset, transform.position.y + yOffset);
    }

    public override void Hit()
    {
        if (!canHit) return;

        // 때린 장소 표시 오브젝트 생성
        GameObject showHitObj = GameManager.instance.prefabManager.GetHit(HIT_OBJ_TYPE.SHOW_HIT);
        HitObjScript showHit = showHitObj.GetComponent<HitObjScript>();
        // 벌레 잡았는지 확인하는 오브젝트 생성
        GameObject checkHitObj = GameManager.instance.prefabManager.GetHit(HIT_OBJ_TYPE.CHECK_HIT);
        HitCheckScript checkHit = checkHitObj.GetComponent<HitCheckScript>();

        // 오브젝트 설정 변경
        showHit.ChangeInfo(radius, attackDuration);
        checkHit.ChangeInfo(radius, damage, tool, attackDuration, attackDelay, false);

        showHit.ChangeImage(HitObjImage);
        checkHit.ChangeImage(HitObjImage);

        // 도구 위치로 오브젝트 이동
        showHit.Show(transform.position);
        checkHit.Show(transform.position);


        // 때리기 쿨타임
        canHit = false;
        StartCoroutine(HitDelay());

        // 때리는 애니메이션 재생
        GameManager.instance.CurrentPlayer.CurrentHitPos.GetComponent<ShowHitPos>().PlayHitAnimation(tool, hitDelay);

        // 때리는 소리 재생
        GameManager.instance.soundManager.EffectPlay(tool);

        // 플레이어에게 더이상 사용할 수 없음을 알려줌
        if (count >= maxCount)
        {
            StartCoroutine(CantUse());
            count = 1;
        }
        else
        {
            showUseText.text = (maxCount - count).ToString();
            ++count;
        }
    }
    IEnumerator CantUse()
    {
        yield return waitCantDelay;
        GameManager.instance.CurrentPlayer.CantUse(gameObject);
    }
}