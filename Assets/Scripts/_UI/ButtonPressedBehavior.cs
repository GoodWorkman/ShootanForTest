using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehavior : StateMachineBehaviour
{
    public static Dictionary<string, System.Action> buttonFunctionTable;

    private void Awake() 
    {
        buttonFunctionTable = new Dictionary<string, System.Action>();
    }
   
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllUIInputs();  
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        buttonFunctionTable[animator.gameObject.name].Invoke();
    }
}
