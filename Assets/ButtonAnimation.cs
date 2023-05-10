using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator ButtonAnimator;
    [SerializeField]
    private AudioSource ButtonSound;

    public GameManager gameManager;
    public bool StartButton = false;
    public bool ContinueButton = false;
    // Start is called before the first frame update
    void Start()
    {
        ButtonAnimator = GetComponent<Animator>();
        ButtonSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered Button Trigger");
        if(other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
            ButtonAnimator.SetTrigger("PressButton");
            ButtonSound.Play();
            if (StartButton)
            {
                StartCoroutine(gameManager.StartGameAfterSecs());
            }
            else if(ContinueButton)
            {
                StartCoroutine(gameManager.ContinueGameAfterSecs());
            }
            //Debug.Log("Its a Hand, do something");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("RightHand") || other.CompareTag("LeftHand"))
        {
            ButtonAnimator.SetTrigger("ReleaseButton");
        }
    }
}
