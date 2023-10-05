using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class representing a SuperTag that can be attached to GameObjects, in order to give them more than one tag.
/// It's Inspired by professor Diogo's Okapi Kit.
/// </summary>
public class SuperTag : MonoBehaviour
{
    /// <summary>List to store all instances of SuperTag in the scene.</summary>
    private static readonly List<SuperTag> AllSuperTags = new List<SuperTag>();
    
    /// <summary> The case-sensitive tag name associated with this SuperTag instance. </summary>
    public string TagName => _tagName;
    [Header("It's Case Sensitive!!!!")]
    [SerializeField] private string _tagName = "none";
    
    private void Awake()
    {
        // Checks if there is another SuperTag with the same tag name attached to this GameObject.
        // If not repeated, add this SuperTag to the list of all SuperTags.
        bool isRepeatedToThisGmObj = FindObjectsWithSuperTag(TagName).Any(gObj => gObj == this.gameObject);
        if (!isRepeatedToThisGmObj) AllSuperTags.Add(this);
        else Debug.LogWarning($"{gameObject.name} has tried to have more than 1 SuperTag named ({TagName}), this action was denied");
    }

    /// <summary>Finds all GameObjects with the specified case-sensitive SuperTag name.</summary>
    /// <param name="superTagNameCaseSensitive">The case-sensitive SuperTag name to search for.</param>
    /// <returns>An IEnumerable of GameObjects with the specified SuperTag name.</returns>
    public static IEnumerable<GameObject> FindObjectsWithSuperTag(string superTagNameCaseSensitive)
    {
        foreach (SuperTag superTag in AllSuperTags)
            if (superTag.TagName.ToUpper().Equals(superTagNameCaseSensitive.ToUpper()))
                yield return superTag.gameObject;
    }

    /// <summary>Gets the first GameObject with the specified case-sensitive SuperTag name.</summary>
    /// <param name="superTagNameCaseSensitive">The case-sensitive SuperTag name to search for.</param>
    /// <returns>The first GameObject with the specified SuperTag name, or null if not found.</returns>
    public static GameObject GetFirstObjectWithSuperTag(string superTagNameCaseSensitive)
    {
        foreach (SuperTag superTag in AllSuperTags)
            if (superTag.TagName.ToUpper().Equals(superTagNameCaseSensitive.ToUpper()))
                return superTag.gameObject;
        return null;
    }
}
