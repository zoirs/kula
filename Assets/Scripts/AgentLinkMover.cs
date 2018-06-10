﻿using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;

[Obsolete("Похоже пока нет нужды")]
[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour {
    public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Teleport;
    private SceneController _scene;
    private AgentController _agentController;

    IEnumerator Start() {
        _scene = GameObject.Find("Scene").GetComponent<SceneController>();

        if (_scene == null) {
            Debug.LogWarning("Scene is null");
        }

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        _agentController = GetComponent<AgentController>();
        agent.autoTraverseOffMeshLink = false;
        while (true) {
            if (agent.isOnOffMeshLink) {
                if (method == OffMeshLinkMoveMethod.NormalSpeed) {
                    yield return StartCoroutine(NormalSpeed(agent));
                } else if (method == OffMeshLinkMoveMethod.Parabola) {
//                    yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                }
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 endPos = data.endPos;// + _scene.GravityDirection.GetOppositeVector() * agent.baseOffset * 2;
//        FlyCamera flyCamera = Camera.main.GetComponent<FlyCamera>();
//        if (flyCamera != null) {
//            flyCamera.Rotate(_scene.GravityDirection);
//        }
        while (agent.transform.position != endPos) {
            Vector3 position = agent.transform.position ;
            agent.transform.position = Vector3.MoveTowards(position, endPos, agent.speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Parabola(NavMeshAgent agent, float height, float duration) {
        OffMeshLinkData data = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
        float normalizedTime = 0.0f;
        while (normalizedTime < 1.0f) {
            float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / duration;
            yield return null;
        }
    }
}