using UnityEngine;
using System.Collections;

public class TankControls : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 10f;
    public float turnSpeed = 100f;
    public float sprintMultiplier = 1.5f;
    public float backwardMultiplier = 0.6f;

    [Header("Quick Turn Settings")]
    public float quickTurnTime = 0.25f;
    public float quickTurnCooldown = 0.5f;   
    public bool IsQuickTurning { get; private set; }
    public bool IsQuickTurnOnCooldown => quickTurnCooldownTimer > 0f;

    private float quickTurnCooldownTimer = 0f;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool IsSprinting { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Reduce cooldown timer
        if (quickTurnCooldownTimer > 0f)
            quickTurnCooldownTimer -= Time.deltaTime;

        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");

        // Quick Turn input — only if allowed
        bool quickTurnPressed = Input.GetKey(KeyCode.Q);

        if (!IsQuickTurning && quickTurnPressed && quickTurnCooldownTimer <= 0f)
        {
            StartCoroutine(QuickTurnCoroutine());
        }

        // Normal turning blocked during quick turn
        if (!IsQuickTurning)
        {
            transform.Rotate(0, HorizontalInput * turnSpeed * Time.deltaTime, 0);
        }

        // Sprint logic
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        IsSprinting = shiftHeld && VerticalInput > 0.1f;

        // Movement speed logic
        float currentSpeed = 0;

        if (!IsQuickTurning)
        {
            if (VerticalInput > 0.1f)
                currentSpeed = IsSprinting ? speed * sprintMultiplier : speed;
            else if (VerticalInput < -0.1f)
                currentSpeed = speed * backwardMultiplier;
        }

        Vector3 moveDir = transform.forward * VerticalInput * currentSpeed;
        controller.Move(moveDir * Time.deltaTime - Vector3.up * 0.1f);
    }

    private IEnumerator QuickTurnCoroutine()
    {
        IsQuickTurning = true;

        Quaternion startRot = transform.rotation;
        Quaternion endRot = transform.rotation * Quaternion.Euler(0f, 180f, 0f);

        float elapsed = 0f;

        while (elapsed < quickTurnTime)
        {
            float t = elapsed / quickTurnTime;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;

        // Start cooldown
        quickTurnCooldownTimer = quickTurnCooldown;

        IsQuickTurning = false;
    }
}
