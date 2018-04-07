using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        if (Input.GetKeyDown ("space")) {
        
            Debug.Log("1111111");
        }

        {
        }

        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
            {
//                Vector3 position = m_Agent.gameObject.transform.position;
//                Vector3 destination = new Vector3(position.x + 5, position.y, position.z);
//                
//                if (!m_Agent.SetDestination(destination))
//                {
//                    destination = new Vector3(position.x + 5, position.y - 5, position.z);
//                    m_Agent.SetDestination(destination);
//                }
                m_Agent.destination = m_HitInfo.point;
            }
        }
    }
}
