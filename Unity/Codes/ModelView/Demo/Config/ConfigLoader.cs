using System;
using System.Collections.Generic;
using ET.Module;
using UnityEngine;

namespace ET
{
       public class ConfigLoader: IConfigLoader
    {
        public void GetAllConfigBytes(Dictionary<string, byte[]> output)
        {
            List<Type> configTypes = Game.EventSystem.GetTypes(typeof (ConfigAttribute));
            foreach (Type configType in configTypes)
            {
                var v = AddressablesCache.LoadAsset<TextAsset>(configType.Name);
                output[configType.Name] = v.bytes;
            }
            
            // Dictionary<string, UnityEngine.Object> keys = ResourcesComponent.Instance.GetBundleAll("config.unity3d");
            //
            // foreach (var kv in keys)
            // {
            //     TextAsset v = kv.Value as TextAsset;
            //     string key = kv.Key;
            //     output[key] = v.bytes;
            // }
        }

        public byte[] GetOneConfigBytes(string configName)
        {
            //TextAsset v = ResourcesComponent.Instance.GetAsset("config.unity3d", configName) as TextAsset;
            var v = AddressablesCache.LoadAsset<TextAsset>(configName);
            return v.bytes;
        }
    }
}