public delegate void StartGameDelegate();

interface IClock
{
    string Clock();
    System.Collections.IEnumerator GameCountDown();
    public event StartGameDelegate startGameDelegate;
}