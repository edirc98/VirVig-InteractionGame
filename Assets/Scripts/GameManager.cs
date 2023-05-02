using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    [Header("Place Colors")]
    public Color ActiveColor;
    public Color CorrectColor;

    public Levels GameManagerLevels;
    public Timer GameTimer;
    public int currentLevel = 0;

    public GameObject ContinueModal;
    [SerializeField]
    private GameObject currentLevelPrefab = null;

    [SerializeField]
    private List<GameObject> _cubePlaces = new List<GameObject>();

    //Useage bools
    private bool _startGame = false;
    private bool _chosedSocket = false;
    private bool _levelFinished = false;
    private bool _gameFinished = false;
    private bool _instanceNextLevel = true;
    
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
        MANUAL,
        NONE
    };
    public SPAWNCUBETYPE _spawnCubeType = SPAWNCUBETYPE.AUTO;

    // Start is called before the first frame update
    void Start()
    {
        _cubeSpawner = GetComponent<CubeSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_startGame)
        {
            if (!_gameFinished)
            {
                if (currentLevelPrefab == null && _instanceNextLevel)
                {
                    //Spawn the corresponding level acording to currentLevel
                    currentLevelPrefab = Instantiate(GameManagerLevels.GameLevels[currentLevel].LevelPrefab);
                    GetCubePlacesObjects(currentLevelPrefab.transform);
                    //Check type of Level to set the type of spawn needed
                    if (GameManagerLevels.GameLevels[currentLevel].levelType == Level.LevelType.COLORS)
                    {
                        _spawnCubeType = SPAWNCUBETYPE.AUTO;
                    }
                    else _spawnCubeType = SPAWNCUBETYPE.NONE;
                    //_levelFinished = false;
                    _instanceNextLevel = false;
                    GameTimer.StartTimer();
                }
                if (!_levelFinished)
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
                if (_cubePlaces.Count == 0)
                {
                    //List Is empty, all cube places used
                    _levelFinished = true;
                    Destroy(currentLevelPrefab,0.5f);
                    _cubeSpawner.DestroySpawnedCubes();
                    currentLevelPrefab = null;
                    //Debug.Log("Level " + currentLevel + "finished");
                    ContinueModal.SetActive(true);
                    GameTimer.StopTimer();
                    GameTimer.ResetTimer();
                }
            }

            if (currentLevel == GameManagerLevels.GameLevels.Count)
            {
                _gameFinished = true;
                Debug.Log("Game Finished");
            }
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

    private void GetCubePlacesObjects(Transform parentObjectTransform)
    {
        foreach (Transform child in parentObjectTransform)
        {
            if (child.CompareTag("CubePlace"))
            {
                _cubePlaces.Add(child.gameObject);
            }
        }
    }

    public void StartGame()
    {
        _startGame = true;
    }
    public void ContinueGame()
    {
        _levelFinished = false;
        _instanceNextLevel = true;
        currentLevel++;
        ContinueModal.SetActive(false);
    }

    public void TestMesage()
    {
        Debug.Log("Interactable SELECT ENTER");
    }
    #endregion
}
