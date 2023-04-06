using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This helps to define the contents of the Node. 
//This is meant to be only an interface, it is not meant to be instantiated.

[System.Serializable]
//create Node Class
public abstract class Node
{

    //Each node should be able to be obtained to check its' contents, delegates are used.
    //Delegate is done so that you are able to find the right content without needing large quantities of data.
    public delegate NodeStates NodeReturn();

    //Each node should have a node state. This should show its state at any given point.
    //This will return if it is in the following states: FAILURE, SUCCESS, RUNNING.
    protected NodeStates m_nodeState;

    public NodeStates nodeState
    {
        get { return m_nodeState; }
    }

    //Constructor
    public Node()
    {

    }

    //Each node should have a function that allows you to evaluate node conditions.
    //this helps to determine the state of the node.
    public abstract NodeStates Evaluate();


}

