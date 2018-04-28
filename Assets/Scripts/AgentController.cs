using System;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class AgentController : MonoBehaviour {
    private SceneController _sceneController;
    Vector3 moveDirection = Vector3.zero;
    float runSpeed;
    float gravity = 1.0f;
    float jumpHeight = 15.0f;
    private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private const int STEP = 5;
    private bool _jumpPressed = false;
    private float elapsed = 0.0f;
    private float _rebroSize;


    void Start() {
        _sceneController = GameObject.Find("Scene").GetComponent<SceneController>();

        if (_sceneController == null) {
            Debug.LogWarning("Scene is null");
        }

        _characterController = gameObject.GetComponent<CharacterController>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        runSpeed = _navMeshAgent.speed;
        SwichMoveType(true);
    }

    void Update() {
        
        // двигаемся по плоскости
        if (_navMeshAgent.enabled && Input.GetKeyDown(KeyCode.RightArrow)) {
            GameObject surface = ((Component) _navMeshAgent.navMeshOwner).gameObject;
            CubeController cube = surface.GetComponentInParent<CubeController>();
            Vector3 nextDestination = FindNextDestination(cube.Position);
    
            _navMeshAgent.SetDestination(nextDestination);
        }

        // Стартуем прыжок
        if (_navMeshAgent.enabled && Input.GetKey(KeyCode.Space)) {
            SwichMoveType(false);
            _jumpPressed = true;
        }


        // прыгаем
        if (_characterController.enabled) {
            if (_jumpPressed) {
                _jumpPressed = false;
                //Выставить высоту прыжка
                switch (_sceneController.GravityDirection) {
                    case GravityDirection.DOWN:
                        moveDirection.y = jumpHeight;
                        break;
                    case GravityDirection.UP:
                        moveDirection.y = -jumpHeight;
                        break;
                    case GravityDirection.LEFT:
                        moveDirection.x = jumpHeight;
                        break;
                    case GravityDirection.RIGHT:
                        moveDirection.x = -jumpHeight;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Направление движения мяча в прыжке
            switch (_sceneController.GravityDirection) {
                case GravityDirection.DOWN:
                    moveDirection = new Vector3(0.5f * runSpeed, moveDirection.y,
                        0);
                    moveDirection.y -= gravity;
                    break;
                case GravityDirection.LEFT:
                    moveDirection = new Vector3(moveDirection.x, Input.GetAxis("Horizontal") * runSpeed,
                        0);
                    moveDirection.x -= gravity;
                    break;
                case GravityDirection.UP:
                    moveDirection = new Vector3(Input.GetAxis("Horizontal") * runSpeed, moveDirection.y,
                        0);
                    moveDirection.y += gravity;
                    break;
                case GravityDirection.RIGHT:
                    moveDirection = new Vector3(moveDirection.x, Input.GetAxis("Horizontal") * runSpeed,
                        0);
                    moveDirection.x += gravity;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CollisionFlags collisionFlags = _characterController.Move(moveDirection * Time.deltaTime);

            if (CheckCollision(collisionFlags)) {
                SwichMoveType(true);
            }
        }
    }

    private bool CheckCollision(CollisionFlags collisionFlags) {
        return collisionFlags == CollisionFlags.CollidedBelow && _sceneController.GravityDirection == GravityDirection.DOWN || 
               collisionFlags == CollisionFlags.CollidedAbove && _sceneController.GravityDirection == GravityDirection.UP || 
               collisionFlags == CollisionFlags.CollidedSides && (_sceneController.GravityDirection == GravityDirection.LEFT ||_sceneController.GravityDirection== GravityDirection.RIGHT );
    }
    
    // ищем на какой куб можно перейти
    private Vector3 FindNextDestination(Vector3 currentCubePosition) {
        _rebroSize = 2.5f;

        //Позиция мячика на кубе
        Vector3 ballPos = _rebroSize * _sceneController.GravityDirection.GetOppositeVector();
        //Вектор из центра куба в сторону направления движения
        Vector3 balRight = _rebroSize * _sceneController.GravityDirection.GetRigthVector();
        //Координаты края куба, по направлению движения
        Vector3 edgePos = ballPos + balRight;

        Vector3 up = edgePos + _rebroSize * _sceneController.GravityDirection.GetUpVector();
        Vector3 forward = edgePos + _rebroSize * _sceneController.GravityDirection.GetRigthVector();
        Vector3 down = edgePos + _rebroSize * _sceneController.GravityDirection.GetDownVector();

        if (_navMeshAgent.CalculatePath(currentCubePosition + up, new NavMeshPath())) {
            Debug.Log("up1 " + up);
            _sceneController.ChangeGravityRigth(Direction.UP);
            return currentCubePosition + up;
        }

        if (_navMeshAgent.CalculatePath(currentCubePosition + forward, new NavMeshPath())) {
            Debug.Log("fr1 " + forward);
            return currentCubePosition + forward;
        }

        if (_navMeshAgent.CalculatePath(currentCubePosition + down, new NavMeshPath())) {
            Debug.Log("dn1 " + down );
            _sceneController.ChangeGravityRigth(Direction.DOWN);
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
    }
}