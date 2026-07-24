using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Game.UI;

public class ResourceSquaresReader : MonoBehaviour
{
    [Header("Asignaciones")]
    [SerializeField] private ResourceSquares vidaSquares;
    [SerializeField] private MonoBehaviour vidaSource;

    [Header("Actualización")]
    [SerializeField] private float pollInterval = 0.15f;

    private Binder vidaBinder = new Binder();

    private void Awake()
    {
        if (vidaSquares == null) vidaSquares = FindObjectOfType<ResourceSquares>();
        if (vidaSource == null) vidaSource = FindObjectOfTypeByName("BarraCorazones") ?? FindObjectOfTypeByName("Vida");

        if (vidaSource != null)
        {
            vidaBinder.BindTo(vidaSource, new[] { "CurrentHealth", "currentHealth", "VidaActual", "vidaActual", "health", "vida" },
                                           new[] { "MaxHealth", "maxHealth", "VidaMaxima", "vidaMaxima", "max", "vidaMax" });
        }

        if (vidaSquares != null)
            StartCoroutine(PollCoroutine());
    }

    private IEnumerator PollCoroutine()
    {
        while (true)
        {
            UpdateVida();
            yield return new WaitForSeconds(pollInterval);
        }
    }

    private void UpdateVida()
    {
        if (vidaSquares == null) return;

        var values = vidaBinder.HasSource ? vidaBinder.GetValues() : (current: 0, max: 1);
        vidaSquares.SetValue(values.current, values.max);
    }

    private MonoBehaviour FindObjectOfTypeByName(string typeName)
    {
        if (string.IsNullOrEmpty(typeName)) return null;
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypesSafe())
            .FirstOrDefault(t => string.Equals(t.Name, typeName, StringComparison.Ordinal));
        if (type == null) return null;
        var obj = FindObjectOfType(type);
        return obj as MonoBehaviour;
    }

    private struct Binder
    {
        public object sourceInstance;
        private PropertyInfo propCurrent;
        private PropertyInfo propMax;
        private FieldInfo fieldCurrent;
        private FieldInfo fieldMax;
        private MethodInfo methodGetCurrent;
        private MethodInfo methodGetMax;

        public bool HasSource => sourceInstance != null;

        public void BindTo(MonoBehaviour source, string[] currentNames, string[] maxNames)
        {
            sourceInstance = source;
            if (sourceInstance == null) return;
            var t = sourceInstance.GetType();

            foreach (var n in currentNames)
            {
                propCurrent = propCurrent ?? t.GetProperty(n);
                fieldCurrent = fieldCurrent ?? t.GetField(n);
                methodGetCurrent = methodGetCurrent ?? t.GetMethod("Get" + n) ?? t.GetMethod("Get" + UpperFirst(n));
            }

            foreach (var n in maxNames)
            {
                propMax = propMax ?? t.GetProperty(n);
                fieldMax = fieldMax ?? t.GetField(n);
                methodGetMax = methodGetMax ?? t.GetMethod("Get" + n) ?? t.GetMethod("Get" + UpperFirst(n));
            }
        }

        public (int current, int max) GetValues()
        {
            int current = 0;
            int max = 1;
            if (sourceInstance == null) return (current, max);

            try
            {
                if (propCurrent != null) current = Convert.ToInt32(propCurrent.GetValue(sourceInstance));
                else if (fieldCurrent != null) current = Convert.ToInt32(fieldCurrent.GetValue(sourceInstance));
                else if (methodGetCurrent != null) current = Convert.ToInt32(methodGetCurrent.Invoke(sourceInstance, null));

                if (propMax != null) max = Convert.ToInt32(propMax.GetValue(sourceInstance));
                else if (fieldMax != null) max = Convert.ToInt32(fieldMax.GetValue(sourceInstance));
                else if (methodGetMax != null) max = Convert.ToInt32(methodGetMax.Invoke(sourceInstance, null));
            }
            catch { }

            current = Mathf.Clamp(current, 0, Mathf.Max(1, max));
            return (current, Mathf.Max(1, max));
        }

        private static string UpperFirst(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            if (s.Length == 1) return s.ToUpperInvariant();
            return char.ToUpperInvariant(s[0]) + s.Substring(1);
        }
    }
}

static class TypeExtensions
{
    public static System.Collections.Generic.IEnumerable<Type> GetTypesSafe(this Assembly asm)
    {
        try { return asm.GetTypes(); }
        catch { return new Type[0]; }
    }
}