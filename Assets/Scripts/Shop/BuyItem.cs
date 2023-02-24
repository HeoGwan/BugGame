using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CESCO;


// 도구 구매 창
public class BuyItem : MonoBehaviour
{
    public Image buyItemImage;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemRate;
    public TextMeshProUGUI ItemRadius;
    public TextMeshProUGUI ItemSpeed;
    public TextMeshProUGUI ItemPrice;
    public TextMeshProUGUI ItemInfo;
    public GameObject toolInfoObj;

    private TOOL itemToolType;
    private int itemPrice;
    private bool isShowInfo = false;

    private void Awake()
    {
        toolInfoObj.SetActive(isShowInfo);
    }

    public void Init(GameObject buyItemObj)
    {
        itemToolType = buyItemObj.GetComponent<ShopItem>().ToolType;
        itemPrice = buyItemObj.GetComponent<ShopItem>().ItemPrice;

        // buyItem의 첫번째 자식은 ItemImage이다.
        // buyItem.transform.GetChild(0).gameObject.GetComponent<Image>()
        // 구매하려는 아이템의 이미지 정보
        Tool toolInfo = buyItemObj.GetComponent<ShopItem>().ToolInfo;

        buyItemImage.sprite = toolInfo.ToolImage;

        // buyItem.GetComponent<Tool>()
        // 구매하려는 아이템의 정보
        ItemName.text = toolInfo.ToolName;
        ItemRate.text = toolInfo.GetRateText();
        ItemRadius.text = toolInfo.GetRadiusText();
        ItemSpeed.text = toolInfo.GetSpeedText();
        ItemPrice.text = itemPrice + "원";
        ItemInfo.text = toolInfo.ToolInfo;
    }

    public void Buy()
    {
        // 구매 버튼에 연결됨
        GameManager.instance.shopManager.BuyItem(itemToolType, itemPrice);
    }

    public void Cancel()
    {
        // 취소 버튼에 연결됨
        gameObject.SetActive(false);
    }

    public void ShowToolInfo()
    {
        isShowInfo = !isShowInfo;
        toolInfoObj.SetActive(isShowInfo);
    }
}
