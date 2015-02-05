// XmlManager.cs
// James Tyson
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace BladesLib.Util
{
    /// <summary>
    /// XmlManager provides xml serialization and deserialization functionality
    /// </summary>
    /// <typeparam name="T">Type of object being serialize/deserialized</typeparam>
    public class XmlManager<T>
    {
        public Type Type;

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlManager()
        {
            Type = typeof(T);
        }

        /// <summary>
        /// Loads an object from a given xml path
        /// </summary>
        /// <param name="path">Path to load from</param>
        /// <returns>Object of Type T created</returns>
        public T Load(string path)
        {
            T instance;
            using (TextReader reader = new StreamReader(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        /// <summary>
        /// Saves an object to a given xml path
        /// </summary>
        /// <param name="path">Path to save in</param>
        /// <param name="obj">Object to serialze</param>
        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
