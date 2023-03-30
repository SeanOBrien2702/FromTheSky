using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private int page = 0;
    private bool isReady = false;
    [SerializeField] private List<GameObject> panels = new List<GameObject>();
    private TextMeshProUGUI textTitle;
    [SerializeField] private Transform panelTransform;
    [SerializeField] private Button buttonPrev;
    [SerializeField] private Button buttonNext;

    private void Start()
    {
        textTitle = transform.GetComponentInChildren<TextMeshProUGUI>();
        buttonPrev.onClick.AddListener(Click_Prev);
        buttonNext.onClick.AddListener(Click_Next);

        foreach (Transform t in panelTransform)
        {
            panels.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }

        panels[page].SetActive(true);
        isReady = true;

        textTitle.text = panels[page].name.Replace("_", " ");
    }

    void Update()
    {
        if (panels.Count <= 0 || !isReady) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Click_Prev();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Click_Next();
    }

    public void Click_Prev()
    {
        ChangePage(-1);
    }

    public void Click_Next()
    {
        ChangePage(1);
    }

    void ChangePage(int direction)
    {
        panels[page].SetActive(false);
        page += direction;
        if(page < 0)
        {
            page = panels.Count - 1;
        }
        else if(page >= panels.Count)
        {
            page = 0;
        }
        panels[page].SetActive(true);
        textTitle.text = panels[page].name.Replace("_", " ");
    }
}
