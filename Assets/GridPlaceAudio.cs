using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GridPlaceAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private XRSocketInteractor _socket;
    // Start is called before the first frame update

    private bool notPlayed = true;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _socket = GetComponent<XRSocketInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
       if(_socket.hasSelection && notPlayed)
        {
            PlayAudioOnSelection();
            notPlayed = false;
        }
    }

    public void PlayAudioOnSelection()
    {
        _audioSource.Play();
    }
}
