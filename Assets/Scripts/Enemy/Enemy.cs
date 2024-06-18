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
    [SerializeField] private float minWaitTime = .001f;
    [SerializeField] private float turnSpeed = 60;

    [SerializeField] private float viewDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private Transform pathHolder;
    [SerializeField] private bool comeAndGo;
    [SerializeField] private float shootCooldown = 1.5f;
    private float deltaShootCooldown = 1.5f;

    [SerializeField] private bool incombat;
    [SerializeField] private Vector3 targetWaypoint;
    [SerializeField] private int targetWaypointIndex;
    private Transform[] waypoints;
    private bool _reverse;

    #endregion

    #region Constants

    private const int INITIAL_ARRAY = 0;
    private const int INITIAL_INDEX = 0;
    private const int ONE_ELEMENT = 1;
    private const float NEGLIGIBLE_ANGLE_DIFF = 0.05f;
    private const int ZERO_INIT_ADJUSTMENT = 1;
    private const float HALF_ANGLE = 0.5f;
    private const float RIGHT_ANGLE = 90f;

#if UNITY_EDITOR
    private const int FIRST_SPHERE = 0;
    private const int FIRST_LINE = 1;
    private const float FIRST_SPHERE_RAD = 0.5f;
    private const float SPHERE_RAD = 0.2f;
    private const float FIRST_LINE_THICK = 4f;
    private const float LINE_THICK = 1f;
    private const int HANDLE_CONTROL = 0;

    private readonly Color _firstHandleNodeColor = new Color(0, 255, 0);
    private readonly Color _handlenodesColor = new Color(255, 255, 0, 0.5f);
    private readonly Color _enemyfovHandleAreaColor = new Color(0, 255, 255, 0.5f);
    private readonly Color _enemycombatHandleAreaColor = new Color(255, 0, 0, 0.5f);
#endif

    #endregion

    #region UnityMethods

    private void Start()
    {
        deltaShootCooldown = Time.time;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waypoints = pathHolder.GetComponentsInChildren<Transform>()
            .Where(x => x != pathHolder.transform).ToArray();
        transform.position = GetWaypoint(waypoints, INITIAL_ARRAY);
        InitializeEnemyRoutine();
        StartCoroutine(EnemyRoutime(waypoints));
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

    private bool NearPlayer()
    {
        var enemyT = transform;
        return Vector3.Distance(enemyT.position, player.position) < attackDistance;
    }

    private void InitializeEnemyRoutine()
    {
        targetWaypoint = GetWaypoint(waypoints, INITIAL_ARRAY);
        targetWaypointIndex = INITIAL_ARRAY;
    }

    private IEnumerator EnemyRoutime(IReadOnlyList<Transform> waypoints)
    {
        while (true)
        {
            if (!incombat)
            {
                if (CanSeePlayer())
                {
                    incombat = true;
                }
                else
                {
                    var waypointsQty = waypoints.Count();
                    if (waypointsQty <= ONE_ELEMENT)
                    {
                        yield return StartCoroutine(Rotate());
                        yield return new WaitForSeconds(waitTime);
                    }
                    else //More wps
                    {
                        transform.position = MoveToWaypoint(targetWaypoint);
                        yield return new WaitForSeconds(minWaitTime);
                        if (transform.position == targetWaypoint)
                        {
                            targetWaypointIndex = GetNewWaypointIndex(waypoints, targetWaypointIndex);
                            targetWaypoint = GetWaypoint(waypoints, targetWaypointIndex);
                            yield return new WaitForSeconds(waitTime);
                            yield return StartCoroutine(FaceTarget(targetWaypoint));
                        }
                    }
                }
            }
            if (incombat)
            {
                if (!NearPlayer())
                {
                    incombat = false;
                }
                else
                {
                    yield return StartCoroutine(FaceTargetAttack(player.position));
                    yield return new WaitForSeconds(minWaitTime);
                    Debug.Log($"Oscar {shootCooldown} and {Time.time-deltaShootCooldown}");
                    if (shootCooldown <= Time.time-deltaShootCooldown)
                    {
                        var shoot = PooledShots.SharedInstance.GetPooledObject();
                        if (shoot == null) continue;
                        var transform1 = transform;
                        shoot.transform.position = transform1.position+transform1.forward;
                        shoot.transform.rotation = transform1.rotation;
                        shoot.SetActive(true);
                        deltaShootCooldown = Time.time;
                    }
                }
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

    private IEnumerator FaceTargetAttack(Vector3 targetPos)
    {
        var directionToLook = (targetPos - transform.position).normalized;
        var targetAngle = RIGHT_ANGLE - Mathf.Atan2(directionToLook.z, directionToLook.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > NEGLIGIBLE_ANGLE_DIFF)
        {
            if (!NearPlayer()) yield break;
            var angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }
    
    private IEnumerator FaceTarget(Vector3 targetPos)
    {
        var directionToLook = (targetPos - transform.position).normalized;
        var targetAngle = RIGHT_ANGLE - Mathf.Atan2(directionToLook.z, directionToLook.x) * Mathf.Rad2Deg;
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > NEGLIGIBLE_ANGLE_DIFF)
        {
            if (CanSeePlayer()) yield break;
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
            if (incombat) yield break;
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
        if (incombat)
        {
            Handles.color = _enemycombatHandleAreaColor;
            Handles.DrawWireDisc(transform.position, enemyTransform.up, attackDistance, FIRST_LINE_THICK);
            Handles.DrawLine(transform.position, transform.position + transform.forward * attackDistance, LINE_THICK);
        }
        else
        {
            Handles.color = _enemyfovHandleAreaColor;
            Handles.DrawSolidArc(transform.position, enemyTransform.up, from, viewAngle, viewDistance);

            var previousPosition = waypoints[INITIAL_ARRAY].position;
            for (var index = INITIAL_INDEX; index < waypoints.Length; index++)
            {
                var waypoint = waypoints[index];
                switch (index)
                {
                    case FIRST_SPHERE:
                        Handles.color = _firstHandleNodeColor
                            ;
                        Handles.SphereHandleCap(HANDLE_CONTROL, waypoints[INITIAL_ARRAY].position, Quaternion.identity,
                            FIRST_SPHERE_RAD,
                            EventType.Repaint);
                        break;
                    case FIRST_LINE:
                        Handles.DrawLine(previousPosition, waypoint.position, FIRST_LINE_THICK);
                        Handles.color = _handlenodesColor;
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
    }
#endif

    #endregion
}