public class MyTimer
{
    public float TimeRemaining { get; private set; }
    public bool Done = false;
    public void Tick(float deltaTime)
    {
        TimeRemaining -= deltaTime;
        if (TimeRemaining <= 0)
        {
            Done = true;
        }
    }

    public void Reset(float deltaTime)
    {
        Done = false;
        TimeRemaining = deltaTime;
    }
}