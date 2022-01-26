interface IClock
{
    void StartClock();
    System.Collections.IEnumerator CountDown();
    public bool startClock {get;set;}
}