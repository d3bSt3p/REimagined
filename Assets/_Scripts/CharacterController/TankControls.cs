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
    public float quickTurnTime = 0.25f; // how long the 180° turn should take
    private bool isQuickTurning = false;

    public float VerticalInput { get; private set; }
    public float HorizontalInput { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool DidQuickTurn { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Read movement inputs
        VerticalInput = Input.GetAxis("Vertical");
        HorizontalInput = Input.GetAxis("Horizontal");

        // Detect the Quick Turn combo (S held + K pressed this frame)
        if (!isQuickTurning && Input.GetKey(KeyCode.Q))
        {
            StartCoroutine(QuickTurnCoroutine());
        }

        // Rotation
        if (!isQuickTurning)
        {
            transform.Rotate(0, HorizontalInput * turnSpeed * Time.deltaTime, 0);
        }

        // Sprint only if moving forward
        bool shiftHeld = Input.GetKey(KeyCode.LeftShift);
        IsSprinting = shiftHeld && VerticalInput > 0.1f;

        // Movement speed logic
        float currentSpeed;

        if (VerticalInput > 0.1f)
        {
            currentSpeed = IsSprinting ? speed * sprintMultiplier : speed;
        }
        else if (VerticalInput < -0.1f)
        {
            currentSpeed = speed * backwardMultiplier;
        }
        else
        {
            currentSpeed = 0f;
        }

        // Apply movement
        Vector3 moveDir = transform.forward * VerticalInput * currentSpeed;
        controller.Move(moveDir * Time.deltaTime - Vector3.up * 0.1f);
    }

    private IEnumerator QuickTurnCoroutine()
    {
        isQuickTurning = true;

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

        // Make sure it ends cleanly at exactly 180°
        transform.rotation = endRot;

        isQuickTurning = false;
    }
}
