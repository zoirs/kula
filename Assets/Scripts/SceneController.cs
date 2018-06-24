using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneController : MonoBehaviour {
    public GameObject cubePrefab;
    public GameObject finishPrefab;
    public GameObject playerPrefab;

    private Direction _gravityDirection = Direction.DOWN;
    private FlyCamera _camera;

    private int currentLevel;
    private Dictionary<int, Level> levels = new Dictionary<int, Level>();

    void Start() {
        addTestLevels();

        _camera = Camera.main.GetComponent<FlyCamera>();
        LoadNextLevel();
    }

    public void ResetLevel() {
        ClearLevel();
        LoadLevel();        
    }
    
    public void LoadNextLevel() {
        ClearLevel();
        currentLevel++;
        LoadLevel();
    }

    private void LoadLevel() {
        Level level = levels[currentLevel];

        GameObject agent = CreateLevelElement(playerPrefab, level.Spawn, Quaternion.identity);
        _camera.SetAgent(agent);

        CreateLevelElement(finishPrefab, level.Finish, Quaternion.identity);

        foreach (Vector3 coords in level.Cubes) {
            GameObject cube = CreateLevelElement(cubePrefab, coords, Quaternion.identity);
            NavMeshSurface[] surfaces = cube.GetComponentsInChildren<NavMeshSurface>();
            foreach (NavMeshSurface surface in surfaces) {
                surface.BuildNavMesh();
            }
        }
    }

    private void ClearLevel() {
        int children = transform.childCount;
        for (int i = 0; i < children; ++i) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public Direction GravityDirection {
        get { return _gravityDirection; }
    }

    public void ChangeGravityClockwise() {
        _gravityDirection = _gravityDirection.GetNextClockwise();
        if (_camera != null) {
            _camera.RotateNextClockwise(_gravityDirection);
        }
    }

    public void ChangeGravityCounterClockwise() {
        _gravityDirection = _gravityDirection.GetCounterClockwise();
        if (_camera != null) {
            _camera.RotateCounterClockwise(_gravityDirection);
        }
    }

    private GameObject CreateLevelElement(GameObject original, Vector3 levelFinish, Quaternion quaternion) {
        GameObject go = Instantiate(original, levelFinish, quaternion);
        go.transform.SetParent(gameObject.transform);
        return go;
    }

    private void addTestLevels() {
        Vector3[] cubesPosition1 = {
            new Vector3(0, 0, 0),
            new Vector3(5, 0, 0),
            new Vector3(10, 0, 0),
            new Vector3(15, 0, 0),
            new Vector3(20, 0, 0),
        };
        Vector3[] cubesPosition2 = {
            new Vector3(0, 0, 0),
            new Vector3(-5, 0, 0),
            new Vector3(10, 5, 0),
            new Vector3(15, 0, 0),
            new Vector3(20, -5, 0),
        };

        levels.Add(1, new Level(new Vector3(0, 3, 0), new Vector3(20, 0, 0), cubesPosition1));
        levels.Add(2, new Level(new Vector3(0, 3, 0), new Vector3(20, 0, 0), cubesPosition2));
    }
}