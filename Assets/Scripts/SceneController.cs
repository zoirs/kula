using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneController : MonoBehaviour {
    public GameObject prefab;
    private GameObject cube;
    private bool _one;

    private GravityDirection _gravityDirection = GravityDirection.DOWN;

    void Start() {
        
        
        //мавмав
//        Vector3[] level = new Vector3[] {
//            new Vector3(0, 0, 0),
//            new Vector3(10, 0, 0),
//            new Vector3(20, 0, 0),
//            new Vector3(40, 0, 0),
//            new Vector3(50, 0, 0),
//        };
//
//        foreach (Vector3 coords in level) {
//            GameObject cube = Instantiate(prefab, coords, Quaternion.identity);
//            NavMeshSurface[] surfaces = cube.GetComponentsInChildren<NavMeshSurface>();
//            foreach (NavMeshSurface surface in surfaces) {
//                surface.BuildNavMesh();
//            }
//        }
    }

    public GravityDirection GravityDirection {
        get { return _gravityDirection; }
    }
    
    //todo наверное, нужно в качестве парамтера передавать параметр "по часовй стрелки" или "против часовой стрелки" меняем направление движения
    public void ChangeGravityRigth(Direction moveDirection) {
        _gravityDirection = _gravityDirection.Rigth(moveDirection);
    }
}
