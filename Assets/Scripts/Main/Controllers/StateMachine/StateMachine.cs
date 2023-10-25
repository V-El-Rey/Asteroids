public class StateMachine
{
    private StateChangeModel m_stateChangeModel;
    private GameStateIndex m_currentStateIndex;


    public GameStateIndex CurrentState
    {
        get { return m_currentStateIndex; }
        set 
        {
            m_stateChangeModel.onStateChanged?.Invoke();
            m_currentStateIndex = value; 
        }
    }

    public StateMachine(GameStateIndex initialState, StateChangeModel stateChangeModel)
    {
        m_currentStateIndex = initialState;
        m_stateChangeModel = stateChangeModel;
    }

    public void ChangeState(GameStateIndex newState)
    {
        CurrentState = newState;
    }

}
