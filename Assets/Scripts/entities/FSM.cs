using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{

    public State currentState;


    private System.Action OnEnterState, OnUpdateState, OnExitState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = new IdleState();
        currentState.OnUpdateState();
        SwitchState(OnEnterIdleState, OnUpdateIdleState, OnExitIdleState);
    }

    // Update is called once per frame
    void Update()
    {
        if (OnUpdateState != null) {
            OnUpdateState.Invoke();
        }
        
    }

    private void SwitchState(
        System.Action OnEnterStateFunction,
        System.Action OnUpdateStateFunction,
        System.Action OnExitStateFunction) {

        if(OnExitState != null) {
            OnExitState.Invoke();
        }
        OnExitState = OnExitStateFunction;
        OnUpdateState = OnUpdateStateFunction;
        OnEnterState = OnEnterStateFunction;

        if(OnEnterState != null) {
            OnEnterState.Invoke();
        }
    }

    //Idle
    private void OnEnterIdleState() { }
    private void OnUpdateIdleState() {

        //SwitchState(OnEnterAttackState, OnUpdateState, OnExitState);
    }
    private void OnExitIdleState() { }
}


public interface IState {
    void OnEnterState();
    void OnUpdateState();
    void OnExitState();

}


public abstract class State : MonoBehaviour, IState {

    public abstract void OnEnterState();
    public abstract void OnExitState();
    public abstract void OnUpdateState();
    
}

public class IdleState : State {
    public override void OnEnterState() {
        throw new System.NotImplementedException();
    }

    public override void OnExitState() {
        throw new System.NotImplementedException();
    }

    public override void OnUpdateState() {
        throw new System.NotImplementedException();
    }
}