using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationViz : INodeNavigationInterface {


    ArrayList allPaths = new ArrayList();
    int shortTestPathIndex;

    public void pathCalculationCompleled()
    {
        Debug.Log("Paths have been calculated");
        allPaths = NodeNavigation.possiblePaths;

        scanPaths();

        drawPathForPathAtIndex(shortTestPathIndex);
    }

    private void drawPathForPathAtIndex(int index)
    {
        ArrayList pathSelected = new ArrayList();
        pathSelected = allPaths[index] as ArrayList;
        LineRenderer line = null;

        int i = 0;
        foreach (string node in pathSelected){
            GameObject nodeObject = GameObject.Find(node);
            if(nodeObject == null){
                Debug.Log("GameObject named " + node + "is not found.. Please check");
            }
            if(i == 0){
                line = nodeObject.AddComponent<LineRenderer>();
                // Set the width of the Line Renderer
                line.startWidth = 1f;
                line.endWidth = 1f;
                // Set the number of vertex fo the Line Renderer
                line.SetVertexCount(pathSelected.Count);
            }
            if(nodeObject == null){
                Debug.Log(node + "is not found.");
                return;
            }
            line.SetPosition(i, nodeObject.transform.position);
            i++;
        }
    }

    private void scanPaths()
    {
        var i = 0;
        foreach (ArrayList path in allPaths)
        {
            var weight = findWeight(path);
            Debug.Log("Weight of path " + (i+1) + " is "+ weight);
            i++;
        }
    }

    private float findWeight(ArrayList path)
    {
        if (path.Count == 1){
            Debug.Log("Weight is Zero");
            return 0;
        }
        //GameObject source;
        GameObject consequentNode = null;
        int i = 0;
        float pathWeight = 0;
        foreach (string node in path){

            if(i == 0){
                consequentNode = GameObject.Find(node);
            }else{

                GameObject nextNode = GameObject.Find(node);
                if(nextNode == null || consequentNode == null){
                    if(consequentNode == null){
                        Debug.Log("Source is not there at place");
                    }
                    if (nextNode == null){
                        Debug.Log(node + " gameobject not found.");
                    }
                    return 0;
                }
                Vector3 difference = consequentNode.transform.position - nextNode.transform.position;
                var distance = difference.magnitude;
                pathWeight += distance;
                consequentNode = nextNode;
            }

            i += 1;

        }
        return pathWeight;
    }
}
