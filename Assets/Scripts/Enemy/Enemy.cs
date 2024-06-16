using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Fields

    [SerializeField] private Transform player;

    [SerializeField] private float speed = 5;
    [SerializeField] private float waitTime = .3f;
    [SerializeField] private float turnSpeed = 60;

    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private Transform pathHolder;
    [SerializeField] private bool comeAndGo;
    private bool _reverse;

    #endregion

    #region Constants

    private const int INITIAL_ARRAY = 0;
    private const int INITIAL_INDEX = 0;
    private const int ONE_ELEMENT = 1;
    private const float NEGLIGIBLE_ANGLE_DIFF = 0.05f;
    private const int ZERO_INIT_ADJUSTMENT = 1;

    private const int FIRST_SPHERE = 0;
    private const int FIRST_LINE = 1;
    private const float FIRST_SPHERE_RAD = 0.5f;
    private const float SPHERE_RAD = 0.2f;
    private const float FIRST_LINE_THICK = 4f;
    private const float LINE_THICK = 1f;
    private const int HANDLE_CONTROL = 0;
    private const float HALF_ANGLE = 0.5f;

    private readonly Color _firstNodeColor = new Color(0, 255, 0);
    private readonly Color _nodesColor = new Color(0, 255, 0, 0.5f);
    private readonly Color _viewAreaColor = new Color(0, 255, 255, 0.5f);

    #endregion

    #region Events

    public static event System.Action OnPlayerSpotted;

    #endregion

    #region UnityMethods

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        var waypoints = pathHolder.GetComponentsInChildren<Transform>()
            .Where(x => x != pathHolder.transform).ToArray();

        StartCoroutine(PatrolPath(waypoints));
    }

    private void Update()
    {
        if (!CanSeePlayer()) return;
        OnPlayerSpotted?.Invoke();
    }

    #endregion

    #region Methods

    private bool CanSeePlayer()
    {
        var enemyT = transform;
        if (!(Vector3.Distance(enemyT.position, player.position) < viewDistance)) return false;

        var directionToPlayer = (player.position - enemyT.position).normalized;
        var angleToPlayer = Vector3.Angle(enemyT.forward, directionToPlayer);
        return angleToPlayer < viewAngle * HALF_ANGLE;
    }

    private IEnumerator PatrolPath(IReadOnlyList<Transform> waypoints)
    {
        var targetWaypointIndex = INITIAL_ARRAY;
        var targetWaypoint = GetWaypoint(waypoints, targetWaypointIndex);
        var waypointsQty = waypoints.Count();
        transform.position = targetWaypoint;
        if (waypointsQty > ONE_ELEMENT)
        {
            targetWaypointIndex = GetNewWaypointIndex(waypoints, targetWaypointIndex);
            targetWaypoint = GetWaypoint(waypoints, targetWaypointIndex);
            transform.LookAt(targetWaypoint);

            while (true)
            {
                transform.position = MoveToWaypoint(targetWaypoint);
                if (transform.position != targetWaypoint)
                {
                    yield return null;
                }
                else
                {
                    targetWaypointIndex = GetNewWaypointIndex(waypoints, targetWaypointIndex);
                    targetWaypoint = GetWaypoint(waypoints, targetWaypointIndex);
                    yield return new WaitForSeconds(waitTime);
                    yield return StartCoroutine(FaceTarget(targetWaypoint));
                }
            }
        }
        else
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(Rotate());
            }
        }
    }

    private Vector3 GetWaypoint(IReadOnlyList<Transform> waypoints, int targetWaypointIndex)
    {
        return waypoints[targetWaypointIndex].position;
    }

    private Vector3 MoveToWaypoint(Vector3 targetWaypoint)
    {
        return Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
    }

    private int GetNewWaypointIndex(IEnumerable<Transform> waypoints, int currentWaypointIndex)
    {
        var waypointsQty = waypoints.Count();
        if (waypointsQty <= ONE_ELEMENT) return INITIAL_ARRAY;
        if (!comeAndGo)
        {
            var nextWaypointIndex = currentWaypointIndex + ONE_ELEMENT;
            nextWaypointIndex %= waypointsQty;
            return nextWaypointIndex;
        }

        if (!_reverse)
        {
            var nextWaypointIndex = currentWaypointIndex + ONE_ELEMENT;
            if (nextWaypointIndex >= waypointsQty - ZERO_INIT_ADJUSTMENT)
            {
                _reverse = !_reverse;
            }

            return nextWaypointIndex;
        }
        else
        {
            var nextWaypointIndex = currentWaypointIndex - ONE_ELEMENT;
            if (nextWaypointIndex <= INITIAL_ARRAY)
            {
                _reverse = !_reverse;
            }

            return nextWaypointIndex;
        }
    }

    private IEnumerator FaceTarget(Vector3 targetPos)
    {
        var directionToLook = (targetPos - transform.position).normalized;
        var targetAngle = viewAngle - Mathf.Atan2(directionToLook.z, directionToLook.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > NEGLIGIBLE_ANGLE_DIFF)
        {
            var angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private IEnumerator Rotate()
    {
        var targetAngle = transform.rotation.eulerAngles.y;
        targetAngle += viewAngle;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > NEGLIGIBLE_ANGLE_DIFF)
        {
            var angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    #endregion

    #region Debug

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        var waypoints = pathHolder.GetComponentsInChildren<Transform>().Where(x => x != pathHolder.transform).ToArray();
        var enemyTransform = transform;
        var forward = enemyTransform.forward;

        var from = Quaternion.AngleAxis(-HALF_ANGLE * viewAngle, Vector3.up) * (
            forward - Vector3.Dot(forward, Vector3.up) * Vector3.up
        );
        Handles.color = _viewAreaColor;
        Handles.DrawSolidArc(transform.position, enemyTransform.up, from, viewAngle, viewDistance);

        var previousPosition = waypoints[INITIAL_ARRAY].position;
        for (var index = INITIAL_INDEX; index < waypoints.Length; index++)
        {
            var waypoint = waypoints[index];
            switch (index)
            {
                case FIRST_SPHERE:
                    Handles.color = _firstNodeColor
                        ;
                    Handles.SphereHandleCap(HANDLE_CONTROL, waypoints[INITIAL_ARRAY].position, Quaternion.identity,
                        FIRST_SPHERE_RAD,
                        EventType.Repaint);
                    break;
                case FIRST_LINE:
                    Handles.DrawLine(previousPosition, waypoint.position, FIRST_LINE_THICK);
                    Handles.color = _nodesColor;
                    break;
                default:
                    Handles.SphereHandleCap(HANDLE_CONTROL, waypoint.position, Quaternion.identity, SPHERE_RAD,
                        EventType.Repaint);
                    Handles.DrawLine(previousPosition, waypoint.position, LINE_THICK);
                    break;
            }

            previousPosition = waypoint.position;
        }

        if (!comeAndGo)
        {
            Handles.DrawLine(previousPosition, waypoints[INITIAL_ARRAY].position, LINE_THICK);
        }
    }
#endif

    #endregion
}