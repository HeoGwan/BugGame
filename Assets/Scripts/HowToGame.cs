using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToGame : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject nextButton;

    private int page = 0;

    void ClosePages()
    {
        foreach (GameObject pageObj in pages)
        {
            if (pageObj.activeSelf) { pageObj.SetActive(false); }
        }
    }

    private void OnEnable()
    {
        page = 0;
        prevButton.SetActive(false);
        nextButton.SetActive(true);
        ClosePages();

        pages[page].SetActive(true);
    }

    public void PrevPage()
    {
        ShowPage(-1);
        if (page == 0) { prevButton.SetActive(false); }
        else if (!nextButton.activeSelf) { nextButton.SetActive(true); }
    }

    public void NextPage()
    {
        ShowPage(1);
        if (page == pages.Length - 1) { nextButton.SetActive(false); }
        else if (!prevButton.activeSelf) { prevButton.SetActive(true); }
    }

    public void ShowPage(int nextPage)
    {
        pages[page].SetActive(false);
        page += nextPage;
        pages[page].SetActive(true);
    }

    public void ExitRule()
    {
        ClosePages();
        GameManager.instance.screenManager.PrevScreen();
    }
}
