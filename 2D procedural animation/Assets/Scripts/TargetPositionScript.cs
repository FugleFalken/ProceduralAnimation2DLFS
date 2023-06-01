using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TargetPositionScript : MonoBehaviour
{
    private UnityEngine.Vector2 targetPoint;
    private UnityEngine.Vector2 currentPosition;
    private UnityEngine.Vector2 oldPosition;
    private UnityEngine.Vector2 newPosition;
    private UnityEngine.Vector2 rayCastOrigin;
    private bool canMove;
    private bool goingRight;
    [SerializeField] private float stepOffSet;
    [SerializeField] private float lerp;
    [SerializeField] private float steppingSpeed;
    [SerializeField] private float steppingHeight;
    [SerializeField] private bool isRightFoot;

    [field:SerializeField]
    public Transform hip { get; set; }
    [field:SerializeField]
    public float StepLength { get; set; }
    public UnityEvent OnFootPlaced;


    // Start is called before the first frame update
    void Start()
    {
        newPosition = targetPoint;
        CalculateStep();
        currentPosition = targetPoint;
        oldPosition = targetPoint;
        if (isRightFoot) canMove = true;
    }

    void Update()
    {
        transform.position = currentPosition;
        if(canMove) CalculateStep();
    }

    private void CalculateStep()
    {
        RaycastHit2D[] rayHits = Physics2D.RaycastAll(rayCastOrigin, UnityEngine.Vector2.down);
        foreach (RaycastHit2D rayHit in rayHits)
        {
            if (rayHit.transform.tag == "Ground")
            {
                //Debug.Log(UnityEngine.Vector2.Distance(currentPosition, targetPoint));
                //Debug.Log(UnityEngine.Vector2.Distance(oldPosition, targetPoint));
                targetPoint = rayHit.point;
                if (UnityEngine.Vector2.Distance(newPosition, targetPoint) > StepLength)
                {
                    Debug.Log(UnityEngine.Vector2.Distance(newPosition, targetPoint) + " " + isRightFoot);
                    newPosition = targetPoint;
                    lerp = 0;
                }
                if (lerp < 1)
                {
                    currentPosition = UnityEngine.Vector2.Lerp(oldPosition, newPosition, lerp);
                    currentPosition.y += Mathf.Sin(lerp * Mathf.PI) * steppingHeight;
                    lerp += Time.deltaTime * steppingSpeed;
                }
                else
                {
                    oldPosition = newPosition;
                    SwitchFootMovementStatus();
                    OnFootPlaced?.Invoke();
                }
                break;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(currentPosition, 0.1f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(oldPosition, 0.1f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPoint, 0.1f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(rayCastOrigin, 0.1f);
    }

    public void SwitchFootMovementStatus()
    {
        canMove = !canMove;
    }
    public void GetDirection(float direction)
    {
        rayCastOrigin = hip.position;
        rayCastOrigin.x += stepOffSet * direction;
    }
}
