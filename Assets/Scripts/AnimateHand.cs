using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class AnimateHand : MonoBehaviour
{
    public Animator HandAnimator;
    public InputActionProperty triggerAnimationAction;
    public InputActionProperty gripAnimationAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = triggerAnimationAction.action.ReadValue<float>();
        HandAnimator.SetFloat("Trigger", triggerValue);
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        HandAnimator.SetFloat("Grip", gripValue);
    }
}
