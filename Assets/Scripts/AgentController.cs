using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {
	Vector3 moveDirection = Vector3.zero;
	float walkSpeed  = 5.0f;
	float runSpeed = 10.0f;
	float gravity =  1.0f;
	float jumpHeight = 15.0f;
	private CharacterController _characterController;
	private NavMeshAgent _navMeshAgent;
	private const int STEP = 10;
	
	private float elapsed = 0.0f;
	private bool _isGo;

	public GravityDirection _gravityDirection = GravityDirection.DOWN;
	
	void Start()
	{
		_isGo = false;
		_characterController = gameObject.GetComponent<CharacterController>();
		_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();

		SwichMoveType(true);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
//		_camera.transform.rotation =_camera.transform.rotation* new Quaternion(0, 0 , 1 , 0.001f );
//		_camera.transform.position =

//		Camera.main.transform.rotation =Quaternion.FromToRotation(Vector3.forward, Vector3.back) ;
		
		
		if(Input.GetKey(KeyCode.Space))
		{
			

			SwichMoveType(false);
//			_navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
//			if (_navMeshAgent != null)
//			{
//				_navMeshAgent.enabled = false;
//			}
//			
			switch (_gravityDirection)
			{
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
		NavMeshPath path = new NavMeshPath();

		

		if (_navMeshAgent.enabled && Input.GetKeyDown(KeyCode.RightArrow))
		{

			Debug.Log(_navMeshAgent.navMeshOwner.name);

			UnityEngine.Object owner = _navMeshAgent.navMeshOwner;
			GameObject surface = ((Component) owner).gameObject;
			//NavMeshDataInstance.owner(_navMeshAgent.navMeshOwner.name);
			Vector3 currentSurfacePosition = surface.transform.position;
			Vector3 dest;

			// составляем путь прямо, на следующий куб
			switch (_gravityDirection)
			{
				case GravityDirection.DOWN:
					dest = new Vector3(currentSurfacePosition.x + STEP, currentSurfacePosition.y , currentSurfacePosition.z);
					break;
				case GravityDirection.LEFT:
					dest = new Vector3(currentSurfacePosition.x, currentSurfacePosition.y - STEP , currentSurfacePosition.z);
					break;
				case GravityDirection.UP:
					dest = new Vector3(currentSurfacePosition.x-STEP, currentSurfacePosition.y, currentSurfacePosition.z);
					break;
				case GravityDirection.RIGHT:
					dest = new Vector3(currentSurfacePosition.x, currentSurfacePosition.y+STEP, currentSurfacePosition.z);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			// пытаемся перейти на следующий куб
			if (_navMeshAgent.CalculatePath(dest, new NavMeshPath()))
			{
				_navMeshAgent.SetDestination(dest);
			}
			else
			{
				// переворачиваемся
				GravityDirection nextGravityValue;
				switch (_gravityDirection)
				{
					case GravityDirection.DOWN:
						nextGravityValue = GravityDirection.LEFT;
						dest = new Vector3(currentSurfacePosition.x + STEP/2, currentSurfacePosition.y - STEP/2, currentSurfacePosition.z);	
						break;
					case GravityDirection.LEFT:
						nextGravityValue = GravityDirection.UP;
						dest = new Vector3(currentSurfacePosition.x - STEP/2, currentSurfacePosition.y - STEP/2, currentSurfacePosition.z);	
						break;
					case GravityDirection.UP:
						nextGravityValue = GravityDirection.RIGHT;
						dest = new Vector3(currentSurfacePosition.x - STEP/2, currentSurfacePosition.y + STEP/2, currentSurfacePosition.z);	
						break;
					case GravityDirection.RIGHT:
						nextGravityValue = GravityDirection.DOWN;
						dest = new Vector3(currentSurfacePosition.x + STEP/2, currentSurfacePosition.y + STEP/2, currentSurfacePosition.z);	
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				_gravityDirection = nextGravityValue;
//				dest = new Vector3(currentSurfacePosition.x + STEP/2, currentSurfacePosition.y - STEP/2-1, currentSurfacePosition.z);	
//				dest = new Vector3(8.3f, -3, position.z);	
				
				 path = new NavMeshPath();
				_navMeshAgent.CalculatePath(dest, path);
				if (path.status != NavMeshPathStatus.PathInvalid) {
					_navMeshAgent.SetDestination(dest);
					Debug.Log("dest " + dest);
					_isGo = true;
				}
//				if (_navMeshAgent.CalculatePath(dest, new NavMeshPath()))
//				{
//					_navMeshAgent.SetDestination(dest);
//				}
				
				
			}

		}

		if (_isGo)
		{
			if (!_navMeshAgent.pathPending)
			{
				if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
				{
					if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
					{
						_isGo = false;
						Debug.Log("123");
//						SwichMoveType(false);
					}
				}
			}
		}

		if (_characterController.enabled)
		{
//			if (Input.GetKey(KeyCode.LeftShift))
//				moveDirection = new Vector3(Input.GetAxis("Horizontal") * walkSpeed, moveDirection.y,
//					Input.GetAxis("Vertical") * walkSpeed);
//			else
//			{
//				moveDirection = new Vector3(Input.GetAxis("Horizontal") * runSpeed, moveDirection.y,
//					Input.GetAxis("Vertical") * runSpeed);

			switch (_gravityDirection)
			{
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
					moveDirection = new Vector3(moveDirection.x,Input.GetAxis("Horizontal") * runSpeed,
						0);
					moveDirection.x += gravity;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			

//			}

			CollisionFlags collisionFlags = _characterController.Move(moveDirection * Time.deltaTime);
			Debug.Log("==+++==" + collisionFlags);

			if (collisionFlags != CollisionFlags.None)
			{
				SwichMoveType(true);
			}
		}

	}

	void OnCollisionEnter(Collision collision)
	{
		// Debug-draw all contact points and normals
		foreach (ContactPoint contact in collision.contacts)
		{
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			Debug.Log("111 "+ contact.otherCollider.gameObject.name);
		}
	}
//	void OnControllerColliderHit(ControllerColliderHit hit) {
//		Debug.Log(hit.gameObject) ;
////		if (body == null || body.isKinematic)
////			return;
////        
////		if (hit.moveDirection.y < -0.3F)
////			return;
////        
////		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
////		body.velocity = pushDir * pushPower;
//	}

	private void SwichMoveType(bool isAgent)
	{
		if (_characterController != null)
		{
			_characterController.enabled = !isAgent;			
		}

		if (_navMeshAgent != null)
		{
			_navMeshAgent.enabled = isAgent;
		}

	}
}
