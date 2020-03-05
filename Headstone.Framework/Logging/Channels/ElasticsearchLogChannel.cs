using Elasticsearch.Net;
using Headstone.Framework.Models.Configuration;
using Headstone.Framework.Models.Logging;
using Headstone.Framework.Models.Logging.Elasticsearch;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Headstone.Framework.Logging.Channels
{
    public class ElasticsearchLogChannel : ILogChannel
    {
        private string _indexNodes = LoggingConfig.LogDirectory;

        private string _indexPrefix = LoggingConfig.LogBucket ?? string.Empty;

        private string _indexUsername = "";

        private string _indexPassword = "";

        private static ElasticClient _client = null;

        public ElasticsearchLogChannel()
        {
            if (_client == null)
            {
                // Get elastic pool
                var pool = GetElasticPool();

                // Get elastic settins
                var settings = GetConnectionSettings(pool, _indexPrefix);

                // Get the elastic client
                _client = new ElasticClient(settings);
            }
        }

        public void Log(LogRecord record)
        {
            // Convert to es log record
            var logRecord = new ESLogRecord
            {
                LogID = record.LogID,
                Namespace = record.Namespace,
                UniqueKey = record.UniqueKey,
                AppKey = record.AppKey,
                Environment = record.Environment,
                Host = record.Host,
                HostIP = record.HostIP,
                Token = record.UserToken,
                UserId = record.UserId,
                UserIP = record.UserIP,
                Username = record.Username,
                IdentityKey = record.IdentityKey,
                Level = record.Level,
                Process = record.Process,
                ThreadId = record.ThreadId,
                Message = record.Message,
                Exception = record.ExceptionString,
                Data = record.DataString,
                TimeStamp = record.TimeStamp
            };

            // Get the index name
            string indexName = GetIndexName(typeof(ESLogRecord));
           
            try
            {
                // Create index if necessary
                ICreateIndexResponse createIndexResponse = null;
                if (!_client.IndexExists(indexName).Exists)
                {
                    // Create the index descriptor
                    var descriptor = GetCreateIndexDescriptor(indexName, typeof(ESLogRecord));

                    createIndexResponse = _client.CreateIndex(descriptor);
                }

                if (createIndexResponse == null || !createIndexResponse.IsValid)
                {
                    // Error logging
                }

                // Index the incoming data
                var indexDataResponse = _client.Index<ESLogRecord>(logRecord, idx => idx.Index(indexName));

            }
            catch (Exception ex)
            {
                
            }
        }

        #region [ Helper functions ]

        public Dictionary<string, CustomAnalyzer> CustomAnalyzers { get; } = new Dictionary<string, CustomAnalyzer>();

        protected StaticConnectionPool GetElasticPool()
        {
            // Get the elastic nodes
            var nodeUris = _indexNodes.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var nodes = nodeUris.Select(u => new Uri(u)).ToList();

            // Create the elastic pool
            var pool = new StaticConnectionPool(nodes);

            return pool;
        }

        protected ConnectionSettings GetConnectionSettings(StaticConnectionPool pool, string indexPrefix = "")
        {
            if (!String.IsNullOrEmpty(indexPrefix))
            {
                _indexPrefix = indexPrefix;
            }

            var settings = new ConnectionSettings(pool).PrettyJson(true);

            if (!String.IsNullOrEmpty(_indexUsername) && !String.IsNullOrEmpty(_indexPassword))
            {
                settings.BasicAuthentication(_indexUsername, _indexPassword);
            }

            return settings;
        }

        protected string GetIndexName(Type t)
        {
            // Create index name
            string indexName = _indexPrefix;
            if (indexName.Length > 0) indexName += ".";
            if (t == typeof(ESLogRecord)) indexName += "logs";

            // Return it
            return indexName;
        }

        private CreateIndexDescriptor GetCreateIndexDescriptor(string indexName, Type objectType)
        {
            #region [ Default analyzers and filters ]

            // Add custom index analyzers
            CustomAnalyzers.Add("full_string_index_analyzer", new CustomAnalyzer { Tokenizer = "standard", Filter = new List<string> { "standard", "string_delimeter", "stop", "asciifolding", "string_ngrams", "lowercase" } });
            CustomAnalyzers.Add("full_keyword_index_analyzer", new CustomAnalyzer { Tokenizer = "keyword", Filter = new List<string> { "standard", "stop", "asciifolding" } });

            // Add custom search analyzers
            CustomAnalyzers.Add("full_string_search_analyzer", new CustomAnalyzer { Tokenizer = "standard", Filter = new List<string> { "standard", "stop", "asciifolding", "lowercase" } });

            #endregion

            // Create a default descriptor
            CreateIndexDescriptor descriptor = null;

            // Create default settings
            var settings = new IndexSettings() { NumberOfReplicas = 1, NumberOfShards = 2 };

            // Add additional settings
            settings.Analysis = new Analysis();
            settings.Analysis.TokenFilters = new TokenFilters();
            settings.Analysis.Analyzers = new Analyzers();
            //settings.Add("index.mapping.single_type", false);
            settings.Add("index.mapping.total_fields.limit", 2000);
            settings.Add("index.mapping.nested_fields.limit", 500);
            settings.Add("index.max_docvalue_fields_search", 500);

            // Create token filters
            var stringNGramsTokenFilter = new EdgeNGramTokenFilter { MinGram = 2, MaxGram = 20 };
            var stringDelimiterTokenFilter = new WordDelimiterTokenFilter { GenerateWordParts = true, CatenateAll = true, CatenateNumbers = true, CatenateWords = true, SplitOnCaseChange = true, SplitOnNumerics = true, PreserveOriginal = true };

            // Add filters
            settings.Analysis.TokenFilters.Add("string_ngrams", stringNGramsTokenFilter);
            settings.Analysis.TokenFilters.Add("string_delimeter", stringDelimiterTokenFilter);

            // Add analyzers
            CustomAnalyzers.ToList().ForEach(a =>
            {
                settings.Analysis.Analyzers.Add(a.Key, a.Value);
            });

            // Create the config
            var indexConfig = new IndexState
            {
                Settings = settings
            };

            #region [ LogRecord Mapping ]

            // Fill the descriptor according to the type
            if (objectType == typeof(ESLogRecord))
            {
                descriptor = new CreateIndexDescriptor(indexName)
                                    .InitializeUsing(indexConfig)
                                    .Mappings(ms => ms.Map<ESLogRecord>(m => m.AutoMap()));
            }

            #endregion

            return descriptor;
        }

        #endregion
    }
}
