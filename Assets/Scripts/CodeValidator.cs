using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
//using UnityEngine.UI;

public class CodeValidator : MonoBehaviour
{
    [Header("Codes")]
    [SerializeField]
    private string _placeCode;
    [SerializeField]
    private string _readedCode;

    [SerializeField]
    private XRSocketInteractor _socket;

    [SerializeField]
    private AudioSource _audioSource;

    private TMPro.TextMeshPro _placeCodeText;
    private TMPro.TextMeshPro _cubeCodeText;

    // Start is called before the first frame update
    void Start()
    {
        _placeCodeText = GetComponentInChildren<TMPro.TextMeshPro>();
        if (_placeCodeText != null) _placeCode = _placeCodeText.text;
        else Debug.Log("Code Text Not Found");

        _socket = GetComponent<XRSocketInteractor>();
        _socket.allowSelect = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (_socket.hasHover)
        {
            IXRHoverInteractable firstHover = _socket.GetOldestInteractableHovered();
            if(firstHover != null)
            {
                GameObject HoveredCubeGO = firstHover.transform.gameObject;
                _cubeCodeText = HoveredCubeGO.GetComponentInChildren<TMPro.TextMeshPro>();
                _readedCode = _cubeCodeText.text;
            }

            if (_placeCode == _readedCode)
            {
                _socket.allowSelect = true;
            }
            else
            {
                _socket.allowSelect = false;
            }
        }
        
    }

}
