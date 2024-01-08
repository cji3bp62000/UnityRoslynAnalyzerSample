
namespace UnityEngine
{
    public class Object
    {
        public void OnDestroy() {}
        public void Destroy(Object obj) {}
        public void DestroyImmediate() {}
    }

    public class GameObject : Object { }

    public class Component : Object { }

    public class Behaviour : Component
    {
        public void OnEnable() { }
        public void OnDisable() { }
    }
    public class MonoBehaviour : Behaviour { }

    public class Material : Object { }
}
