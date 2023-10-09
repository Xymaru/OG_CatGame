using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PawsAndClaws.Utils
{
    public static class BinaryUtils
    {
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter fmt = new BinaryFormatter();
            using(MemoryStream ms = new MemoryStream())
            {
                fmt.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T ByteArrayToObject<T>(byte[] bytes)
        {
            if (bytes == null)
                return default(T);

            BinaryFormatter fmt = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                object obj = fmt.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}