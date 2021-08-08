using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableNumber : MonoBehaviour
{
    public Image panel;
    public Text numText;
    [HideInInspector]
    public Button button;
    [HideInInspector]
    public int Number;

    private SelectableNumber[] otherRelatedNumbers;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate () { SelectNumber(); });
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void SelectNumber()
    {
        if(Number == 0)
        {
            return;
        }

        UnselectOtherRelatedNumbers();
        SelectPanel();
        UIHandler.Instance.SetNumToPlace(Number);
    }

    public void SetRelatedNumbers(SelectableNumber[] otherRelatedNumbers)
    {
        this.otherRelatedNumbers = otherRelatedNumbers;
    }

    public void SetNumber(int num)
    {
        Number = num;
        if(num == 0)
        {
            numText.text = "";
            return;
        }
        numText.text = num.ToString();
    }

    public void SelectPanel()
    {
        changeOpacity(0.5f);
    }

    public void UnselectPanel()
    {
        changeOpacity(0.15f);
    }

    private void UnselectOtherRelatedNumbers()
    {
        foreach(SelectableNumber num in otherRelatedNumbers)
        {
            num.UnselectPanel();
        }
    }

    public void changeOpacity(float opacityValue)
    {
        Color c = panel.color;
        c.a = opacityValue;
        panel.color = c;
    }
}
