using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIScaleOnFocus : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float duration;
    [SerializeField] float factor;
    private Vector2 originScale;

    private bool isTweening = false;
    private Sequence seq;

    private void Awake()
    {
        originScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isTweening)
            seq.Kill();

        isTweening = true;
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(originScale * factor, duration).SetEase(Ease.InOutSine));
        seq.OnComplete(() => isTweening = false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(isTweening)
            seq.Kill();

        isTweening = true;
        seq = DOTween.Sequence();
        seq.Append(transform.DOScale(originScale, duration).SetEase(Ease.InOutSine));
        seq.OnComplete(() => isTweening = false);
    }
}
