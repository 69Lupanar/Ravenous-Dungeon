using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts.Runtime.ViewModels.BinarySerializer
{
    /// <summary>
    /// Gère la (dé)sérialisation d'un objet sous forme de fichier binaire
    /// </summary>
    public static class BinarySerializer
    {
        /// <summary>
        /// Obtient les noms de tous les fichiers renseignés au chemin d'accès spécifié
        /// </summary>
        /// <param name="folderPath">Le chemin d'accès</param>
        /// <param name="fileExtension">L'extension du fichier. Si laissé vide, récupère tous les fichiers du répertoire.</param>
        /// <returns>La liste des noms de chaque fichier au chemin spécifié</returns>
        public static string[] GetAllSaveFileNames(string folderPath, string fileExtension = null)
        {
            if (Directory.Exists(folderPath))
            {
                string[] savedFiles;

                if (string.IsNullOrEmpty(fileExtension))
                {
                    savedFiles = Directory.GetFiles(folderPath);
                }
                else
                {
                    savedFiles = Directory.GetFiles(folderPath, $"*{fileExtension}");
                }

                return savedFiles;
            }
            else
            {
                throw new DirectoryNotFoundException($"Erreur : Le dossider de sauvegarde référencé n'existe pas : \"{folderPath}\".");
            }
        }

        /// <summary>
        /// Supprime tous les fichiers de l'extension renseignée au chemin d'accès renseigné
        /// </summary>
        /// <param name="folderPath">Le chemin d'accès</param>
        /// <param name="fileExtension">L'extension du fichier. Si laissé vide, efface tous les fichiers du répertoire.</param>
        public static void DeleteAllSaveFiles(string folderPath, string fileExtension = null)
        {
            string[] savedFiles;

            if (Directory.Exists(folderPath))
            {
                if (string.IsNullOrEmpty(fileExtension))
                {
                    savedFiles = Directory.GetFiles(folderPath);
                }
                else
                {
                    savedFiles = Directory.GetFiles(folderPath, $"*{fileExtension}");
                }
            }
            else
            {
                throw new DirectoryNotFoundException($"Erreur : Le dossider de sauvegarde référencé n'existe pas : \"{folderPath}\".");
            }

            for (int i = 0; i < savedFiles.Length; ++i)
            {
                File.Delete(savedFiles[i]);
            }

            Debug.Log($"Tous les fichiers ont bien été supprimés à 'emplacement \"{folderPath}\".");
        }

        /// <summary>
        /// Supprime le fichier de l'extension renseignée au chemin d'accès renseigné
        /// </summary>
        /// <param name="fileName">Le nom du fichier</param>
        /// <param name="folderPath">Le chemin d'accès</param>
        /// <param name="fileExtension">L'extension du fichier. Si laissé vide, efface tous les fichiers du répertoire.</param>
        public static void DeleteSaveFileAtPath(string fileName, string folderPath, string fileExtension)
        {
            string path = string.Format("{0}/{1}.{2}", folderPath, fileName, fileExtension.Replace(".", string.Empty));

            if (File.Exists(path))
            {
                if (string.IsNullOrEmpty(fileExtension))
                {
                    throw new FileNotFoundException($"Erreur : L'extension du fichier à supprimer n'a pas été référencé.");
                }
                else
                {
                    File.Delete(path);
                    Debug.Log($"Fichié supprimé à l'emplacement \"{path}\".");
                }
            }
            else
            {
                throw new FileNotFoundException($"Erreur : Le fichier renseigné est introuvable à l'emplacement \"{folderPath}\".");
            }
        }

        /// <summary>
        /// Sauvegarde les données renseignées dans un fichier
        /// </summary>
        /// <param name="fileName">Le nom du fichier</param>
        /// <param name="folderPath">Le chemin d'accès</param>
        /// <param name="fileExtension">L'extension du fichier. Si laissé vide, efface tous les fichiers du répertoire.</param>
        /// <param name="data">Les données à sauvegarder</param>
        /// <param name="formatter">Le BinaryFormatter gérant la sauvegarde</param>
        /// <returns>true si les données ont été sauvegardées avec succès</returns>
        public static void Save(string fileName, string folderPath, string fileExtension, object data, BinaryFormatter formatter)
        {
            // Dans le cas où l'on a pas les droits de création de dossier,
            // on arrête l'opération

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string path = string.Format("{0}/{1}.{2}", folderPath, fileName, fileExtension.Replace(".", string.Empty));
                FileStream file = File.Create(path);

                formatter.Serialize(file, data);

                Debug.Log($"Fichier enregistré à l'emplacement \"{path}\".");
                file.Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        /// <summary>
        /// Obtient les données au chemin d'accès renseigné
        /// </summary>
        /// <typeparam name="T">Le type de la donnée</typeparam>
        /// <param name="path">Le chemin d'accès</param>
        /// <param name="formatter">Le BinaryFormatter gérant la sauvegarde</param>
        /// <returns>Les données converties au type renseigné</returns>
        public static T Load<T>(string path, BinaryFormatter formatter)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Erreur : Le fichier demandé n'existe pas à l'emplacement \"{path}\".");
            }

            try
            {
                FileStream file = File.OpenRead(path);
                T dataToLoad = default;

                dataToLoad = (T)formatter.Deserialize(file);

                file.Close();

                return dataToLoad;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw e;
            }
        }

        /// <summary>
        /// Instancie un BinaryFormatter
        /// ou obtient celui déjà créé s'il existe
        /// </summary>
        /// <returns>Le BinaryFormatter</returns>
        public static BinaryFormatter CreateBinaryFormatter()
        {
            return new BinaryFormatter() { SurrogateSelector = GetSurrogateSelector() };
        }

        /// <summary>
        /// Crée des substituts pour les types non pris en charge par le BinaryFormatter
        /// </summary>
        /// <returns>Le SurrogateSelector</returns>
        private static SurrogateSelector GetSurrogateSelector()
        {
            SurrogateSelector selector = new();

            selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), new Vector3SerializationSurrogate());
            selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), new QuaternionSerializationSurrogate());

            return selector;

        }
    }
}