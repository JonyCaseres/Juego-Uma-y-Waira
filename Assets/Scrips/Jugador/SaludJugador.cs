using System;
using UnityEngine;

public class BarraCorazones : MonoBehaviour
{
    [SerializeField] private int maxLives = 5;

    public event Action<int,int> OnLivesChanged;

    private int cantidadActual;
    private SimpleDamageUI simpleUI;

    private void Awake()
    {
        simpleUI = FindObjectOfType<SimpleDamageUI>();
    }

    private void Start()
    {
        if (maxLives < 1) maxLives = 1;
        cantidadActual = maxLives;
        OnLivesChanged?.Invoke(cantidadActual, maxLives);
    }

    public void RecibirDaño(int amount = 1)
    {
        if (cantidadActual <= 0) return;
        cantidadActual = Mathf.Max(0, cantidadActual - amount);
        simpleUI?.OnDamage(amount);
        OnLivesChanged?.Invoke(cantidadActual, maxLives);
    }

    public void Recuperar(int amount = 1)
    {
        if (cantidadActual >= maxLives) return;
        cantidadActual = Mathf.Min(maxLives, cantidadActual + amount);
        simpleUI?.OnHeal(amount);
        OnLivesChanged?.Invoke(cantidadActual, maxLives);
    }

    public void ResetToFull()
    {
        cantidadActual = maxLives;
        simpleUI?.ResetAll();
        OnLivesChanged?.Invoke(cantidadActual, maxLives);
    }

    public int CurrentLives => cantidadActual;
    public int MaxLives => maxLives;
}