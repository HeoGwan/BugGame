using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CESCO;

public class ToolManager : MonoBehaviour
{
    public GameObject[] toolObjs;

    private List<GameObject> tools;
    private List<TOOL> toolTypes;

    private void Awake()
    {
        tools = new List<GameObject>();
        toolTypes = new List<TOOL>();

        foreach (GameObject toolObj in toolObjs)
        {
            GameObject tool = Instantiate(toolObj, transform);
            tool.SetActive(false);
            AddTool(tool);
        }
    }

    void AddTool(GameObject tool)
    {
        tools.Add(tool);
        toolTypes.Add(tool.GetComponent<Tool>().ToolType);
        tool.transform.SetParent(transform);
    }

    public void ReturnTool(GameObject tool)
    {
        tool.SetActive(false);
        AddTool(tool);
    }

    public GameObject GetTool(TOOL tool)
    {
        int index = toolTypes.IndexOf(tool);
        return tools[index];
    }

    public Tool GetToolInfo(TOOL tool)
    {
        int index = toolTypes.IndexOf(tool);
        return tools[index].GetComponent<Tool>();
    }

    public Sprite GetToolImage(TOOL tool)
    {
        int index = toolTypes.IndexOf(tool);
        return tools[index].GetComponent<SpriteRenderer>().sprite;
    }


    public GameObject GivePlayer(TOOL tool)
    {
        int index = toolTypes.IndexOf(tool);
        return GameManager.instance.CurrentPlayer.GetTool(tool, tools[index]);
    }

    public GameObject GiveTool(TOOL tool)
    {
        int index = toolTypes.IndexOf(tool);
        return tools[index];
    }

    public void ReinforceRate(GameObject tool)
    {
        if (tool.GetComponent<Tool>().ToolRate == TOOL_RATE.SUPER_FAST) return;

        tool.GetComponent<Tool>().ToolRate += 1;
        tool.GetComponent<Tool>().SetRate();
    }

    public void ReinforceRadius(GameObject tool)
    {
        if (tool.GetComponent<Tool>().ToolRadius == TOOL_RADIUS.LARGE) return;

        tool.GetComponent<Tool>().ToolRadius += 1;
        tool.GetComponent<Tool>().SetRadius();
    }

    public void ReinforceSpeed(GameObject tool)
    {
        if (tool.GetComponent<Tool>().ToolSpeed == TOOL_SPEED.SUPER_FAST) return;

        tool.GetComponent<Tool>().ToolSpeed += 1;
        tool.GetComponent<Tool>().SetSpeed();
    }

    public void ReinforceDamage(GameObject tool)
    {
        if (tool.GetComponent<Tool>().ToolDamage == TOOL_DAMAGE.VERY_STRONG) return;

        tool.GetComponent<Tool>().ToolDamage += 1;
        tool.GetComponent<Tool>().SetDamage();
    }
}
