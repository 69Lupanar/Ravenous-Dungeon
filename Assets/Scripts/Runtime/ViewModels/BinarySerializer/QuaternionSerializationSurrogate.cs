using System.Runtime.Serialization;
using UnityEngine;

namespace Assets.Scripts.Runtime.ViewModels.BinarySerializer
{
    /// <summary>
    /// Convertit une valeur de type Quaternion en un format sérialisable
    /// </summary>
    public class QuaternionSerializationSurrogate : ISerializationSurrogate
    {
        /// <summary>
        /// Convertit la donnée en valeur
        /// </summary>
        /// <param name="data">La donnée</param>
        /// <param name="info">Les infos sur la désérialisation de l'objet</param>
        public void GetObjectData(object data, SerializationInfo info, StreamingContext context)
        {
            Quaternion q = (Quaternion)data;
            info.AddValue("x", q.x);
            info.AddValue("y", q.y);
            info.AddValue("z", q.z);
            info.AddValue("w", q.w);
        }

        /// <summary>
        /// Convertit la valeur en données sérialisables
        /// </summary>
        /// <param name="info">Les infos sur la désérialisation de l'objet</param>
        /// <returns>La donnée à sérialiser</returns>
        public object SetObjectData(object data, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion q = (Quaternion)data;
            q.x = (float)info.GetValue("x", typeof(float));
            q.y = (float)info.GetValue("y", typeof(float));
            q.z = (float)info.GetValue("z", typeof(float));
            q.w = (float)info.GetValue("w", typeof(float));
            data = q;
            return data;
        }
    }
}