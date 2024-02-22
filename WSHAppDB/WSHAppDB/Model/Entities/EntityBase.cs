using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WSHAppDB.Model.Entities
{
    public abstract class EntityBase
    {
        public int GetKey()
        {
            int keyValue = 0;
            var entityType = this.GetType();
            foreach (var propInfo in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var key = propInfo.GetCustomAttribute(typeof(KeyAttribute));
                if (key is KeyAttribute keyAttr)
                {
                    object keyPropValue = propInfo.GetValue(this, null);
                    keyValue = (int)keyPropValue;
                }
            }
            if (keyValue != 0)
            {
                return keyValue;
            }
            else
            {
                throw new Exception("Entity has no property with key attribute!");
            }

        }
    }
}