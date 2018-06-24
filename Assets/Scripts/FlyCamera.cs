using System;
using UnityEngine;

public class FlyCamera : MonoBehaviour {
    public float speed = 15f;
    public float zPosition = 10f;
    float rotationZ;
    float EndrotationZ;
    private bool rotateNextClockwise;

    private AgentController agentController;
    private Vector3 _wantedPosition;

    public void RotateNextClockwise(Direction gravityDirection) {
        rotateNextClockwise = true;
        Rotate(gravityDirection);
    }

    public void RotateCounterClockwise(Direction gravityDirection) {
        rotateNextClockwise = false;
        Rotate(gravityDirection);
    }

    private void Rotate(Direction gravityDirection) {
        switch (gravityDirection) {
            case Direction.DOWN:
                EndrotationZ = 0f;
                break;
            case Direction.LEFT:
                EndrotationZ = 90f;
                break;
            case Direction.UP:
                EndrotationZ = 179f;
                break;
            case Direction.RIGHT:
                EndrotationZ = -90f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Update() {
        if (agentController == null) {
            return;
        }

        // поворот
        Quaternion zQuaternion = Quaternion.AngleAxis(EndrotationZ, Vector3.back);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, zQuaternion, Time.deltaTime * 30f);

        // позиция
        _wantedPosition = agentController.transform.position;
        _wantedPosition.z -= zPosition;
        transform.position = Vector3.MoveTowards(transform.position, _wantedPosition, speed * Time.deltaTime);
    }

    public void SetAgent(GameObject agent) {
        agentController = agent.GetComponent<AgentController>();
    }
}