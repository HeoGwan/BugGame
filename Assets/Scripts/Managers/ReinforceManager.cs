using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CESCO;

public class ReinforceManager : MonoBehaviour
{
    public Reinforce reinforce;

    private GameObject currentSelectTool;
    private Price price;

    public void Init()
    {
        // 초기화 코드 작성
        /*
         * 초기화를 하게 된다면 Can Select Tool에 있는 오브젝트를 전부 없앤다. 
        */
        reinforce.Init();
    }

    public void Open()
    {
        GameManager.instance.screenManager.ChangeScreen(SCREEN.REINFORCE);
        reinforce.ShowWindow();
    }

    public void Close()
    {
        GameManager.instance.screenManager.PrevScreen();
    }

    public void ReinforceToolRate()
    {
        // 연사 속도 강화
        if (price.RatePrice == -1 || GameManager.instance.CurrentPlayer.Money < price.RatePrice)
        {
            GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.CANCEL);
            return;
        }

        GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.APPLY);
        GameManager.instance.CurrentPlayer.Money -= price.RatePrice;
        GameManager.instance.toolManager.ReinforceRate(currentSelectTool);
        reinforce.ShowMoney();
        price = reinforce.ShowToolInfo();
    }

    public void ReinforceToolRadius()
    {
        // 범위 강화
        if (price.RadiusPrice == -1 || GameManager.instance.CurrentPlayer.Money < price.RadiusPrice)
        {
            GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.CANCEL);
            return;
        }

        GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.APPLY);
        GameManager.instance.CurrentPlayer.Money -= price.RadiusPrice;
        GameManager.instance.toolManager.ReinforceRadius(currentSelectTool);
        reinforce.ShowMoney();
        price = reinforce.ShowToolInfo();
    }
    
    public void ReinforceToolSpeed()
    {
        // 이동 속도 강화
        if (price.SpeedPrice == -1 || GameManager.instance.CurrentPlayer.Money < price.SpeedPrice)
        {
            GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.CANCEL);
            return;
        }

        GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.APPLY);
        GameManager.instance.CurrentPlayer.Money -= price.SpeedPrice;
        GameManager.instance.toolManager.ReinforceSpeed(currentSelectTool);
        reinforce.ShowMoney();
        price = reinforce.ShowToolInfo();
    }    
    
    public void ReinforceToolDamage()
    {
        // 이동 속도 강화
        if (price.DamagePrice == -1 || GameManager.instance.CurrentPlayer.Money < price.DamagePrice)
        {
            GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.CANCEL);
            return;
        }

        GameManager.instance.soundManager.UIEffectPlay(UI_SOUND.APPLY);
        GameManager.instance.CurrentPlayer.Money -= price.DamagePrice;
        GameManager.instance.toolManager.ReinforceDamage(currentSelectTool);
        reinforce.ShowMoney();
        price = reinforce.ShowToolInfo();
    }    


    public void AddTool(GameObject tool)
    {
        reinforce.AddTool(tool);
    }

    public void RemoveTool(GameObject tool)
    {
        reinforce.RemoveTool(tool);
    }

    public void SelectTool(ref GameObject selectTool, Price price)
    {
        currentSelectTool = selectTool;
        this.price = price;
    }
}
