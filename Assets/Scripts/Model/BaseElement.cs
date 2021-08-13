using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseElement : MonoBehaviour
{
    public void EnableElement()
    {
        gameObject.SetActive(true);
    }

    public void changeOpacity(float opacityValue)
    {
        Color c = currentImage.color;
        c.a = opacityValue;
        currentImage.color = c;
    }

    public void AnimateMovement(Vector3 fromMovement, Vector3 toMovement)
    {
        uiTweener.AnimateMovement(fromMovement, toMovement);
    }

    public void AnimateMovement(Vector3 fromMovement, Vector3 toMovement, float delay)
    {
        uiTweener.AnimateMovement(fromMovement, toMovement, delay);
    }

    public void AnimateScaling()
    {
        uiTweener.AnimateScaling();
    }

    public void AnimateScaling(float delay)
    {
        uiTweener.AnimateScaling(delay);
    }

    public void AnimateScalingReverse()
    {
        uiTweener.AnimateReverseScaling();
    }

    public void AnimateScalingReverse(float delay)
    {
        uiTweener.AnimateReverseScaling(delay);
    }

    private Image _currentImage;
    [HideInInspector]
    public Image currentImage
    {
        get
        {
            if (null == _currentImage)
            {
                _currentImage = GetComponent<Image>();
                return _currentImage;
            }
            return _currentImage;
        }

        set
        {
            _currentImage = value;
        }
    }

    private UITweener _uiTweener;
    [HideInInspector]
    public UITweener uiTweener
    {
        get
        {
            if (null == _uiTweener)
            {
                _uiTweener = GetComponent<UITweener>();
                return _uiTweener;
            }
            return _uiTweener;
        }

        set
        {
            _uiTweener = value;
        }
    }

    private RectTransform _rectTransform;
    [HideInInspector]
    public RectTransform rectTransform
    {
        get
        {
            if (null == _rectTransform)
            {
                _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
            return _rectTransform;
        }

        set
        {
            _rectTransform = value;
        }
    }
}
