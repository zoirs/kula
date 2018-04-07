using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour {
    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Teleport;
    private AgentController _agentController;

    IEnumerator Start () {
        NavMeshAgent agent = GetComponent<NavMeshAgent> ();
        _agentController = GetComponent<AgentController> ();
        agent.autoTraverseOffMeshLink = false;
        while (true) {
            if (agent.isOnOffMeshLink) {
                Debug.Log(method);
                if (method == OffMeshLinkMoveMethod.NormalSpeed)
                    yield return StartCoroutine (NormalSpeed (agent));
                else if (method == OffMeshLinkMoveMethod.Parabola)
                    yield return StartCoroutine (Parabola (agent, 2.0f, 0.5f));
                else if (method == OffMeshLinkMoveMethod.Rotated)
                    yield return StartCoroutine (Rotated(agent));
                agent.CompleteOffMeshLink ();
            }
            yield return null;
        }
    }
    IEnumerator NormalSpeed (NavMeshAgent agent) {
        Debug.Log("norm");
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos + Vector3.up*agent.baseOffset;
        var quaternion = new Quaternion(0,0,1,0);
        FlyCamera flyCamera = Camera.main.GetComponent<FlyCamera>();
        if (flyCamera != null)
        {
            switch (_agentController._gravityDirection)
            {
                case GravityDirection.DOWN:
                    flyCamera.Rotate(0f);
                    break;
                case GravityDirection.LEFT:
                    flyCamera.Rotate(90f);
                    break;
                case GravityDirection.UP:
                    flyCamera.Rotate(180f);
                    break;
                case GravityDirection.RIGHT:
                    flyCamera.Rotate(270f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        while (agent.transform.position != endPos) {
//            gameObject.transform.rotation =gameObject.transform.rotation*
//                                           new Quaternion(0, 0 , 1 , 1f*Time.deltaTime);
            agent.transform.position = Vector3.MoveTowards (agent.transform.position, endPos, agent.speed*Time.deltaTime);
//            agent.transform.position = Vector3.RotateTowards(agent.transform.position, endPos, agent.speed*Time.deltaTime, 0.0F);
            Vector3 newDir = Vector3.RotateTowards(agent.transform.position, Vector3.down, 1f*Time.deltaTime, 0.0f);
//            agent.transform.rotation = Quaternion.LookRotation(newDir);

            var oldRot =  agent.transform.rotation;
//            agent.transform.rotation = transform.rotation * new Quaternion(0, 0 , 1 , 10f * Time.deltaTime);
            quaternion = quaternion * new Quaternion(0, 0, 1, 1f);
                        agent.transform.rotation = Quaternion.LookRotation(quaternion.eulerAngles);

            Debug.Log(quaternion.eulerAngles);
            yield return null;
        }
    }
    
    IEnumerator Rotated(NavMeshAgent agent) {
        Debug.Log("rotated");
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = Quaternion.AngleAxis(90, Vector3.forward) * data.endPos + Vector3.up*agent.baseOffset;
//        Vector3 foo = Quaternion.AngleAxis(90, Vector3.forward) * endPos;
        int endRot = 90;
        while (agent.transform.position != endPos) {
            agent.transform.position = Vector3.MoveTowards (agent.transform.position, endPos, agent.speed*Time.deltaTime);
//            agent.transform.Rotate(new Vector3(0,0,agent.transform.rotation.z + 10*Time.deltaTime));
            //Vector3 newDir = Vector3.RotateTowards(agent.transform.position, endPos, step, 0.0f);
            yield return null;
        }
    }
    
    IEnumerator Parabola (NavMeshAgent agent, float height, float duration) {
        Debug.Log("parab");
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up*agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f) {
            float yOffset = height * 4.0f*(normalizedTime - normalizedTime*normalizedTime);
            agent.transform.position = Vector3.Lerp (startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}