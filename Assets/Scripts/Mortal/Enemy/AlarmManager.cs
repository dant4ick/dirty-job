using System.Collections;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    public enum AlarmLevel
    {
        Calm,
        Concerned,
        Aware,
        Alarmed
    }

    private EnemyAI _enemyAI;
    private EnemyAttackManager _enemyAttackManager;

    [SerializeField] private GameObject _suspectCuePreFab;
    [SerializeField] private GameObject _alarmedCuePreFab;
    private GameObject _suspectCue;
    private GameObject _alarmedCue;

    public AlarmLevel alarmLevel = AlarmLevel.Calm;
    private float reactionTime = 0.5f;
    public Vector2 soundPosition;
    public Transform target;

    private void Start()
    {
        _enemyAI = GetComponent<EnemyAI>();
        _enemyAttackManager = GetComponent<EnemyAttackManager>();
    }

    public void GoToSound(Transform target)
    {
        this.target = target;
    }

    public void HearQuietSound(Vector2 soundPosition)
    {
        if (alarmLevel == AlarmLevel.Concerned || alarmLevel == AlarmLevel.Aware || alarmLevel == AlarmLevel.Alarmed)
            return;

        StartCoroutine(AlarmLevelChanged(AlarmLevel.Concerned));

        this.soundPosition = soundPosition;
        alarmLevel = AlarmLevel.Concerned;
    }

    public void HearLoudSound(Vector2 soundPosition)
    {
        if (alarmLevel == AlarmLevel.Alarmed)
            return;

        if (alarmLevel != AlarmLevel.Aware)
            StartCoroutine(AlarmLevelChanged(AlarmLevel.Aware));

        this.soundPosition = soundPosition;
        alarmLevel = AlarmLevel.Aware;
    }

    public void PlayerHasBeenSpoted(Transform player)
    {
        if (alarmLevel == AlarmLevel.Alarmed)
            return;

        if (alarmLevel != AlarmLevel.Aware)
            StartCoroutine(AlarmLevelChanged(AlarmLevel.Alarmed));

        target = player;
        alarmLevel = AlarmLevel.Alarmed;
    }

    private IEnumerator AlarmLevelChanged(AlarmLevel nextAlarmLevel)
    {
        _enemyAI.followEnabled = false;

        float heightOfObject = transform.GetComponent<CapsuleCollider2D>().bounds.size.y;
        float widthOfObject = transform.GetComponent<CapsuleCollider2D>().bounds.center.x;

        if (nextAlarmLevel == AlarmLevel.Concerned)
        {
            _suspectCue = Instantiate(_suspectCuePreFab, transform, false);
            _suspectCue.transform.position = new Vector2(transform.position.x, transform.position.y + heightOfObject + 0.0625f);
        }
        else if (nextAlarmLevel == AlarmLevel.Aware || nextAlarmLevel == AlarmLevel.Alarmed)
        {
            _enemyAttackManager._canAttack = false;

            _alarmedCue = Instantiate(_alarmedCuePreFab, transform, false);
            _alarmedCue.transform.position = new Vector2(transform.position.x, transform.position.y + heightOfObject + 0.0625f);
        }

        yield return new WaitForSeconds(reactionTime);

        _enemyAI.followEnabled = true;
        if (nextAlarmLevel == AlarmLevel.Concerned)
        {
            Destroy(_suspectCue);
        }
        else if (nextAlarmLevel == AlarmLevel.Aware || nextAlarmLevel == AlarmLevel.Alarmed)
        {
            //_enemyAttackManager.GetHand().SetActive(true);
            _enemyAttackManager._canAttack = true;

            Destroy(_alarmedCue);
        }   
    }

    static public void AlarmEnemiesByQuietSound(Transform point, Vector2 size)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(point.position, size, 0, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<AlarmManager>().HearQuietSound(point.position);
        }
    }

    static public void AlarmEnemiesByQuietSound(Transform point, int radius)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point.position, radius, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<AlarmManager>().HearQuietSound(point.position);
        }
    }

    static public void AlarmEnemiesByLoudSound(Transform point, int radius)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(point.position, radius, LayerMask.GetMask("Enemy"));

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<AlarmManager>().HearLoudSound(point.position);
        }
    }
}
