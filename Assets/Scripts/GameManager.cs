using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public Transform CubePlacesParent;
    [Header("Place Colors")]
    public Color ActiveColor;
    [SerializeField]
    private List<GameObject> _cubePlaces = new List<GameObject>();

    //Useage bools
    private bool chosedSocket = false;
    private int usedSockets = 0;

    private XRSocketInteractor currentActiveSocket;

    public enum GAMESTATE
    {
        CHOOSING_SOCKET,
        WAITING_PLAYER,
        END_ROUND
    };
    private GAMESTATE _gameState = GAMESTATE.CHOOSING_SOCKET;
    private void Awake()
    {
        foreach( Transform child in CubePlacesParent)
        {
            _cubePlaces.Add(child.gameObject);
        }
        disableSockets();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (_gameState)
        {
            case GAMESTATE.CHOOSING_SOCKET:
                if (!chosedSocket) {
                    Debug.Log("STATE: Chosing Socket");
                    ChooseSocket();
                    chosedSocket = true;
                }
                break;
            case GAMESTATE.WAITING_PLAYER:
                //Debug.Log("STATE: Waiting Player to put the cube");
                WaitingPlayer();
                break;
            case GAMESTATE.END_ROUND:
                EndRound();
                break;
            default:
                break;
        }
    }


    private void ChooseSocket()
    {
        //Select one place from the list
        int selectedIndex = Random.Range(0, _cubePlaces.Count);
        //Check for already used sockets and not repete them.
        //Activate the socketInteractor
        currentActiveSocket = _cubePlaces[selectedIndex].GetComponent<XRSocketInteractor>();
        currentActiveSocket.socketActive = true;
        //Change the color of the mesh
        MeshRenderer socketMesh = _cubePlaces[selectedIndex].GetComponent<MeshRenderer>();
        socketMesh.material.color = ActiveColor;

        ChangeGameState(GAMESTATE.WAITING_PLAYER);
    }
    private void WaitingPlayer()
    {
        //When the user puts the cube in place we jump of state to the next one
        if (currentActiveSocket.hasSelection)
        {
            Debug.Log("SELECTION ACHIVED");
            ChangeGameState(GAMESTATE.END_ROUND);
        }
    }
    private void EndRound()
    {
        //Give Some feedback to the player
        //EG: PLay sound and vibration TODO
        //Activate for choose another socket
        chosedSocket = false;
        //Update the number of cubes used
        usedSockets += 1;
        //JUmp to the first state
        ChangeGameState(GAMESTATE.CHOOSING_SOCKET);
    }

    public void ChangeGameState(GAMESTATE newState)
    {
        _gameState = newState;
    }

    private void disableSockets()
    {
        foreach(GameObject cube in _cubePlaces)
        {
            XRSocketInteractor socket = cube.GetComponent<XRSocketInteractor>();
            socket.socketActive = false;
        }
    }

    public void TestMesage()
    {
        Debug.Log("Interactable SELECT ENTER");
    }
}
