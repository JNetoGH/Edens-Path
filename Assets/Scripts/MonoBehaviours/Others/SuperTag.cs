using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A class representing a SuperTag that can be attached to GameObjects, in order to give it more than one tag.
/// It's inspired by professor Diogo's Okapi Kit.
/// </summary>
public class SuperTag : MonoBehaviour
{
    /// <summary>List to store all instances of SuperTags.</summary>
    private static List<SuperTag> _allSuperTags = new List<SuperTag>();
    public static string ListState
    {
        get
        {
            StringBuilder msg = new StringBuilder();
            msg.AppendLine("LIST CURRENT STATE:");
            foreach (SuperTag superTag in _allSuperTags)
                msg.AppendLine($"- {superTag.TagName} tag of {superTag.gameObject.name}");
            return msg.ToString();
        }
    }
    
    /// <summary>The case-insensitive tag name associated with this SuperTag instance.</summary>
    public string TagName => _tagName;
    [Header("It's Case-Insensitive!!!!")]
    [SerializeField] private string _tagName = "NONE";
    
    private void Awake()
    {
        // Checks if there is another SuperTag with the same tag name attached to this GameObject.
        // If not repeated, add this SuperTag to the list of all SuperTags.
        _tagName = _tagName.ToUpper();
        bool isRepeatedToThisGmObj = GetObjectsWithSuperTag(TagName).Any(gObj => gObj == this.gameObject);
        if (!isRepeatedToThisGmObj) _allSuperTags.Add(this);
        else Debug.LogWarning($"{gameObject.name} has tried to have more than 1 SuperTag named ({TagName}), this action was denied");
    }
    
    /// <summary>Finds all GameObjects with the specified case-insensitive SuperTag name.</summary>
    /// <param name="superTagNameCaseInsensitive">The case-insensitive SuperTag name to search for.</param>
    /// <returns>An IEnumerable of GameObjects with the specified SuperTag name.</returns>
    public static IEnumerable<GameObject> GetObjectsWithSuperTag(string superTagNameCaseInsensitive)
    {
        foreach (SuperTag superTag in _allSuperTags)
            if (superTag.TagName.ToUpper().Equals(superTagNameCaseInsensitive.ToUpper()))
                yield return superTag.gameObject;
    }

    /// <summary>Gets the first GameObject with the specified case-insensitive SuperTag name.</summary>
    /// <param name="superTagNameCaseInsensitive">The case-insensitive SuperTag name to search for.</param>
    /// <returns>The first GameObject with the specified SuperTag name, or null if not found.</returns>
    public static GameObject GetFirstObjectWithSuperTag(string superTagNameCaseInsensitive)
    {
        foreach (SuperTag superTag in _allSuperTags)
            if (superTag.TagName.ToUpper().Equals(superTagNameCaseInsensitive.ToUpper()))
                return superTag.gameObject;
        return null;
    }

    private void OnDestroy()
    {
        Debug.Log($"SuperTag {this.TagName} of {gameObject.name} WILL BE destroyed! \n{ListState}");
        _allSuperTags.Remove(this);
        Debug.Log($"SuperTag {this.TagName} of {gameObject.name} HAS BEEN destroyed! \n{ListState}");
    }
    
    public static void ResetControlListReference()
    {
        _allSuperTags = new List<SuperTag>();
    }
}

