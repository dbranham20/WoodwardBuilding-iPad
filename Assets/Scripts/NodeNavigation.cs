using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INodeNavigationInterface{

    void pathCalculationCompleled();

}

public class NodeNavigation : MonoBehaviour {

    public INodeNavigationInterface delegator;
    NavigationViz navViz = new NavigationViz();
    
    public static NodeNavigation instance;
    public TextAsset textFile;
    // Start is called before the first frame update
    public static string startNode, destination;
    Dictionary<string, ArrayList> graph = new Dictionary<string, ArrayList>();
    public static ArrayList possiblePaths = new ArrayList();
    private void Awake(){
        if (instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }

    }

    void Start()
    {
        GenerateGraph();

        delegator = navViz;
        startNode = "4042";
        destination = "130";

        GetAllPaths();
        if (possiblePaths.Count == 0){
            print("No paths have been found");
        }else{
            print("there are " + possiblePaths.Count + " possible paths ");
        }
        navViz.pathCalculationCompleled();
    }

    private void GenerateGraph()
    {
        string text = textFile.text;  //this is the content as string
        //print("the text in the file is:\n " + text);

        string[] nodeCollection = text.Split('\n');

        foreach(string nodeList in nodeCollection)
        {
            string[] nodes = nodeList.Split(' ');
            var key = nodes[0];
            var childrens = new ArrayList();
            for (int i = 1; i < nodes.Length; i++){
                //print("Adding child to node "+ key +" as " + nodes[i]);
                childrens.Add(nodes[i]);
            }
            graph.Add(key, childrens);
        }
        print("Graph is generated");

        //delegator.pathCalculationCompleled();

    }

    private void GetAllPaths(){

        var paths = new ArrayList();
        paths.Add(startNode);

        Queue queue = new Queue();
        queue.Enqueue(paths);


        while (queue.Count > 0){
            var temporaryPath = queue.Dequeue() as ArrayList;
            var last = temporaryPath[temporaryPath.Count - 1] as string;

            //Get list of nodes that this last element of the path can reach to 
            //print("Trying to access children for node " + last + " in the graph");
            var children = graph[last] as ArrayList;

            //print("The last element is of current path is " + last);
            if (last == destination){
                print("A path has been found.");
                possiblePaths.Add(temporaryPath);
                string pathPrint = "";
                for (int l = 0; l < temporaryPath.Count; l++){
                    //print(temporaryPath[l]);
                    pathPrint += " -> ";
                    pathPrint += temporaryPath[l];
                }
                print("It goes like : "+pathPrint);
            }

            //for each child create new path and new paths to the queue.
            for (int i = 0; i < children.Count; i++){
                string child = children[i] as string;
                if (!existsInPath(child, temporaryPath)){
                    ArrayList newPath = new ArrayList();
                    foreach (string element in temporaryPath)
                    {
                        newPath.Add(element);
                    }
                    newPath.Add(child);
                    queue.Enqueue(newPath);
                }
            }
        }



    }

    private bool existsInPath(string child, ArrayList temporaryPath)
    {
        for (int i = 0; i < temporaryPath.Count; i++){
            if (temporaryPath[i] as string == child){
                return true;
            }
        }
        return false;
    }
}



//Extre code to be deleted
class Node{

    Dictionary<string, int> connections;
    string name;
    //string parent;
    public Node(string name, string parent){
        this.name = name;
        //this.parent = parent;
    }


    public void AddConnection(string nodeName, int distance){

        if (connections == null){
            connections = new Dictionary<string, int>();
        }

        connections.Add(nodeName, distance);

    }

}




