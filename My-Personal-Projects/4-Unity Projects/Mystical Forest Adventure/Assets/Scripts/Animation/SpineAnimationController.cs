using UnityEngine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;

public class SpineAnimationController : MonoBehaviour
{
    #region Fields
    private SkeletonAnimation skeletonAnimation;
    private Coroutine moveCoroutine;
    [SerializeField] private GameObject paylineObject;
    #endregion 

    #region Mono & Methods
    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        ToggleSpine(false);
    }

    public void PlayAnimation(string animationName, bool loop)
    {
        if (skeletonAnimation == null || !AnimationExists(animationName))
        {
            return;
        }

        if (!skeletonAnimation.gameObject.activeInHierarchy)
            skeletonAnimation.gameObject.SetActive(true);

        // Reset previous animations before playing new one
        skeletonAnimation.AnimationState.ClearTracks();
        skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton);
    }

    public void PlayPaylineEffect(LineRenderer paylineRenderer, int index)
    {
        if (paylineRenderer == null || paylineRenderer.positionCount < 2)
        {
            return;
        }
        string animName = "payline_" + index;
        Debug.Log($"Playing animation: {animName}");
        PlayAnimation(animName, true);
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveAlongPayline(paylineRenderer));
    }

    private IEnumerator MoveAlongPayline(LineRenderer paylineRenderer)
    {
        int pointsCount = paylineRenderer.positionCount;
        if (pointsCount < 2)
        {
            yield break;
        }

        Vector3[] points = new Vector3[pointsCount];

        for (int i = 0; i < pointsCount; i++)
        {
            points[i] = paylineRenderer.GetPosition(i);
        }

        float totalDuration = 0.8f;
        float speedMultiplier = 1.2f;

        for (int i = 0; i < points.Length - 1; i++)
        {
            float duration = totalDuration / (points.Length - 1);
            float elapsedTime = 0f;

            Vector3 startPos = points[i];
            Vector3 endPos = points[i + 1];

            while (elapsedTime < duration)
            {
                float progress = Mathf.SmoothStep(0f, 1f, elapsedTime / duration); //Smooth acceleration/deceleration
                transform.position = Vector3.Lerp(startPos, endPos, progress);
                elapsedTime += Time.deltaTime * speedMultiplier;
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f); 
        StopAnimation();
    }

    public void StopAnimation()
    {
        PlayAnimation("Idle", true);
    }

    public void ToggleSpine(bool status)
    {
        paylineObject.SetActive(status);
    }

    private bool AnimationExists(string animationName)
    {
        bool exists = skeletonAnimation.skeleton.Data.FindAnimation(animationName) != null;
        return exists;
    }
    #endregion
}
