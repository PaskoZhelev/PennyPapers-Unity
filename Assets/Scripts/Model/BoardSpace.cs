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
    public Text treasureCircle;
    [HideInInspector]
    public Image shipSkullImage;
    [HideInInspector]
    public Text numText;
    [HideInInspector]
    public GameObject cross;

    public bool isOccupied;
    public bool isMountain;
    public bool isSea;
    public bool hasShip;
    [HideInInspector]
    public bool hasSkull;
    [HideInInspector]
    public bool isEnabled;
    [HideInInspector]
    public bool hasTreasure;
    [HideInInspector]
    public int treasureValue;

    public int Number;
    
    public int row;
    public int column;
    
    // Start is called before the first frame update
    void Start()
    {
        selectionPanel = transform.Find("SelectionPanel").gameObject.GetComponent<Image>();
        treasureCircle = transform.Find("Circle").gameObject.GetComponent<Text>();
        shipSkullImage = transform.Find("ShipSkull").gameObject.GetComponent<Image>();
        numText = transform.Find("Number").gameObject.GetComponent<Text>();
        cross = transform.Find("Cross").gameObject;
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

    public void PutNumber(int num)
    {
        isOccupied = true;
        Number = num;
        numText.text = num.ToString();
        numText.color = Constants.NUMBER_COLORS[num - 1];
        numText.gameObject.SetActive(true);
        GameHandler.Instance.player.numIslandSpacesFilled++;
        GameHandler.Instance.AddPossibleAdjacentIslandSpaces(row, column);
        GameHandler.Instance.CheckForTreasure(this);
        GameHandler.Instance.OvercomeSkullWhenPossible(this);
    }

    public void PutSkull()
    {
        isOccupied = true;
        shipSkullImage.sprite = UIHandler.Instance.skullSprite;
        hasSkull = true;
        shipSkullImage.gameObject.SetActive(true);
        GameHandler.Instance.player.numIslandSpacesFilled++;
        GameHandler.Instance.player.numSkullsFilled++;
        GameHandler.Instance.spacesWithSkull.Add(this);
        GameHandler.Instance.OvercomeSkullWhenPossible(this);
    }

    public void PutShip()
    {
        isOccupied = true;
        shipSkullImage.sprite = UIHandler.Instance.shipSprite;
        hasShip = true;
        shipSkullImage.gameObject.SetActive(true);
        GameHandler.Instance.AddPossibleAdjacentIslandSpaces(row, column);
        GameHandler.Instance.CheckForTreasure(this);
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

    public void ActivateTreasureCircle(int num)
    {
        hasTreasure = true;
        treasureValue = num;
        treasureCircle.gameObject.SetActive(true);
        treasureCircle.color = Constants.NUMBER_COLORS[num - 1];
    }

    public void ActivateCross()
    {
        cross.SetActive(true);
    }

    public void changeSelectionPanelOpacity(float opacityValue)
    {
        Color c = selectionPanel.color;
        c.a = opacityValue;
        selectionPanel.color = c;
    }
}
