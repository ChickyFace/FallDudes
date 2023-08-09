using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    public ObjectPool Parent;
    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
    // add a reference the public object pool parent that automtically gets assigned by the objectpool whenever its creating the pool
    // whenever we disable we just return this object to the pool 

}