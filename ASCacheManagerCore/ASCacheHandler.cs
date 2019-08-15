using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using ASLogger;
using System.Runtime.InteropServices;

namespace ASCacheManager
{
    /// <summary>
    /// Alternative soft Cache manager
    /// </summary>
    public class ASCacheProvider : IASCacheProvider
    {
        private readonly IAslogger _logger;
        //
        // Summary:
        //     Sets an item into the cache at the cache key specified regardless if it already
        //     exists or not.

        //

        private readonly RedisManagerPool redismanager;
        /// <summary>
        /// Redise Server URL
        /// and this will return an instance on cachemanager
        /// Register this Object as Sigleton Object to maintain Persistent connection
        /// </summary>
        /// <param name="host"></param>
        public ASCacheProvider(string host, [Optional] IAslogger logger)
        {
            redismanager = new RedisManagerPool(host);
            if (logger != null)
                _logger = logger;
            else
                _logger = new ASLogProvider("");
        }

        /// <summary>
        /// Save All the items with respect to its key at one shot
        /// Key should to string and unique
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyValuePair"></param>
        public void SetAll<T>(IDictionary<string, T> values)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    redisClient.SetAll(values);
                }
            }
            catch (Exception r)
            {
                _logger.Error(r, "Error on Set Items");
                throw;
            }

        }



        /// <summary>
        /// Get particular item from the cache using key If exixts
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {

            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    return redisClient.Get<T>(key);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Get");
                throw;
            }

        }



        /// <summary>
        /// Save Single item tnto the cache server respect to the KEY
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Set<T>(string key, T value)
        {

            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    if (redisClient.Get<string>(key) == null)
                        return redisClient.Set(key, value);
                    else
                        throw new Exception("100 - Same Key is Already Exists");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error On Set");
                throw;
            }

        }

        /// <summary>
        /// Save Single item tnto the cache server respect to the KEY with Expiry Time Period
        /// in Milliseconds
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeout"></param>
        public bool Set<T>(string key, T value, TimeSpan timeout)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    if (redisClient.Get<string>(key) == null)
                        return redisClient.Set(key, value, timeout);
                    else
                        throw new Exception("100 - Same Key is Already Exists");
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Set");
                throw;
            }

        }

        /// <summary>
        /// Remove a item from the List besed on the key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            bool removed = false;
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    removed = redisClient.Remove(key);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Remove");
                throw;
            }


            return removed;
        }
        /// <summary>
        /// Check Wthether the items is in Cache or not based on key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsInCache(string key)
        {
            bool isInCache = false;
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    isInCache = redisClient.ContainsKey(key);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Iscache Checking");
                throw;
            }

            return isInCache;
        }



        /// <summary>
        /// Flush All the Data from Database
        /// </summary>
        public void FlushAll()
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    redisClient.FlushAll();
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Flush");
                throw;
            }

        }
        /// <summary>
        /// Get all the elements with key using Key list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public IDictionary<string, T> GetAll<T>(IEnumerable<string> keys)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    return redisClient.GetAll<T>(keys);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on GetALl ");
                throw;
            }

        }


        /// <summary>
        /// Remove All the Multiple items from the cache based onthe list of keys
        /// </summary>
        /// <param name="keys"></param>
        public void RemoveAll(IEnumerable<string> keys)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    redisClient.RemoveAll(keys);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Remove ALl");
                throw;
            }

        }

        /// <summary>
        /// Replace the value of particular item using key set Expiry Date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, DateTime expiresAt)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    return redisClient.Replace(key, value, expiresAt);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Replace");
                throw;
            }


        }
        /// <summary>
        /// Replace value and set Expiry Time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresIn"></param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    return redisClient.Replace(key, value, expiresIn);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on REplace");
                throw;
            }

        }
        /// <summary>
        /// Only REplace Element using Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Replace<T>(string key, T value)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    return redisClient.Replace(key, value);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Replace");
                throw;
            }

        }

        /// <summary>
        /// Set element with Expiry Date
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiresAt"></param>
        /// <returns></returns>
        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            try
            {
                using (IRedisClient redisClient = redismanager.GetClient())
                {
                    return redisClient.Set(key, value, expiresAt);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error on Set");
                throw;
            }

        }


    }


    public interface IASCacheProvider
    {

        // Summary:
        //     Invalidates all data on the cache.
        void FlushAll();
        //
        // Summary:
        //     Retrieves the specified item from the cache.
        //
        // Parameters:
        //   key:
        //     The identifier for the item to retrieve.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     The retrieved item, or null if the key was not found.
        T Get<T>(string key);
        //
        // Summary:
        //     Retrieves multiple items from the cache. The default value of T is set for all
        //     keys that do not exist.
        //
        // Parameters:
        //   keys:
        //     The list of identifiers for the items to retrieve.
        //
        // Returns:
        //     a Dictionary holding all items indexed by their key.
        IDictionary<string, T> GetAll<T>(IEnumerable<string> keys);

        //
        // Summary:
        //     Removes the specified item from the cache.
        //
        // Parameters:
        //   key:
        //     The identifier for the item to delete.
        //
        // Returns:
        //     true if the item was successfully removed from the cache; false otherwise.
        bool Remove(string key);
        //
        // Summary:
        //     Removes the cache for all the keys provided.
        //
        // Parameters:
        //   keys:
        //     The keys.
        void RemoveAll(IEnumerable<string> keys);
        bool Replace<T>(string key, T value, DateTime expiresAt);
        bool Replace<T>(string key, T value, TimeSpan expiresIn);
        //
        // Summary:
        //     Replaces the item at the cachekey specified only if an items exists at the location
        //     already.
        bool Replace<T>(string key, T value);
        bool Set<T>(string key, T value, DateTime expiresAt);
        //
        // Summary:
        //     Sets an item into the cache at the cache key specified regardless if it already
        //     exists or not.
        bool Set<T>(string key, T value);
        bool Set<T>(string key, T value, TimeSpan expiresIn);
        //
        // Summary:
        //     Sets multiple items to the cache.
        //
        // Parameters:
        //   values:
        //     The values.
        //
        // Type parameters:
        //   T:
        void SetAll<T>(IDictionary<string, T> values);
    }
}
 