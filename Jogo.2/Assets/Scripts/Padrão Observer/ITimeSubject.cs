using System.Collections.Generic;

// Interface que define o "Subject" (Sujeito) no padrão Observer
// Responsável por gerenciar e notificar os observadores sobre mudanças no tempo
public interface ITimeSubject
{
    // Adiciona um observador à lista
    // Observadores implementam a interface ITimeObserver
    void AddObserver(ITimeObserver observer);

    // Remove um observador da lista
    void RemoveObserver(ITimeObserver observer);

    // Notifica todos os observadores quando o tempo restante muda
    // timeLeft representa o tempo atual em segundos
    void NotifyTimeChanged(float timeLeft);

    // Notifica todos os observadores quando o tempo se esgota
    void NotifyTimeEnded();
}