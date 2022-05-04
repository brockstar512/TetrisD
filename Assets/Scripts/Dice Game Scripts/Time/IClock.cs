public delegate void StartGameDelegate();

public interface IClock
{
    public void StartGame();
    string Clock();
    System.Collections.IEnumerator GameCountDown();
    public event StartGameDelegate startGameDelegate;
}