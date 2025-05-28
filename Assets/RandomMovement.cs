using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 2f;          // �������� �������� ������
    public float rotationSpeed = 30f;     // �������� �������� (������� � �������)
    public float minIdleTime = 1f;        // ����������� ����� �������
    public float maxIdleTime = 3f;        // ������������ ����� �������
    public float minMoveTime = 1f;        // ����������� ����� ��������
    public float maxMoveTime = 3f;        // ������������ ����� ��������

    private Animator animator;
    private float timer;
    private MovementState currentState;
    private float currentStateDuration;
    private float targetRotation;

    private enum MovementState
    {
        Idle,
        MovingForward,
        TurningLeft,
        TurningRight
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found!");
            return;
        }

        SetNewState(MovementState.Idle);
    }

    void Update()
    {
        if (animator == null) return;

        timer += Time.deltaTime;

        if (timer >= currentStateDuration)
        {
            // �������� ����� ��������� ���������
            MovementState newState = (MovementState)Random.Range(0, System.Enum.GetValues(typeof(MovementState)).Length);
            SetNewState(newState);
        }

        // ��������� �������� ���������
        switch (currentState)
        {
            case MovementState.Idle:
                // ������ �� ������, ������ �����
                break;

            case MovementState.MovingForward:
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                break;

            case MovementState.TurningLeft:
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                break;

            case MovementState.TurningRight:
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                break;
        }

        // ���������� ��������� ����� �������� Speed
        float animationSpeed = (currentState == MovementState.Idle) ? 0f : 1f;
        animator.SetFloat("Speed", animationSpeed);
    }

    private void SetNewState(MovementState newState)
    {
        currentState = newState;
        timer = 0f;

        switch (newState)
        {
            case MovementState.Idle:
                currentStateDuration = Random.Range(minIdleTime, maxIdleTime);
                break;

            case MovementState.MovingForward:
            case MovementState.TurningLeft:
            case MovementState.TurningRight:
                currentStateDuration = Random.Range(minMoveTime, maxMoveTime);
                break;
        }
    }
}