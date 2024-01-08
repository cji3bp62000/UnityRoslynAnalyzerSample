namespace UnityEngine;

public class Object
{
    public void Destroy(Object obj) { }
}
public class Material : Object { }

public class Component : Object { }
public class Behaviour : Component { }
public class MonoBehaviour : Behaviour { }

public class GameObject : Object { }
