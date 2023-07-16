using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicTestEnemy : MonoBehaviour
{
	[System.Serializable]
	public struct PatrolPoints
	{
		public Vector3 position;
		public float angle;
		public float stayTime;
	}
	[Header("Patrols")]
	public PatrolPoints[] patrolPoints = new PatrolPoints[0];
	int currentRoute;
	float patrolTimer;
	float currentAngle;

	[Header("Movement")]
	public float maxSpeed = 15;
	public float minSpeed = 5;
	float baseSpeed = 10;
	public float patrolSpeed = 10;
	public float distractedSpeed = 7;
	public float idleSpeed = 5;

	[SerializeField] private Transform player;
	Rigidbody rb;
	[Header("Sight controls")]
	public float sightRange = 20;
	[Range(1, 90)] public float sightAngle = 45;
	public float detectionSpeed = 10;
	public float detectionLossStart = 1;
	public float detectionLossMax = 10;
	public float detectionLossAcc = 1;
	float detectionLoss = 1;
	Vector3 toPlayer;
	RaycastHit hit;

	[Header("Detection Controls")]
	int playerPartsDetected;
	int playerPartsDetectReq = 2;
	float playerPartsApart = 0.45f;
	public float alertRange = 5;

	[Header("Detection Output")]
	[Range(0, 100)] float awareness = 0;
	public enum State { Unaware, Suspecious, Alert }
	State state = State.Unaware;

	[Header("Damage output")]
	public int dmg = 1;


	private Vector3 target;

	float dist;

	UnityEngine.AI.NavMeshAgent agent;
	float usualStoppingDistance;


	Vector3 startRot;
	Vector3 startPos;

	[Header("Distraction by rocks")]
	public float timeDistracted = 2;
	[Range(0, 100)] public float distractAwarenes = 50;
	bool distracted = false;
	float distractedTimer = 0;
	public float maxDistractionTime = 10;
	float maxDistractionTimer = 0;

	[Header("Animations")]
	public Animator anim;

	bool toClose = false;

	// Start is called before the first frame update
	void Start()
	{
		startRot = transform.eulerAngles;

		startPos = transform.position;
		rb = GetComponent<Rigidbody>();
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		usualStoppingDistance = agent.stoppingDistance;
		player = player.GetComponent<Transform>();
		
	}

	// Update is called once per frame
	void Update()
	{

		toClose = Vector3.Distance(transform.position, player.position) < 2.5;
		if (toClose)
		{
			transform.LookAt(target);

		}

		if (!distracted)
			target = player.position;
		else
			MoveTowardsDistraction();
		DetectingPlayer();
		if (GetState() == State.Alert)
			attackPlayer();
		suspecious();
		idle();
		Patrol();
		ResetVolocity();
		if (anim)
		{
			AnimateWalk();
		}

		if(Physics.Raycast(transform.position, transform.forward, out hit, 20) && hit.collider.tag=="Player"){
			Debug.LogWarning("looking at player");
			Debug.DrawRay(transform.position, transform.forward * sightRange, Color.black);
		}
	}

	void DetectingPlayer()
	{
		/*if (rewindTime.rewinding)
		{
			ResetAwareness();
			return;
		}*/
		toPlayer = player.position - transform.position;
		if (toPlayer.magnitude <= sightRange && Vector3.Angle(transform.forward, toPlayer) <= sightAngle)
		{

			playerPartsDetected = 0;
			//Debug.DrawRay(transform.position, toPlayer, Color.red);
			//if (SpottingPlayer(toPlayer))
			//	playerPartsDetected++;
			Debug.DrawRay(transform.position, toPlayer + transform.right * playerPartsApart, Color.red);
			if (SpottingPlayer(toPlayer + transform.right * playerPartsApart))
				playerPartsDetected++;
			Debug.DrawRay(transform.position, toPlayer - transform.right * playerPartsApart, Color.red);
			if (SpottingPlayer(toPlayer - transform.right * playerPartsApart))
				playerPartsDetected++;

			Debug.Log("player stats = " + playerPartsDetected);
			if (playerPartsDetected >= playerPartsDetectReq)
			{
				ChangeAwareness(detectionSpeed * playerPartsDetected / 2 * (1 - Mathf.Pow(toPlayer.magnitude / sightRange, 2)) * Time.deltaTime);
				detectionLoss = detectionLossStart;
				return;
			}
		}
		
		detectionLoss = Mathf.Clamp(detectionLoss + detectionLossAcc * Time.deltaTime, detectionLossStart, detectionLossMax);
		ChangeAwareness(-detectionLoss * Time.deltaTime);
	}
	int abc = 0;
	bool SpottingPlayer(Vector3 toPlayer)
	{
		//if (Physics.Raycast(transform.position, toPlayer, out hit, sightRange))
		Debug.DrawRay(transform.position, transform.forward*sightRange, Color.black);
		if (Physics.Raycast(transform.position, transform.forward, out hit, sightRange))
		{
			if (hit.collider.tag == "Player")
			
				return true;
		}
		return false;
	}

	void ChangeAwareness(float amount)
	{
		awareness = Mathf.Clamp(awareness + amount, 0, 100);
		if (state == State.Unaware && awareness >= 10)
		{
			state = State.Suspecious;
		}
		else if (state == State.Suspecious && awareness < 10)
		{
			state = State.Unaware;
		}
		else if (state == State.Suspecious && awareness > 90)
		{
			state = State.Alert;
			AlertOthers();
			if (anim)
				StartAlertAnimation();
		}
		else if (state == State.Alert && awareness < 20)
		{
			state = State.Suspecious;

		}
	}


	void AlertOthers()
	{
		Collider[] otherCols = Physics.OverlapSphere(transform.position, alertRange);
		for (int i = 0; i < otherCols.Length; i++)
		{
			//if (otherCols[i].GetComponent<EnemyScript>())
				//otherCols[i].GetComponent<EnemyScript>().Alert();
		}
	}

	public void Alert()
	{
		ChangeAwareness(90);
	}

	public float GetAwareness()
	{
		return awareness;
	}

	public State GetState()
	{
		return state;
	}

	void ResetAwareness()
	{
		ChangeAwareness(-100);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "Player")
		{
			//awareness = 0;
			ResetAwareness();
			//print("hit");
		}
	}


	void attackPlayer()
	{
		dist = Vector3.Distance(transform.position, target);
		if (dist > 10)
		{
			baseSpeed -= Time.deltaTime;
		}
		else
		{
			baseSpeed += Time.deltaTime;
		}
		baseSpeed = Mathf.Clamp(baseSpeed, minSpeed, maxSpeed);
		agent.speed = baseSpeed;

		agent.SetDestination(target);
	}

	void ResetVolocity()
	{
		rb.velocity = Vector3.up * rb.velocity.y;
	}

	void suspecious()
	{
		if (GetState() == State.Suspecious)
		{
			transform.LookAt(target);
			transform.eulerAngles = transform.eulerAngles.y * Vector3.up;

		}
	}

	void idle()
	{

		if (patrolPoints.Length <= 0 && GetState() == State.Unaware && !distracted)
		{
			if (Vector3.Distance(transform.position, startPos) > 0.3f)
			{
				agent.speed = idleSpeed;
				agent.SetDestination(startPos);
			}
			if (transform.eulerAngles != startRot && Vector3.Distance(transform.position, startPos) <= 0.3f)
			{
				transform.eulerAngles = startRot;
			}

		}

	}

	void Patrol()
	{
		if (patrolPoints.Length > 0 && !distracted)
		{
			if (GetState() == State.Unaware)
			{
				agent.speed = patrolSpeed;
				agent.SetDestination(patrolPoints[currentRoute].position);

				if ((patrolPoints[currentRoute].position - transform.position).magnitude < 0.3f)
				{

					currentAngle = transform.eulerAngles.y;
					currentAngle = Mathf.MoveTowardsAngle(currentAngle, patrolPoints[currentRoute].angle, agent.angularSpeed * Time.deltaTime);
					transform.eulerAngles = Vector3.up * currentAngle;

					patrolTimer += Time.deltaTime;
					if (patrolTimer > patrolPoints[currentRoute].stayTime)
					{
						currentRoute++;
						if (currentRoute >= patrolPoints.Length)
							currentRoute = 0;
						patrolTimer = 0;
					}
				}
			}
			else if (GetState() == State.Suspecious)
			{
				agent.SetDestination(transform.position);
			}
		}


	}

	public void Distract(Vector3 sourcePos)
	{
		if (GetState() != State.Alert)
		{
			distracted = true;
			target = new Vector3(sourcePos.x, transform.position.y, sourcePos.z);
			agent.stoppingDistance = 0.1f;
			ChangeAwareness(distractAwarenes - awareness);
			detectionLoss = detectionLossStart;
		}
	}

	void MoveTowardsDistraction()
	{
		agent.speed = distractedSpeed;
		if (Vector3.Distance(transform.position, target) < 1.0f)
		{
			distractedTimer += Time.deltaTime;
			if (distractedTimer >= timeDistracted)
			{
				StopBeingDistracted();
			}
		}
		else
		{
			attackPlayer();
			maxDistractionTimer += Time.deltaTime;
			if (maxDistractionTimer >= maxDistractionTime)
			{
				StopBeingDistracted();
			}
		}

	}

	void StopBeingDistracted()
	{
		distracted = false;
		distractedTimer = 0;
		maxDistractionTimer = 0;
		agent.stoppingDistance = usualStoppingDistance;
	}

	void AnimateWalk()
	{
		anim.SetBool("Moving", agent.hasPath);

		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Walking"))
			anim.speed = baseSpeed / 5;
		else
			anim.speed = 1;
	}

	void StartAlertAnimation()
	{
		anim.SetTrigger("Alert");
	}

	public void ResetPosition()
	{
		agent.SetDestination(startPos);
		transform.position = startPos;
		transform.eulerAngles = startRot;
		ChangeAwareness(-100);
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Vector3 coneEdgeLeft = transform.position + (transform.forward * Mathf.Cos(sightAngle * Mathf.Deg2Rad) - transform.right * Mathf.Sin(sightAngle * Mathf.Deg2Rad)) * sightRange;
		Vector3 coneEdgeRight = transform.position + (transform.forward * Mathf.Cos(sightAngle * Mathf.Deg2Rad) + transform.right * Mathf.Sin(sightAngle * Mathf.Deg2Rad)) * sightRange;
		Vector3 coneEdgeCenter = transform.position + transform.forward * sightRange;
		Gizmos.DrawLine(transform.position, coneEdgeLeft);
		Gizmos.DrawLine(transform.position, coneEdgeRight);
		Gizmos.DrawLine(coneEdgeCenter, coneEdgeLeft);
		Gizmos.DrawLine(coneEdgeCenter, coneEdgeRight);

	}

	/*void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		if (patrolPoints.Length <= 0)
		{
			Gizmos.DrawWireSphere(transform.position, alertRange);
		}

		for (int i = 0; i < patrolPoints.Length; i++)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, patrolPoints[i].position);

			if (i < patrolPoints.Length - 1)
				Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
			else
				Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);

			Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i].position + 2.0f * (Vector3.right * Mathf.Sin(patrolPoints[i].angle * Mathf.Deg2Rad) + Vector3.forward * Mathf.Cos(patrolPoints[i].angle * Mathf.Deg2Rad)));

			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(patrolPoints[i].position, alertRange);
		}
	}*/


}
