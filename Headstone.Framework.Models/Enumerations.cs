using Headstone.Framework.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headstone.Framework.Models
{
    // ENTITY
    [Serializable]
    public enum EntityStatus : short
    {
        [Display(Name = "Enumeration_EntityStatus_Unknown", ResourceType = typeof(FrameworkModels))]
        Unknown = -999,

        [Display(Name = "Enumeration_EntityStatus_Deleted", ResourceType = typeof(FrameworkModels))]
        Deleted = -99,

        [Display(Name = "Enumeration_EntityStatus_Blocked", ResourceType = typeof(FrameworkModels))]
        Blocked = -12,

        [Display(Name = "Enumeration_EntityStatus_Freezed", ResourceType = typeof(FrameworkModels))]
        Freezed = -11,

        [Display(Name = "Enumeration_EntityStatus_Test", ResourceType = typeof(FrameworkModels))]
        Test = -9,

        [Display(Name = "Enumeration_EntityStatus_Passive", ResourceType = typeof(FrameworkModels))]
        Passive = -1,

        [Display(Name = "Enumeration_EntityStatus_Draft", ResourceType = typeof(FrameworkModels))]
        Draft = 0,

        [Display(Name = "Enumeration_EntityStatus_Active", ResourceType = typeof(FrameworkModels))]
        Active = 1
    }

    // IDENTITY
    public enum AccountStatus
    {
        [Display(Name = "AccountStatus_Deleted", ResourceType = typeof(FrameworkModels))]
        Deleted = -99,

        [Display(Name = "AccountStatus_Freezed", ResourceType = typeof(FrameworkModels))]
        Freezed = -1,

        [Display(Name = "AccountStatus_Passive", ResourceType = typeof(FrameworkModels))]
        Passive = 0,

        [Display(Name = "AccountStatus_Active", ResourceType = typeof(FrameworkModels))]
        Active = 1,

        [Display(Name = "AccountStatus_Verified", ResourceType = typeof(FrameworkModels))]
        Verified = 2
    }

    // CONFIGURATION
    [Serializable]
    public enum ConfigurationEventTypes
    {
        Loaded,

        Refreshed,

        Changed
    }

    // LOGGING
    [Serializable]
    public enum LogChannel
    {
        DB,
        FS,
        LOG4NET,
        COUCHBASE,
        MONGO,
        ELASTICSEARCH
    }

    [Serializable]
    public enum LogMode
    {
        All,

        Debug,

        Info,

        Warn,

        Error,

        Fatal,

        None
    }

    // CHANNELS
    [Serializable]
    public enum ChannelType
    {
        Charging
    }

    // SECURITY
    [Serializable]
    public enum SecurityModel
    {
        Key,

        Key_Secret,

        Username_Password,

        Key_Username_Password,

        All
    }

    public enum EncryptorType
    {
        BasicEncryption,

        MD5Encryption,

        RSAEncryption,

        EkinEncryption,

        HMACMDMD5Encryption,

        RijndaelEncryption
    }

    // COMMUNICATION
    [Serializable]
    public enum CommunicationType
    {
        Sync,

        Async
    }

    [Serializable]
    public enum ResponseStatus : short
    {
        [Display(Name = "Enumeration_ResponseStatus_Unknown", ResourceType = typeof(FrameworkModels))]
        Unknown = 0,

        [Display(Name = "Enumeration_ResponseStatus_Error", ResourceType = typeof(FrameworkModels))]
        Error = 1,

        [Display(Name = "Enumeration_ResponseStatus_Declined", ResourceType = typeof(FrameworkModels))]
        Declined = 2,

        [Display(Name = "Enumeration_ResponseStatus_Approved", ResourceType = typeof(FrameworkModels))]
        Approved = 3,

        [Display(Name = "Enumeration_ResponseStatus_Pending", ResourceType = typeof(FrameworkModels))]
        Pending = 4,

        [Display(Name = "Enumeration_ResponseStatus_Timeout", ResourceType = typeof(FrameworkModels))]
        Timeout = 5
    }

    [Serializable]
    public enum ServiceResponseTypes
    {
        Error = -99,

        Declined = -1,

        Unknown = 0,

        Success = 1
    }

    [Serializable]
    public enum ServiceResponseSources
    {
        Undefined = 0,

        MsSQL = 1,

        Index = 2,

        Cache = 3,

        NoSQL = 4,

        ThirdParty = 5,

        FileSystem = 6
    }
}
