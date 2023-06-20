using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using static Read.Data.Helper.Constants;

namespace Read.Data.Models
{
    [Serializable]
    public class ApiResponse
    {
        public ApiResponse() { }

        public ApiResponse(object data)
        {
            Data = data;
            Success = true;
            Error = null;
        }

        public ApiResponse(ApiError error)
        {
            Error = error;
            Success = false;
            Data = null;
        }

        public ApiResponse(object data, bool success, ApiError error)
        {
            Data = data;
            Success = success;
            Error = error;
        }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }
        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        /// <summary>
        /// Custom Error if response is not success
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public ApiError Error { get; set; }
    }

    [Serializable]
    public class ApiError
    {
        public ApiError() { }
        public ApiError(string errorMessage, string errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }
        [JsonProperty(PropertyName = "errorCode")]
        public string ErrorCode { get; set; }
    }

    /// <summary>
    /// API Response Object With Type
    /// </summary>
    [Serializable]
    public class ApiResponse<T>
    {
        public ApiResponse() { }

        public ApiResponse(T data)
        {
            Data = data;
            Success = true;
            Error = null;
        }

        public ApiResponse(ApiError error)
        {
            Error = error;
            Success = false;
            Data = default(T);
        }

        public ApiResponse(T data, bool success, ApiError error)
        {
            Data = data;
            Success = success;
            Error = error;
        }

        /// <summary>
        /// Specifies Response Success or Failure
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// Generic Data Object
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        /// <summary>
        /// Custom Error if response is not success
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public ApiError Error { get; set; }
    }

    /// <summary>
    /// Version object of the class
    /// </summary>
    [Serializable]
    [KnownType(typeof(ApiVersion))]
    public class ApiVersion
    {
        /// <summary>
        /// Latest Version
        /// </summary>
        [JsonProperty(PropertyName = "latest")]
        public string Latest { get; set; }

        /// <summary>
        /// All Versions
        /// </summary>
        [JsonProperty(PropertyName = "versions")]
        public string[] Versions { get; set; }
    }

    public class ConfigClass
    {
        public string DatabaseHost { get; set; }
        public string ThirdPartyAPIBaseURL { get; set; }
    }

}
