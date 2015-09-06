using System;

namespace WarehouseEntry.Business.Models
{
    [Serializable]
    public class JsonModel
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
        public object ObjectModel { get; set; }

        private JsonModel()
        {

        }

        public static JsonModel Success()
        {
            return Success(string.Empty);
        }

        public static JsonModel Success(string msg)
        {
            return new JsonModel
            {
                Status = true,
                Msg = msg
            };
        }

        public static JsonModel Fail(string msg, object obj = null)
        {
            return new JsonModel
            {
                Status = false,
                Msg = msg,
                ObjectModel = obj
            };
        }
    }
}