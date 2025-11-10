using System.Collections.Generic;

public interface ITimeSubject
{
    void AddObserver(ITimeObserver observer);
    void RemoveObserver(ITimeObserver observer);
    void NotifyTimeChanged(float timeLeft);
    void NotifyTimeEnded();
}