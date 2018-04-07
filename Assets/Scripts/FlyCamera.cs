using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class FlyCamera : MonoBehaviour {
 // Присваиваем переменные
        public float mouseSensitivity = 3f;
        public float speed = 5f;
        private Vector3 transfer;
        public float minimumX = -360f;
        public float maximumX = 360f;
        public float minimumY = -60f;
        public float maximumY = 60f;
        public float minimumZ = -360f;
        public float maximumZ = 360f;
        float rotationX = 0f;
        float rotationY = 0f;
        float rotationZ = 0f;
        float EndrotationZ = 0f;
        Quaternion originalRotation;

        public AgentController agentController;
        
        void Awake(){
//                camera.orthographic = false;
        }
        
        void Start(){
                originalRotation = transform.rotation;
        }

        public void Rotate(float rotation)
        {
                EndrotationZ = rotation;
        }

        void Update(){
                // Движения мыши -> Вращение камеры
//                rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
//                rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
                if (!FloatComparer.AreEqual(rotationZ, EndrotationZ, 3f)){
                        
                        rotationZ += Time.deltaTime * 30f;
                
        }
//                rotationY += Time.deltaTime * 2f;
//                rotationX = ClampAngle (rotationX, minimumX, maximumX);
//                rotationY = ClampAngle (rotationY, minimumY, maximumY);
                rotationZ = ClampAngle (rotationZ, minimumZ, maximumZ);
                Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
                Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
                Quaternion zQuaternion = Quaternion.AngleAxis (rotationZ, Vector3.back);
                transform.rotation = originalRotation * xQuaternion * yQuaternion * zQuaternion ;

                // Ускорение при нажатии клавиши Shift
                if (Input.GetKeyDown(KeyCode.LeftShift))
                        speed *= 10;
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                        speed /= 10;

                // Поднятие и опускание камеры
                float offset = 5;
                if (!FloatComparer.AreEqual(agentController.transform.position.y + offset, transform.position.y, 1))
                {
                        if (agentController.transform.position.y + offset > transform.position.y)
                        {
                                Vector3 newPos = new Vector3(0, 1, 0);
                                transform.position += newPos * speed * Time.deltaTime;
                        }
                        else
                        {
                                Vector3 newPos = new Vector3(0, 1, 0);
                                transform.position -= newPos * speed * Time.deltaTime;
                        }
                }
                
                if (!FloatComparer.AreEqual(agentController.transform.position.x + offset, transform.position.x, 1))
                {
                        if (agentController.transform.position.x + offset > transform.position.x)
                        {
                                Vector3 newPos = new Vector3(1, 0, 0);
                                transform.position += newPos * speed * Time.deltaTime;
                        }
                        else
                        {
                                Vector3 newPos = new Vector3(1, 0, 0);
                                transform.position -= newPos * speed * Time.deltaTime;
                        }
                }
//
//                Vector3 newPos = new Vector3(0, 1, 0);
//                if (Input.GetKey(KeyCode.E))
//                        transform.position += newPos * speed * Time.deltaTime;
//                else if (Input.GetKey(KeyCode.Q))
//                        transform.position -= newPos * speed * Time.deltaTime;

                // перемещение камеры
//                transfer = transform.forward * Input.GetAxis("Vertical");
//                transfer += transform.right * Input.GetAxis("Horizontal");
//                transform.position += transfer * speed * Time.deltaTime;
        }
        
        public static float ClampAngle (float angle, float min, float max){
                if (angle < -360F) angle += 360F;
                if (angle > 360F) angle -= 360F;
                return Mathf.Clamp (angle, min, max);
        }
        
}
