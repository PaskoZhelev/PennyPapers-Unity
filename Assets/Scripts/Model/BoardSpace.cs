using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoardSpace : BaseElement, IPointerDownHandler
{
    [HideInInspector]
    public Image selectionPanel;
    [HideInInspector]
    public GameObject treasureCircle;
    [HideInInspector]
    public Image shipSkullImage;
    [HideInInspector]
    public Text numText;

    [HideInInspector]
    public bool isOccupied;
    public bool isMountain;
    public bool isSea;
    [HideInInspector]
    public bool hasShip;
    [HideInInspector]
    public bool hasSkull;
    [HideInInspector]
    public bool isEnabled;
    
    public int Number;
    
    public int row;
    public int column;
    
    // Start is called before the first frame update
    void Start()
    {
        selectionPanel = transform.Find("SelectionPanel").gameObject.GetComponent<Image>();
        treasureCircle = transform.Find("Circle").gameObject;
        shipSkullImage = transform.Find("ShipSkull").gameObject.GetComponent<Image>();
        numText = transform.Find("Number").gameObject.GetComponent<Text>();
        changeSelectionPanelOpacity(0.25f);
    }

    // Click Space
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isEnabled)
        {
            GameHandler.Instance.lastFilledSpace = this;
            GameHandler.Instance.SpaceClicked();
        }
    }

    public void SetNumber(int num)
    {
        isOccupied = true;
        Number = num;
        numText.text = num.ToString();
        numText.color = Constants.NUMBER_COLORS[num - 1];
        numText.gameObject.SetActive(true);
        GameHandler.Instance.player.numIslandSpacesFilled++;
    }

    public void PutSkull()
    {
        isOccupied = true;
        shipSkullImage.sprite = UIHandler.Instance.skullSprite;
        hasSkull = true;
        shipSkullImage.gameObject.SetActive(true);
        GameHandler.Instance.player.numIslandSpacesFilled++;
        GameHandler.Instance.player.numSkullsFilled++;
    }

    public void PutShip()
    {
        isOccupied = true;
        shipSkullImage.sprite = UIHandler.Instance.shipSprite;
        hasShip = true;
        shipSkullImage.gameObject.SetActive(true);
    }

    public void EnableSpace()
    {
        isEnabled = true;
        selectionPanel.gameObject.SetActive(true);
    }

    public void DisableSpace()
    {
        isEnabled = false;
        selectionPanel.gameObject.SetActive(false);
    }

    public void changeSelectionPanelOpacity(float opacityValue)
    {
        Color c = selectionPanel.color;
        c.a = opacityValue;
        selectionPanel.color = c;
    }
}
