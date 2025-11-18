using UnityEngine;

public class MovementState : MonoBehaviour
{
    private TankControls tank;

    public bool IsMovingForward { get; private set; }
    public bool IsMovingBackward { get; private set; }
    public bool IsTurningLeft { get; private set; }
    public bool IsTurningRight { get; private set; }
    public bool IsSprinting { get; private set; }

    void Start()
    {
        tank = GetComponent<TankControls>();
    }

    void Update()
    {
        float v = tank.VerticalInput;
        float h = tank.HorizontalInput;

        IsMovingForward = v > 0.1f;
        IsMovingBackward = v < -0.1f;
        IsTurningLeft = h < -0.1f;
        IsTurningRight = h > 0.1f;

        // Sprint only when moving forward and holding shift
        IsSprinting = tank.IsSprinting && IsMovingForward;
    }
}