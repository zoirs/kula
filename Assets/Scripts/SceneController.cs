using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneController : MonoBehaviour {
    public GameObject prefab;
    private GameObject cube;
    private bool _one;

    private Direction _gravityDirection = Direction.DOWN;
    private FlyCamera _camera;

    void Start() {
        
        
        //мавмав
//        Vector3[] level = new Vector3[] {
//            new Vector3(0, 0, 0),
//            new Vector3(5, 0, 0),
//            new Vector3(10, 0, 0),
//            new Vector3(15, 0, 0),
//            new Vector3(20, 0, 0),
//        };
//
//        foreach (Vector3 coords in level) {
//            GameObject cube = Instantiate(prefab, coords, Quaternion.identity);
//            NavMeshSurface[] surfaces = cube.GetComponentsInChildren<NavMeshSurface>();
//            foreach (NavMeshSurface surface in surfaces) {
//                surface.BuildNavMesh();
//            }
//        }
        _camera = Camera.main.GetComponent<FlyCamera>();

        
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
}
