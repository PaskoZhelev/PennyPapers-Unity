using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Die : MonoBehaviour
{
    public DieType DieType;

    [HideInInspector]
    public Image image;

    [HideInInspector]
    public int Number;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void RollDieAndSetValue(int value)
    {
        StartCoroutine(RollDieCoroutine(value));
    }

    private IEnumerator RollDieCoroutine(int value)
    {
        // only set values to the dice (no images)
        // to avoid concurrency issues
        // numberDie should start from 1
        Number = value + 1;

        float countDown = 0.3f;
        for (int i = 0; i < 10; i++)
        {
            while (countDown >= 0)
            {
                transform.Rotate(Vector3.back, Constants.ANIMATION_SPEED * Time.deltaTime);
                countDown -= Time.deltaTime;
                yield return null;
            }
        }

        setDieImage(value);
    }

    private void setDieImage(int value)
    {
        if(value < 5)
        {
            image.sprite = DiceManager.Instance.NumberSprites[value];
        } else
        {
            switch (DieType)
            {
                case DieType.PURPLE:
                    image.sprite = DiceManager.Instance.PurpleSideDie;
                    break;
                case DieType.GREEN:
                    image.sprite = DiceManager.Instance.GreenSideDie;
                    break;
                default:
                    image.sprite = DiceManager.Instance.RedSideDie;
                    break;
            }
        }
        
        transform.rotation = Quaternion.identity;
    }
}
