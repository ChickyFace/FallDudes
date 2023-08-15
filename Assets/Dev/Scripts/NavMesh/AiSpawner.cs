using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions.Must;

public class AiSpawner : MonoBehaviour
{
    public Transform FinishPoint;
    
    public int numberOfOppenetToSpawn;
    public float spawnDelay;

    public List<Oppenent> OppenentPrefabs = new List<Oppenent>();
    public SpawnMethod OppenentSpawnMethod;

    public bool gameStart;

   // private NavMeshTriangulation triangulation; // in all navmesh area 
    public Transform platform;
    public Dictionary<int, ObjectPool> OppenentObjectPools = new Dictionary<int, ObjectPool>();
    private void Awake()
    {
        for (int i = 0; i < OppenentPrefabs.Count; i++)
        {
            OppenentObjectPools.Add(i, ObjectPool.CreateInstance(OppenentPrefabs[i], numberOfOppenetToSpawn));
        }
    }
    private void Start()
    {

        //triangulation = NavMesh.CalculateTriangulation();
      


    }

    IEnumerator WaitHalfSecond()
    {
        //This is where you would put the code that needs to run before the delay

       
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds


        //This is where you put the code that you want to run after the 0.5-second delay
    }
    private void Update()
    {
        if (gameStart)
        {
            StartCoroutine(SpawnOppenets());
            Debug.Log("Game Start : " + gameStart);
            gameStart = false;
            //StartCoroutine(WaitSpawnOpponents());

        }
       


    }
    private IEnumerator SpawnOppenets()
    {
        WaitForSeconds Wait = new WaitForSeconds(spawnDelay);
        int SpawnedOppenents = 0;

        while (SpawnedOppenents < numberOfOppenetToSpawn)
        {
            if(OppenentSpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinOppenent(SpawnedOppenents);
            }
            else if (OppenentSpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomOppenent();
            }
           
            SpawnedOppenents++;


            yield return Wait;
        }

    }
    private void SpawnRoundRobinOppenent(int SpawnedOppenents)
    {
        int SpawnIndex = SpawnedOppenents % OppenentPrefabs.Count; // 0 % 2 = 0, 1 % 2 = 1, 2 % 2 = 0
        DoSpawnOppenent(SpawnIndex);
    }
    private void SpawnRandomOppenent()
    {
        DoSpawnOppenent(Random.Range(0, OppenentPrefabs.Count));
    }
    private void DoSpawnOppenent(int spawnIndex)
    {
        PoolableObject poolableObject = OppenentObjectPools[spawnIndex].GetObject();


        if(poolableObject !=  null)
        {
            Oppenent oppenent = poolableObject.GetComponent<Oppenent>();


            // int VertexIndex = Random.Range(0, triangulation.vertices.Length);

            // Get the dimensions and position of the platform
            Bounds platformBounds = platform.GetComponent<Renderer>().bounds;
            Vector3 platformCenter = platformBounds.center;
            Vector3 platformExtents = platformBounds.extents;

            // Generate a random position within the platform's area
            Vector3 randomPosition = new Vector3(
                Random.Range(platformCenter.x - platformExtents.x, platformCenter.x + platformExtents.x),
                platformCenter.y,
                Random.Range(platformCenter.z - platformExtents.z, platformCenter.z + platformExtents.z)
            );

            NavMeshHit Hit;

            if (NavMesh.SamplePosition(randomPosition, out Hit, 2f, -1))  
                /*if (NavMesh.SamplePosition(triangulation.vertices[VertexIndex], out Hit, 2f, -1 )) */
            {
                oppenent.agent.Warp(Hit.position);   //oppenent needs to get enabled and start chasing now
                oppenent.movement.followTarget = FinishPoint;
                oppenent.agent.enabled = true;
                oppenent.movement.StartChasing();
                string opponentTag = oppenent.gameObject.tag;

                RatingManager.instance.SetOpponentData(oppenent.transform, opponentTag);

            }


        }
        else
        {
            Debug.LogError($"Unable to fetch oppenent of type {spawnIndex} from object pool, Out of objects?");
        }
    }
    public enum SpawnMethod
    {
        RoundRobin,
        Random
    }


}
