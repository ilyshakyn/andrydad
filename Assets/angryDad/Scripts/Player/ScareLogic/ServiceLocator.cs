using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
    private static Dictionary<System.Type, object> services = new();

    public static void Register<T>(T service) where T : class
    {
        services[typeof(T)] = service;
    }

    public static T Resolve<T>() where T : class
    {
        if (services.TryGetValue(typeof(T), out var service))
            return service as T;

        Debug.LogError($"Service of type {typeof(T)} not found");
        return null;
    }
}
