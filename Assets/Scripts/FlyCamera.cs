using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class FlyCamera : MonoBehaviour {
    public float speed = 5f;
    public float minimumZ = -360f;
    public float maximumZ = 360f;
    float rotationZ;
    float EndrotationZ;
    Quaternion originalRotation;

    public AgentController agentController;

    void Start() {
        originalRotation = transform.rotation;
    }

    public void Rotate(GravityDirection gravityDirection) {
        switch (gravityDirection) {
            case GravityDirection.DOWN:
                EndrotationZ = 0f;
                break;
            case GravityDirection.LEFT:
                EndrotationZ = 90f;
                break;
            case GravityDirection.UP:
                EndrotationZ = 180f;
                break;
            case GravityDirection.RIGHT:
                EndrotationZ = 270f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void Update() {
        if (!FloatComparer.AreEqual(rotationZ, EndrotationZ, 3f)) {
            rotationZ += Time.deltaTime * 30f;
        }

        rotationZ = ClampAngle(rotationZ, minimumZ, maximumZ);
        Quaternion zQuaternion = Quaternion.AngleAxis(rotationZ, Vector3.back);
        transform.rotation = originalRotation * zQuaternion;

        // Поднятие и опускание камеры
        float offset = 5;
        if (!FloatComparer.AreEqual(agentController.transform.position.y + offset, transform.position.y, 1)) {
            if (agentController.transform.position.y + offset > transform.position.y) {
                Vector3 newPos = new Vector3(0, 1, 0);
                transform.position += newPos * speed * Time.deltaTime;
            }
            else {
                Vector3 newPos = new Vector3(0, 1, 0);
                transform.position -= newPos * speed * Time.deltaTime;
            }
        }

        if (!FloatComparer.AreEqual(agentController.transform.position.x + offset, transform.position.x, 1)) {
            if (agentController.transform.position.x + offset > transform.position.x) {
                Vector3 newPos = new Vector3(1, 0, 0);
                transform.position += newPos * speed * Time.deltaTime;
            }
            else {
                Vector3 newPos = new Vector3(1, 0, 0);
                transform.position -= newPos * speed * Time.deltaTime;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max) {
        if (angle < -360F) angle += 360F;
        if (angle > 360F) angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}