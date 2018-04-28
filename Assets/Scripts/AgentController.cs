﻿using System;
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
            _jumpPressed = true;
        }


        // прыгаем
        if (_characterController.enabled) {
            if (_jumpPressed) {
                _jumpPressed = false;
                //Выставить высоту прыжка
                switch (_sceneController.GravityDirection) {
                    case Direction.DOWN:
                        moveDirection.y = jumpHeight;
                        break;
                    case Direction.UP:
                        moveDirection.y = -jumpHeight;
                        break;
                    case Direction.LEFT:
                        moveDirection.x = jumpHeight;
                        break;
                    case Direction.RIGHT:
                        moveDirection.x = -jumpHeight;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Направление движения мяча в прыжке
            switch (_sceneController.GravityDirection) {
                case Direction.DOWN:
                    moveDirection = new Vector3(0.5f * runSpeed, moveDirection.y,
                        0);
                    moveDirection.y -= gravity;
                    break;
                case Direction.LEFT:
                    moveDirection = new Vector3(moveDirection.x, Input.GetAxis("Horizontal") * runSpeed,
                        0);
                    moveDirection.x -= gravity;
                    break;
                case Direction.UP:
                    moveDirection = new Vector3(Input.GetAxis("Horizontal") * runSpeed, moveDirection.y,
                        0);
                    moveDirection.y += gravity;
                    break;
                case Direction.RIGHT:
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
    }
}