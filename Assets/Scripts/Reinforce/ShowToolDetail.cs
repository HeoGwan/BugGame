using CESCO;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct Price {
    public Price(int ratePrice, int radiusPrice, int speedPrice, int damagePrice)
    {
        RatePrice = ratePrice;
        RadiusPrice = radiusPrice;
        SpeedPrice = speedPrice;
        DamagePrice = damagePrice;
    }

    public int RatePrice { get; set; }
    public int RadiusPrice { get; set; }
    public int SpeedPrice { get; set; }
    public int DamagePrice { get; set; }
}

public class ShowToolDetail : MonoBehaviour
{
    private Sprite sprite;
    private Tool selectToolScript;

    public Image toolImage;
    
    [Header("▼ 도구 속성 정보")]
    public TextMeshProUGUI toolName;
    public TextMeshProUGUI toolRateOfHit;
    public TextMeshProUGUI toolRadius;
    public TextMeshProUGUI toolSpeed;
    public TextMeshProUGUI toolDamage;
    
    [Header("▼ 강화 버튼")]
    public Button RateButton;
    public Button RadiusButton;
    public Button SpeedButton;
    public Button DamageButton;

    [Header("▼ 강화 금액 정보")]
    public TextMeshProUGUI ratePriceText;
    public TextMeshProUGUI radiusPriceText;
    public TextMeshProUGUI speedPriceText;
    public TextMeshProUGUI damagePriceText;

    [Header("▼ 도구 정보")]
    [SerializeField] GameObject toolInfoObj;
    [SerializeField] TextMeshProUGUI toolInfo;

    Price basePrice;
    Price price;
    bool isShowToolInfo = false;

    private void Awake()
    {
        price = new Price(0, 0, 0, 0);
        toolInfoObj.SetActive(false);
    }

    public void SelectTool(GameObject selectTool)
    {
        /*
         * 상점에서 도구 구매 후 강화 시 바로 제일 높은 단계로 강화되는 버그 발생
        */
        sprite = selectTool.GetComponent<SpriteRenderer>().sprite;
        selectToolScript = selectTool.GetComponent<Tool>();

        // transform.GetChild(0) : Selected Tool
        // transform.GetChild(1) : Tool Info
        // 이미지를 바꾸려면 transform.GetChild(0).GetChild(0)으로 접근해야 한다.
        toolImage.sprite = sprite;

        // 도구 정보
        toolName.text = selectToolScript.ToolName;
        toolInfo.text = selectToolScript.ToolInfo;

        basePrice.RatePrice = selectToolScript.RatePrice;
        basePrice.RadiusPrice = selectToolScript.RadiusPrice;
        basePrice.SpeedPrice = selectToolScript.SpeedPrice;
        basePrice.DamagePrice = selectToolScript.DamagePrice;

        ShowAttributes();
        ShowPrices();

        GameManager.instance.reinforceManager.SelectTool(ref selectTool, price);
    }

    public void ShowAttributes()
    {
        toolRateOfHit.text = selectToolScript.GetRateText();
        toolRadius.text = selectToolScript.GetRadiusText();
        toolSpeed.text = selectToolScript.GetSpeedText();
        toolDamage.text = selectToolScript.GetDamageText();
    }

    public Price ShowPrices()
    {
        price.RatePrice = GetPrice(selectToolScript.ToolRate);
        price.RadiusPrice = GetPrice(selectToolScript.ToolRadius);
        price.SpeedPrice = GetPrice(selectToolScript.ToolSpeed);
        price.DamagePrice = GetPrice(selectToolScript.ToolDamage);

        ratePriceText.text = price.RatePrice == -1 ? "강화 불가" : price.RatePrice + "원";
        radiusPriceText.text = price.RadiusPrice == -1 ? "강화 불가" : price.RadiusPrice + "원";
        speedPriceText.text = price.SpeedPrice == -1 ? "강화 불가" : price.SpeedPrice + "원";
        damagePriceText.text = price.DamagePrice == -1 ? "강화 불가" : price.DamagePrice + "원";

        return price;
    }

    private int GetPrice(TOOL_RATE toolAttr)
    {
        if (toolAttr == TOOL_RATE.SUPER_FAST)
        {
            return -1;
        }

        return basePrice.RatePrice + (int)(basePrice.RatePrice * 0.5f * (int)toolAttr);
        //switch (toolAttr)
        //{
        //    case TOOL_RATE.SUPER_SLOW:
        //        return basePrice.RatePrice;
        //    case TOOL_RATE.SLOW:
        //        return basePrice.RatePrice + (int)(basePrice.RatePrice * 0.5f * (int)toolAttr);
        //    case TOOL_RATE.NORMAL:
        //        return 20000;
        //    case TOOL_RATE.FAST:
        //        return 25000;
        //    default:
        //        return -1;
        //}
    }
    
    private int GetPrice(TOOL_SPEED toolAttr)
    {
        if (toolAttr == TOOL_SPEED.SUPER_FAST)
        {
            return -1;
        }

        return basePrice.SpeedPrice + (int)(basePrice.SpeedPrice * 0.5f * (int)toolAttr);

        //switch (toolAttr)
        //{
        //    case TOOL_SPEED.SUPER_SLOW:
        //        return basePrice.SpeedPrice;
        //    case TOOL_SPEED.SLOW:
        //        return 15000;
        //    case TOOL_SPEED.NORMAL:
        //        return 20000;
        //    case TOOL_SPEED.FAST:
        //        return 25000;
        //    default:
        //        return -1;
        //}
    }

    private int GetPrice(TOOL_RADIUS toolAttr)
    {
        if (toolAttr == TOOL_RADIUS.LARGE)
        {
            return -1;
        }

        return basePrice.RadiusPrice + (int)(basePrice.RadiusPrice * 0.5f * (int)toolAttr);
        //switch (toolAttr)
        //{
        //    case TOOL_RADIUS.SMALL:
        //        return basePrice.RadiusPrice;
        //    case TOOL_RADIUS.MEDIUM:
        //        return 20000;
        //    default:
        //        return -1;
        //}
    }

    private int GetPrice(TOOL_DAMAGE toolAttr)
    {
        if (toolAttr == TOOL_DAMAGE.VERY_STRONG)
        {
            return -1;
        }

        return basePrice.DamagePrice + (int)(basePrice.DamagePrice * 0.5f * (int)toolAttr);
        //switch (toolAttr)
        //{
        //    case TOOL_DAMAGE.VERY_WEAK:
        //        return basePrice.DamagePrice;
        //    case TOOL_DAMAGE.WEAK:
        //        return 15000;
        //    case TOOL_DAMAGE.NORMAL:
        //        return 20000;
        //    case TOOL_DAMAGE.STRONG:
        //        return 25000;
        //    default:
        //        return -1;
        //}
    }

    public void ShowToolInfo()
    {
        isShowToolInfo = !isShowToolInfo;
        toolInfoObj.SetActive(isShowToolInfo);
    }
}
