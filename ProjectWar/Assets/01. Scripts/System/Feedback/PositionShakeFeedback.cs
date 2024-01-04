using Base.Feedback;
using DG.Tweening;
using UnityEngine;

public class PositionShakeFeedback : FeedbackBase
{
    [SerializeField] Transform target = null;
    [SerializeField] float duration = 1f;
    [SerializeField] float power = 3f;
    [SerializeField] int frequency = 5;

    private Vector3 originPosition;

    private void Awake()
    {
        originPosition = transform.position;
    }

    // private void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //         CreateFeedback();
    // }

    public override void CreateFeedback()
    {
        target.DOShakePosition(duration, power, frequency);
    }

    public override void FinishFeedback()
    {
        target.DOKill();
        transform.position =originPosition;
    }
}