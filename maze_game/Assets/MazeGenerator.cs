using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject Player;
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] Vector2Int mazeSize;

    public void Start()
    {
        //StartCoroutine(GenerateMaze(mazeSize));
        GenerateMazeInstant(mazeSize);
    }

    void GenerateMazeInstant(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        //create maze
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3((x - size.x/2f), 0, y - (size.y / 2f));
                Player.transform.position = nodePos; //remove later
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        
        /*centered in 10 x 10
            int halfWayTen = (int)(nodes.Count / 1.8);*/

        //choose starting node in center off odd number dimensions
        currentPath.Add(nodes[nodes.Count / 2]);

        //try tp get player to spawn at the starting node
        
        //currentPath.add(nodes[nodes.Count/2]);
        currentPath[0].setState(NodeState.Start);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) && 
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
                //chosenNode.setState(NodeState.Current);
            }

            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                //currentPath[currentPath.Count - 1].setState(NodeState.Completed);

                currentPath.RemoveAt(currentPath.Count - 1);
            }

        }

        MazeNode ending = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp1 = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp2 = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp3 = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp4 = nodes[Random.Range(0, nodes.Count - 1)];

        ending.setState(NodeState.End);
        pickUp1.setState(NodeState.PickUp);
        pickUp2.setState(NodeState.PickUp);
        pickUp3.setState(NodeState.PickUp);
        pickUp4.setState(NodeState.PickUp);

        ending.setState(NodeState.End);
        Debug.Log("Completed");
        
    }

    IEnumerator GenerateMaze(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        //create maze
        for(int x = 0; x < size.x; x++)
        {
            for(int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3((x - size.x/2f), 0, y - (size.y / 2f));
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                yield return null;
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        //choose starting node
        //centered in 10 x 10
            //int halfWayTen = (int)(nodes.Count / 1.8);
        currentPath.Add(nodes[nodes.Count / 2]);
        //to start in center
        //currentPath.add(nodes[nodes.Count/2]);
        currentPath[0].setState(NodeState.Start);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to the current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) && 
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }

                currentPath.Add(chosenNode);
                chosenNode.setState(NodeState.Current);
            }

            else
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);
                //currentPath[currentPath.Count - 1].setState(NodeState.Completed);

                currentPath.RemoveAt(currentPath.Count - 1);
            }

            yield return new WaitForSeconds(0.05f);
        }

        MazeNode ending = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp1 = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp2 = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp3 = nodes[Random.Range(0, nodes.Count - 1)];
        MazeNode pickUp4 = nodes[Random.Range(0, nodes.Count - 1)];

        ending.setState(NodeState.End);
        pickUp1.setState(NodeState.PickUp);
        pickUp2.setState(NodeState.PickUp);
        pickUp3.setState(NodeState.PickUp);
        pickUp4.setState(NodeState.PickUp);


        Debug.Log("Completed");
        
    }
}

