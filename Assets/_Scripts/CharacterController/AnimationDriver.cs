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
        // Normal movement animations
        animator.SetBool("QuickTurn", movement.IsQuickTurning);
        animator.SetBool("MovingForward", movement.IsMovingForward);
        animator.SetBool("MovingBackward", movement.IsMovingBackward);
        animator.SetBool("TurningLeft", movement.IsTurningLeft);
        animator.SetBool("TurningRight", movement.IsTurningRight);
        animator.SetBool("IsSprinting", movement.IsSprinting);
        
        if (!movement.CanMove)
        {
            // Block all other anims during quick turn
            animator.SetBool("QuickTurn", false);
            animator.SetBool("MovingForward", false);
            animator.SetBool("MovingBackward", false);
            animator.SetBool("TurningLeft", false);
            animator.SetBool("TurningRight", false);
            animator.SetBool("IsSprinting", false);
            
        }
    }
}