using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {
    private SceneController _sceneController;
    float gravity = 1.0f;
    float jumpHeight = 15.0f;
    private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private bool _jumpPressed = false;
    private float _rebroSize;


    void Start() {
        _sceneController = GameObject.Find("Scene").GetComponent<SceneController>();
            
        if (_sceneController == null) {
            Debug.LogWarning("Scene is null");
        }

        _characterController = gameObject.GetComponent<CharacterController>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        SwichMoveType(true);
    }

    void Update() {
        bool rightDown = Input.GetKeyDown(KeyCode.RightArrow);
        bool leftDown = Input.GetKeyDown(KeyCode.LeftArrow);
        // двигаемся по плоскости
        if (_navMeshAgent.enabled && (rightDown || leftDown)) {
            GameObject surface = ((Component) _navMeshAgent.navMeshOwner).gameObject;
            CubeController cube = surface.GetComponentInParent<CubeController>();
            Vector3 nextDestination = FindNextDestination(cube.Position, rightDown ? Direction.RIGHT : Direction.LEFT);
            _navMeshAgent.SetDestination(nextDestination);
        }

        // Стартуем прыжок
        if (_navMeshAgent.enabled && Input.GetKey(KeyCode.Space)) {
            SwichMoveType(false);
        }


        // прыгаем
        if (_characterController.enabled) {

            CollisionFlags collisionFlags = _characterController.Move(_navMeshAgent.speed * _sceneController.GravityDirection.GetOppositeVector() * Time.deltaTime);

            if (CheckCollision(collisionFlags)) {
                _sceneController.ChangeGravityClockwise();                
                _sceneController.ChangeGravityClockwise();                
                SwichMoveType(true);
            }
        }
    }
    
    private bool CheckCollision(CollisionFlags collisionFlags) {
        return collisionFlags == CollisionFlags.CollidedBelow && _sceneController.GravityDirection == Direction.DOWN || 
               collisionFlags == CollisionFlags.CollidedAbove && _sceneController.GravityDirection == Direction.UP || 
               collisionFlags == CollisionFlags.CollidedSides && (_sceneController.GravityDirection == Direction.LEFT ||_sceneController.GravityDirection== Direction.RIGHT );
    }
    
    // ищем на какой куб можно перейти
    private Vector3 FindNextDestination(Vector3 currentCubePosition, Direction moveDir) {
        _rebroSize = 2.5f;

        //Позиция мячика на кубе
        Vector3 ballPos = _rebroSize * _sceneController.GravityDirection.GetOppositeVector();
        //Вектор из центра куба в сторону направления движения
        Vector3 ballPosDirection = _rebroSize * _sceneController.GravityDirection.GetDirection(moveDir);
        //Координаты края куба, по направлению движения
        Vector3 edgePos = ballPos + ballPosDirection;

        Vector3 up = edgePos + _rebroSize * _sceneController.GravityDirection.GetUpVector();
        Vector3 forward = edgePos + _rebroSize * _sceneController.GravityDirection.GetDirection(moveDir);
        Vector3 down = edgePos + _rebroSize * _sceneController.GravityDirection.GetDownVector();

        if (_navMeshAgent.CalculatePath(currentCubePosition + up, new NavMeshPath())) {
            Debug.Log("up1 " + up);
            
            if (moveDir == Direction.LEFT) {
                _sceneController.ChangeGravityClockwise();                
            }
            if (moveDir == Direction.RIGHT) {
                _sceneController.ChangeGravityCounterClockwise();                
            }
            
            return currentCubePosition + up;
        }

        if (_navMeshAgent.CalculatePath(currentCubePosition + forward, new NavMeshPath())) {
            Debug.Log("fr1 " + forward);
            return currentCubePosition + forward;
        }

        if (_navMeshAgent.CalculatePath(currentCubePosition + down, new NavMeshPath())) {
            Debug.Log("dn1 " + down );
            
            if (moveDir == Direction.LEFT) {
                _sceneController.ChangeGravityCounterClockwise();                
            }
            if (moveDir == Direction.RIGHT) {
                _sceneController.ChangeGravityClockwise();                
            }
            return currentCubePosition + down;
        }

        return currentCubePosition + ballPos;
//      Vector3 up = Quaternion.Euler(0, 0, _sceneController.GravityDirection.MRigth(Direction.UP)) * size;
    }

    private void SwichMoveType(bool isAgent) {
        if (_characterController != null) {
            _characterController.enabled = !isAgent;
        }

        if (_navMeshAgent != null) {
            _navMeshAgent.enabled = isAgent;
        }

        if (!isAgent) {
            StartCoroutine(CheckFly());
        }
    }

    // После старта полета, проверяем приземлился ли или нет
    private IEnumerator CheckFly() {
        yield return new WaitForSeconds(3);

        // если через 3 сек не приземлился, то умираем
        if (!_navMeshAgent.enabled) {
            _sceneController.ResetLevel();            
        }
    }

    void OnTriggerEnter(Collider a)
    {
        if (a.name.Contains("Finish")) {
            _sceneController.LoadNextLevel();
        }


    }
}