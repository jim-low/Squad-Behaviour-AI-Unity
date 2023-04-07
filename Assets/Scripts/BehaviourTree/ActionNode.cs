using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: May need possible arrangements, just a generic action node. Would need to see what we need to make ammendments.
//The risks of this kind of generic action node is that it only provides one delegate signature, and is unable to take any arguments.

//What is an ActionNode?
//an actionNode is a generic leaf node, leaf nodes do not have any children. And are often the finalized nodes in the trees
public class ActionNode : Node
{
    //method signature for the action
    public delegate NodeStates ActionNodeDelegate();

    //a variable to store the action that is called from delegate. 
    private ActionNodeDelegate m_action;

    //due to the node having no definition in itself,
    //it's definition has to be obtained from a delegate.
    //from the method signature declared above, it must return a NodeState enum. 
    //constructor will contain the node.
    public ActionNode(ActionNodeDelegate action) {
        m_action = action;
    }

    //proceed to evaluate the node using the passed delegate and report it's state.
    //override the Evaluate function from Node Class
    public override NodeStates Evaluate() {
        switch (m_action()) {
            case NodeStates.SUCCESS:
                m_nodeState = NodeStates.SUCCESS;
                return m_nodeState;
            case NodeStates.FAILURE:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;
            case NodeStates.RUNNING:
                m_nodeState = NodeStates.RUNNING;
                return m_nodeState;
            //for default case, you can change this according to your needs.
            default:
                m_nodeState = NodeStates.FAILURE;
                return m_nodeState;

        }
    }
      

}
