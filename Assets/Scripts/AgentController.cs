using System;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

public class AgentController : MonoBehaviour {
    Vector3 moveDirection = Vector3.zero;
    float runSpeed = 30.0f;
    float gravity = 1.0f;
    float jumpHeight = 15.0f;
    private CharacterController _characterController;
    private NavMeshAgent _navMeshAgent;
    private const int STEP = 5;
    private bool _jumpPressed = false;
    private float elapsed = 0.0f;

    public GravityDirection _gravityDirection = GravityDirection.DOWN;

    void Start() {
        _characterController = gameObject.GetComponent<CharacterController>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

        SwichMoveType(true);
    }

    void Update() {
        
        // двигаемся по плоскости
        if (_navMeshAgent.enabled && Input.GetKeyDown(KeyCode.RightArrow)) {
            GameObject surface = ((Component) _navMeshAgent.navMeshOwner).gameObject;

            Debug.Log("текущий куб "+ surface.name + " " + surface.transform.position);
            Vector3 nextForwardCube = FindForwardCube(surface.transform.position);

            NavMeshPath navMeshPath = new NavMeshPath();
            // пытаемся перейти на следующий куб
            bool path = _navMeshAgent.CalculatePath(nextForwardCube, navMeshPath);
            bool calculatePath = path;

            Debug.Log("расчет позиция     "+ nextForwardCube);

            if (calculatePath) {
                Debug.Log("Перезодим на след куб " + nextForwardCube);
                _navMeshAgent.SetDestination(nextForwardCube);
            } else {

                // переворачиваемся
                Vector3 nextSurfaceCurrentCube = FindRightCubeSurface(surface.transform.position);
                //todo менять направление гравитации в момент когда шарик на линке
                _gravityDirection = _gravityDirection.Rigth();
                Debug.Log("расчет позиция     "+ nextSurfaceCurrentCube);

                if (_navMeshAgent.CalculatePath(nextSurfaceCurrentCube, new NavMeshPath())) {
                    Debug.Log("Переходим на другую стороно текущего куба " +nextSurfaceCurrentCube);
                    _navMeshAgent.SetDestination(nextSurfaceCurrentCube);
                }
            }
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
                switch (_gravityDirection) {
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
            switch (_gravityDirection) {
                case GravityDirection.DOWN:
                    moveDirection = new Vector3(Input.GetAxis("Horizontal") * runSpeed, moveDirection.y,
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

            if (collisionFlags != CollisionFlags.None) {
                SwichMoveType(true);
            }
        }
    }

    // переходим на правую плоскость куба
    private Vector3 FindRightCubeSurface(Vector3 currentSurfacePosition) {
        Vector3 forwardCube;
        float halfStep = (float) STEP / 2;
        switch (_gravityDirection) {
            case GravityDirection.DOWN:
                //todo брать отстаток от деления на STEP|2
                forwardCube = new Vector3(currentSurfacePosition.x + halfStep, currentSurfacePosition.y - halfStep,
                    currentSurfacePosition.z);
                break;
            case GravityDirection.LEFT:
                forwardCube = new Vector3(currentSurfacePosition.x - halfStep, currentSurfacePosition.y - halfStep,
                    currentSurfacePosition.z);
                break;
            case GravityDirection.UP:
                forwardCube = new Vector3(currentSurfacePosition.x - halfStep, currentSurfacePosition.y + halfStep,
                    currentSurfacePosition.z);
                break;
            case GravityDirection.RIGHT:
                forwardCube = new Vector3(currentSurfacePosition.x + halfStep, currentSurfacePosition.y + halfStep,
                    currentSurfacePosition.z);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return forwardCube;
    }

    // Ищем позицию следующего куба
    private Vector3 FindForwardCube(Vector3 currentSurfacePosition) {
        Vector3 dest;

        // составляем путь прямо, на следующий куб
        switch (_gravityDirection) {
            case GravityDirection.DOWN:
                dest = new Vector3(currentSurfacePosition.x + STEP, currentSurfacePosition.y,
                    currentSurfacePosition.z);
                break;
            case GravityDirection.LEFT:
                dest = new Vector3(currentSurfacePosition.x, currentSurfacePosition.y - STEP,
                    currentSurfacePosition.z);
                break;
            case GravityDirection.UP:
                dest = new Vector3(currentSurfacePosition.x - STEP, currentSurfacePosition.y,
                    currentSurfacePosition.z);
                break;
            case GravityDirection.RIGHT:
                dest = new Vector3(currentSurfacePosition.x, currentSurfacePosition.y + STEP,
                    currentSurfacePosition.z);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return dest;
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