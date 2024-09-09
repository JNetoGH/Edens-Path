using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.ModularAnimeCharacter.Customization
{
    public interface IAssetLoader
    {
        IEnumerator LoadAssetList(string path, System.Action<string[]> onLoaded);
        IEnumerator LoadAsset<T>(string path, System.Action<T> onLoaded);
    }
}