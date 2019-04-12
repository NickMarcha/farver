using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Disables camelcase warning
#pragma warning disable

/// <summary>
/// Extended vesion of UnityEngine.MonoBehaviour
/// </summary>
public abstract class ExtendedMonoBehaviour : UnityEngine.MonoBehaviour
{

    /// <summary>
    /// Gets a collection of all the components of this game object
    /// </summary>
    public IEnumerable<Component> AllComponents
    {
        get
        {
            return gameObject.GetComponents<Component>().AsEnumerable();
        }
    }

    /// <summary>
    /// Gets the amount of components this game object has 
    /// </summary>
    public int ComponentCount
    {
        get
        {
            return AllComponents.Count();
        }
    }

    protected virtual void Awake()
    {
        foreach (System.Reflection.FieldInfo i in GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
        {
            foreach (object attr in i.GetCustomAttributes(true))
            {
                if (attr is AutoGetComponentAttribute)
                {
                    try
                    {
                        i.SetValue(this, GetComponent((attr as AutoGetComponentAttribute).ComponentType));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("AutoGetComponent could not be evaluated: " + ex.Message);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds a component to this game object
    /// </summary>
    /// <typeparam name="T">Objec type to add</typeparam>
    /// <returns></returns>
    public T AddComponent<T>() where T : Component
    {
        return gameObject.AddComponent<T>();
    }

    public T AddOrGetComponent<T>() where T : Component
    {
        T c = GetComponent<T>();

        if (c != null)
        {
            return c;
        }

        return AddComponent<T>();
    }

    /// <summary>
    /// Deletes a component from this game object. Like using Destroy
    /// </summary>
    /// <typeparam name="T">Component to delete</typeparam>
    /// <returns></returns>
    public bool RemoveComponent<T>() where T : Component
    {
        if (HasComponent<T>())
        {
            Destroy(GetComponent<T>());

            return true;
        }
        return false;
    }

    /// <summary>
    /// Does this game object have a specific component?
    /// </summary>
    /// <typeparam name="T">The component to test for</typeparam>
    /// <returns></returns>
    public bool HasComponent<T>() where T : Component
    {
        return GetComponent<T>();
    }

    /// <summary>
    /// Gets the AudioSource component of this game object that has a specific audio clip
    /// </summary>
    /// <param name="clip">Clip in question</param>
    /// <returns></returns>
    public AudioSource GetAudioSource(AudioClip clip)
    {
        return gameObject.GetComponents<AudioSource>().Where(i => i.clip == clip).SingleOrDefault();
    }

    /// <summary>
    ///Gets the AudioSource component of this game object that has a specific audio clip
    /// </summary>
    /// <param name="clipName">Name of clip in question</param>
    /// <returns></returns>
    public AudioSource GetAudioSource(string clipName)
    {
        return gameObject.GetComponents<AudioSource>().Where(i => i.clip.name == clipName).SingleOrDefault();
    }

    /// <summary>
    /// Gets the Rigidbody of this game object. If it does not exist, null will be returned
    /// </summary>
    public new Rigidbody rigidbody
    {
        get
        {
            return GetComponent<Rigidbody>();
        }
    }

    /// <summary>
    /// Gets the Rigidbody2D of this game object. If it does not exist, null will be returned
    /// </summary>
    public new Rigidbody2D rigidbody2D
    {
        get
        {
            return GetComponent<Rigidbody2D>();
        }
    }


    /// <summary>
    /// Gets the first Collider of this game object. If it does not exist, null will be returned
    /// </summary>
    public new Collider collider
    {
        get
        {
            return GetComponent<Collider>();
        }
    }

    /// <summary>
    /// Gets the first BoxCollider of this game object. If it does not exist, null will be returned
    /// </summary>
    public BoxCollider boxCollider
    {
        get
        {
            return GetComponent<BoxCollider>();
        }
    }

    /// <summary>
    /// Gets the first SphereCollider of this game object. If it does not exist, null will be returned
    /// </summary>
    public SphereCollider sphereCollider
    {
        get
        {
            return GetComponent<SphereCollider>();
        }
    }

    /// <summary>
    /// Gets the first CapsuleCollider of this game object. If it does not exist, null will be returned
    /// </summary>
    public CapsuleCollider capsuleCollider
    {
        get
        {
            return GetComponent<CapsuleCollider>();
        }
    }

    /// <summary>
    /// Gets the first MeshCollider of this game object. If it does not exist, null will be returned
    /// </summary>
    public MeshCollider meshCollider
    {
        get
        {
            return GetComponent<MeshCollider>();
        }
    }

    /// <summary>
    /// Gets the first Collider2D of this game object. If it does not exist, null will be returned
    /// </summary>
    public new Collider2D collider2D
    {
        get
        {
            return GetComponent<Collider2D>();
        }
    }

    /// <summary>
    /// Gets the first BoxCollider2D of this game object. If it does not exist, null will be returned
    /// </summary>
    public BoxCollider2D boxCollider2D
    {
        get
        {
            return GetComponent<BoxCollider2D>();
        }
    }

    /// <summary>
    /// Gets the first CircleCollider2D of this game object. If it does not exist, null will be returned
    /// </summary>
    public CircleCollider2D circleCollider2D
    {
        get
        {
            return GetComponent<CircleCollider2D>();
        }
    }

    /// <summary>
    /// Gets the first PolygonCollider2D of this game object. If it does not exist, null will be returned
    /// </summary>
    public PolygonCollider2D polygonCollider2D
    {
        get
        {
            return GetComponent<PolygonCollider2D>();
        }
    }

    /// <summary>
    /// Gets the MeshFilter of this game object. If it does not exist, null will be returned
    /// </summary>
    public MeshFilter meshFilter
    {
        get
        {
            return GetComponent<MeshFilter>();
        }
    }

    /// <summary>
    /// Gets the mesh currently loaded by the MeshFilter. If no MeshFilter is active, null will be returned
    /// </summary>
    public Mesh ActiveMesh
    {
        get
        {
            if (meshRenderer != null)
            {
                return meshFilter.mesh;
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the sprite currently loaded by the SpriteRenderer of the object. if no SpriteRenderer is active. Null will be returned
    /// </summary>
    public Sprite ActiveSprite
    {
        get
        {
            if (spriteRenderer != null)
            {
                return spriteRenderer.sprite;
            }

            return null;
        }
    }

    /// <summary>
    /// Gets the MeshRenderer of this game object. If it does not exist, null will be returned
    /// </summary>
    public MeshRenderer meshRenderer
    {
        get
        {
            return GetComponent<MeshRenderer>();
        }
    }

    /// <summary>
    /// Gets the SpriteRenderer of this game object. If it does not exist, null will be returned
    /// </summary>
    public SpriteRenderer spriteRenderer
    {
        get
        {
            return GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// Gets the Camera of this game object. If it does not exist, null will be returned
    /// </summary>
    public new Camera camera
    {
        get
        {
            return GetComponent<Camera>();
        }
    }

    /// <summary>
    /// Gets the Light of this game object. If it does not exist, null will be returned
    /// </summary>
    public new Light light
    {
        get
        {
            return GetComponent<Light>();
        }
    }

    /// <summary>
    /// Gets the first GUIElement of this game object. If it does not exist, null will be returned
    /// </summary>
    public new GUIElement guiElement
    {
        get
        {
            return GetComponent<GUIElement>();
        }
    }

    /// <summary>
    /// Gets the HingeJoint of this game object. If it does not exist, null will be returned
    /// </summary>
    public new HingeJoint hingeJoint
    {
        get
        {
            return GetComponent<HingeJoint>();
        }
    }

    /// <summary>
    /// Gets the HingeJoint2D of this game object. If it does not exist, null will be returned
    /// </summary>
    public HingeJoint2D hingeJoint2D
    {
        get
        {
            return GetComponent<HingeJoint2D>();
        }
    }

    /// <summary>
    /// Gets the first ParticleSystem of this game object. If it does not exist, null will be returned
    /// </summary>
    public new ParticleSystem particleSystem
    {
        get
        {
            return GetComponent<ParticleSystem>();
        }
    }

    /// <summary>
    /// Gets the first AudioSource of this game object. If it does not exist, null will be returned
    /// </summary>
    public new AudioSource audio
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }

    /// <summary>
    /// Gets the first Renderer of this game object. If it does not exist, null will be returned.
    /// </summary>
    public new Renderer renderer
    {
        get
        {
            return GetComponent<Renderer>();
        }
    }

    /// <summary>
    /// Asserts an expression, throwing an exception if the expression is false
    /// </summary>
    /// <param name="expression">Expression to assert</param>
    //public static void Assert(bool expression)
    //{
    //    Assert(expression, "Assertion failed!");
    //}

    ///// <summary>
    ///// Asserts an expression, throwing an exception if the expression is false
    ///// </summary>
    ///// <param name="expression">Expression to assert</param>
    ///// <param name="errorMessage">Custom exception message</param>
    //public static void Assert(bool expression, string errorMessage)
    //{
    //    if (!expression)
    //    {
    //        throw new Exception(errorMessage);
    //    }
    //}
}

[AttributeUsage(AttributeTargets.Field)]
public class AutoGetComponentAttribute : Attribute
{
    public Type ComponentType;    
    public AutoGetComponentAttribute(string componentName)
    {
        ComponentType = AppDomain.CurrentDomain.GetAssemblies()
            .Select(i => i.GetType(componentName, false))
            .First(i => i != null);
    }

    public AutoGetComponentAttribute(Type component)
    {
        ComponentType = component;
    }
}