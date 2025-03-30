using UnityEngine;

public class Vfx : StateMachineBehaviour
{
    public GameObject spellVfx;
    public float triggerTime = 0.2f; // Время срабатывания (0 - начало, 1 - конец)
    public float offset = 0.3f;
    private bool triggered = false;
    private VFXManager vfxManager;
    public float lifetime = 3f;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vfxManager = animator.GetComponent<VFXManager>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= triggerTime && !triggered)
        {
            triggered = true;
            
            Vector3 midpoint = (vfxManager.leftHand.position + vfxManager.rightHand.position) / 2;

            // Создаём объект в середине
            GameObject spell = Instantiate(spellVfx, new Vector3(midpoint.x, midpoint.y, midpoint.z + offset), Quaternion.identity);

            Destroy(spell, lifetime);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        triggered = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
