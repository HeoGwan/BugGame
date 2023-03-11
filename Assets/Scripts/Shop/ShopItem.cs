using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CESCO;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private int itemPrice;
    [SerializeField] private TOOL toolType;
    private bool isLock = true;

    private Tool toolInfo;

    #region Getter
    public int ItemPrice
    {
        get { return itemPrice; }
    }

    public TOOL ToolType
    {
        get { return toolType; }
    }

    public bool IsLock
    {
        get { return isLock; }
    }

    public Tool ToolInfo
    {
        get { return toolInfo; }
    }
    #endregion


    public void UnLock()
    {
        isLock = false;
    }

    public void Lock()
    {
        isLock = true;
    }

    public void SetItemInfo(ShopItem copy)
    {
        itemPrice = copy.ItemPrice;
        toolType = copy.ToolType;
        isLock = copy.IsLock;
        toolInfo = GameManager.instance.toolManager.GetToolInfo(copy.ToolType);
        SetToolImage();
    }

    public void SetToolImage()
    {
        transform.GetChild(0).GetComponent<Image>().sprite =
        GameManager.instance.toolManager.GetToolImage(toolType);
    }
}
