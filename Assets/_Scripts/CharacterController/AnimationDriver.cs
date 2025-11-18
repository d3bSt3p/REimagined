using UnityEngine;

public class AnimationDriver : MonoBehaviour
{
    public Animator animator;
    private MovementState movement;

    void Start()
    {
        movement = GetComponent<MovementState>();
    }

    void Update()
    {
        animator.SetBool("MovingForward", movement.IsMovingForward);
        animator.SetBool("MovingBackward", movement.IsMovingBackward);
        animator.SetBool("TurningLeft", movement.IsTurningLeft);
        animator.SetBool("TurningRight", movement.IsTurningRight);
        animator.SetBool("IsSprinting", movement.IsSprinting);
        
    }
}