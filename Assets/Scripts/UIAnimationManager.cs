using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIAnimationManager : MonoBehaviour
{

    public float newScoreTextXPercent = 50;
    public LTBezierPath bezier;

    public AnimationCurve pointsMovementCurve;

    public Color color;

    public void PointsAnimation(bool moveRight, GameObject obj)
    {
        //LeanTween.move(obj, new Vector2(obj.transform.position.x + 50 * (moveRight ? 1 : -1), obj.transform.position.y + 200), 1).setEase(pointsMovementCurve);
        obj.LeanMove(new Vector2(obj.transform.position.x + 50 * (moveRight ? 1 : -1), obj.transform.position.y + 200), 1).setEase(pointsMovementCurve);
        ChangeColor(color, obj, 1);
    }

    public void ChangeColor(Color to, GameObject obj, float time)
    {
        obj.LeanColor(to, time);
    }

}
