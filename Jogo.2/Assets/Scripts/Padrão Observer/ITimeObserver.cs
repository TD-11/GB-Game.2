// Padrão Observer
public interface ITimeObserver
{
    void OnTimeChanged(float timeLeft);
    void OnTimeEnded();
}