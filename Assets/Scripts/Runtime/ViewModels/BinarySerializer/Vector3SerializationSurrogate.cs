using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts.Runtime.ViewModels.BinarySerializer
{
    /// <summary>
    /// Convertit une valeur de type Vector3 en un format sérialisable
    /// </summary>
    public class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        /// <summary>
        /// Convertit la donnée en valeur
        /// </summary>
        /// <param name="data">La donnée</param>
        /// <param name="info">Les infos sur la désérialisation de l'objet</param>
        public void GetObjectData(object data, SerializationInfo info, StreamingContext context)
        {
            Vector3 v3 = (Vector3)data;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
        }

        /// <summary>
        /// Convertit la valeur en données sérialisables
        /// </summary>
        /// <param name="info">Les infos sur la désérialisation de l'objet</param>
        /// <returns>La donnée à sérialiser</returns>
        public object SetObjectData(object data, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 v3 = (Vector3)data;
            v3.x = (float)info.GetValue("x", typeof(float));
            v3.y = (float)info.GetValue("y", typeof(float));
            v3.z = (float)info.GetValue("z", typeof(float));
            data = v3;
            return data;
        }
    }
}