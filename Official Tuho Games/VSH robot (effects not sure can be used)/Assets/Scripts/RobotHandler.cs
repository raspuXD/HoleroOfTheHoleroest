using System.Collections;
using UnityEngine;

public class RobotHandler : MonoBehaviour
{
    public enum RobotState { None, MovingToMiddle, MovingToPortal, MovingToEnd, AtMiddle }
    public RobotState currentState = RobotState.None;

    public Transform startPoint, middlePoint, portalPoint, endPoint;
    public GameObject robotToSpawn;
    public GameObject currentRobot;

    public float moveSpeed = 5f;

    Coroutine movingRoutine = null;

    public AudioSource green, red, stopSound;
    public ConverBelt[] belts;

    public  BrokenPart[] brokenParts;
    public DoesPartMiss DoesPartMiss;

    [Header("Camera objects")]
    public GameObject cameraHead;

    private void Update()
    {
        foreach (var belt in belts)
        {
            belt.ChangeSpeed(currentState != RobotState.AtMiddle);
        }
    }

    void SpawnRobot()
    {
        if (currentRobot != null)
        {
            Destroy(currentRobot);
        }
        currentRobot = Instantiate(robotToSpawn, startPoint.position, Quaternion.identity);
        currentState = RobotState.None;

        DoesPartMiss = currentRobot.GetComponentInChildren<DoesPartMiss>();
        DoesPartMiss.theCameraObject = cameraHead;
        brokenParts = currentRobot.GetComponentsInChildren<BrokenPart>();

        MoveRobotForward();
    }

    public void MoveRobotForward()
    {
        if (currentRobot == null)
        {
            SpawnRobot();
        }

        if (currentState == RobotState.AtMiddle && IsFixedFully() && DoesPartMiss.IsThePartPut())
        {
            StartMoving(MoveToEndPoint(), RobotState.MovingToEnd, green);
        }
        else if (currentState == RobotState.None)
        {
            StartMoving(MoveToMiddlePoint(), RobotState.MovingToMiddle, green);
        }
    }

    public void MoveRobotToPortal()
    {
        if (currentState == RobotState.AtMiddle && IsFixedFully() && DoesPartMiss.IsThePartPut())
        {
            StartMoving(MoveToPortal(), RobotState.MovingToPortal, red);
        }
    }

    private void StartMoving(IEnumerator routine, RobotState newState, AudioSource sound)
    {
        if (movingRoutine != null) StopCoroutine(movingRoutine);
        movingRoutine = StartCoroutine(routine);
        currentState = newState;
        sound.Play();
    }

    public bool IsFixedFully()
    {
        foreach (var part in brokenParts)
        {
            if (!part.hasBeenFixed)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator MoveToMiddlePoint()
    {
        while (currentRobot != null)
        {
            Vector3 direction = (middlePoint.position - currentRobot.transform.position).normalized;
            currentRobot.transform.position = Vector3.MoveTowards(
                currentRobot.transform.position,
                middlePoint.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(currentRobot.transform.position, middlePoint.position) < 0.1f)
            {
                currentState = RobotState.AtMiddle;
                movingRoutine = null;
                stopSound.Play();
                yield break;
            }
            yield return null;
        }
    }


    private IEnumerator MoveToPortal()
    {
        while (currentRobot != null)
        {
            Vector3 direction = (portalPoint.position - currentRobot.transform.position).normalized;
            currentRobot.transform.position = Vector3.MoveTowards(
                currentRobot.transform.position,
                portalPoint.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    private IEnumerator MoveToEndPoint()
    {
        while (currentRobot != null)
        {
            Vector3 direction = (endPoint.position - currentRobot.transform.position).normalized;
            currentRobot.transform.position = Vector3.MoveTowards(
                currentRobot.transform.position,
                endPoint.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }
    }
}
