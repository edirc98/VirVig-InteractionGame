using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public Transform CubePlacesParent;
    [Header("Place Colors")]
    public Color ActiveColor;
    public Color CorrectColor;

    [SerializeField]
    private List<GameObject> _cubePlaces = new List<GameObject>();

    //Useage bools
    private bool _chosedSocket = false;
    private bool _gameFinished = false;
    
    private int _usedSockets = 0;
    private int _selectedIndex;

    private XRSocketInteractor currentActiveSocket;
    private MeshRenderer currentSocketMesh;
    [SerializeField]
    private CubeSpawner _cubeSpawner;

    public enum GAMESTATE
    {
        CHOOSING_SOCKET,
        WAITING_PLAYER,
        END_ROUND
    };
    private GAMESTATE _gameState = GAMESTATE.CHOOSING_SOCKET;

    public enum SPAWNCUBETYPE
    {
        AUTO,
        MANUAL
    };
    public SPAWNCUBETYPE _spawnCubeType = SPAWNCUBETYPE.AUTO;

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
        _cubeSpawner = GetComponent<CubeSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameFinished)
        {
            switch (_gameState)
            {
                case GAMESTATE.CHOOSING_SOCKET:
                    if (!_chosedSocket)
                    {
                        //Debug.Log("STATE: Chosing Socket");
                        ChooseSocket();
                        _chosedSocket = true;
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
        
        if(_cubePlaces.Count == 0)
        {
            //List Is empty, all cube places used
            //Debug.Log("GAME FINISHED");
            _gameFinished = true;
        }
    }

    #region STATE FUNCTIONS
    private void ChooseSocket()
    {
        //Select one place from the list
        _selectedIndex = Random.Range(0, _cubePlaces.Count);
        //Check for already used sockets and not repete them.
        //Activate the socketInteractor
        currentActiveSocket = _cubePlaces[_selectedIndex].GetComponent<XRSocketInteractor>();
        currentActiveSocket.socketActive = true;
        //Change the color of the mesh
        ChangeCubePlaceColor(_selectedIndex, ActiveColor);
        //For automatic spawn cubes when needed
        if(_spawnCubeType == SPAWNCUBETYPE.AUTO)_cubeSpawner.SpawnCube();
        ChangeGameState(GAMESTATE.WAITING_PLAYER);
    }
    private void WaitingPlayer()
    {
        //When the user puts the cube in place we jump of state to the next one
        if (currentActiveSocket.hasSelection)
        {
            //Debug.Log("SELECTION ACHIVED");
            ChangeGameState(GAMESTATE.END_ROUND);
        }
    }
    private void EndRound()
    {
        //Give Some feedback to the player
        //EG: PLay sound and vibration TODO
        //Activate for choose another socket
        _chosedSocket = false;
        //Update the number of cubes used
        _usedSockets += 1;
        ChangeCubePlaceColor(_selectedIndex, CorrectColor);
        _cubePlaces.Remove(currentActiveSocket.gameObject);
        //JUmp to the first state
        ChangeGameState(GAMESTATE.CHOOSING_SOCKET);
    }
    #endregion
    #region OTHER FUNCTIONS
    private void ChangeCubePlaceColor(int selectedIndex, Color newColor)
    {
        currentSocketMesh = _cubePlaces[selectedIndex].GetComponent<MeshRenderer>();
        currentSocketMesh.material.color = newColor;
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
    #endregion
}
