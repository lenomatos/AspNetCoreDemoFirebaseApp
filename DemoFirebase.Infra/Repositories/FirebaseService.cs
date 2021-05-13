using DemoFirebase.Infra.Models.Firestore;
using DemoFirebase.Infra.Repositories.Interfaces;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DemoFirebase.Infra.Repositories
{
    public class FirebaseService<TEntity> : IFirebaseService<TEntity> where TEntity : Base
    {
        private readonly IHostEnvironment _hostEnvironment;
        private FirebaseSettings _firebaseSettings;
        private FirestoreDb _fireStoreDb;
        public FirebaseService(IOptions<FirebaseSettings> firebaseSettings,
            IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;

            string contentRootPath = _hostEnvironment.ContentRootPath;
            string pathCredential = Path.Combine(contentRootPath, @"App_Data\YOUR-KEY");

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", pathCredential);
            _firebaseSettings = firebaseSettings.Value;
            _fireStoreDb = FirestoreDb.Create(_firebaseSettings.DatabaseName);
        }

        public TEntity Add(TEntity record)
        {
            try
            {
                CollectionReference colRef = _fireStoreDb.Collection(_firebaseSettings.CollectionName);
                DocumentReference doc = colRef.AddAsync(record).GetAwaiter().GetResult();
                record.Id = doc.Id;
                return record;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public bool Delete(TEntity record)
        {
            try
            {
                DocumentReference recordRef = _fireStoreDb.Collection(_firebaseSettings.CollectionName).Document(record.Id);
                recordRef.DeleteAsync().GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TEntity Get(TEntity record)
        {
            try
            {
                DocumentReference docRef = _fireStoreDb.Collection(_firebaseSettings.CollectionName).Document(record.Id);
                DocumentSnapshot snapshot = docRef.GetSnapshotAsync().GetAwaiter().GetResult();
                if (snapshot.Exists)
                {
                    Dictionary<string, object> item = snapshot.ToDictionary();
                    string json = JsonConvert.SerializeObject(item);
                    TEntity newItem = JsonConvert.DeserializeObject<TEntity>(json);
                    newItem.Id = snapshot.Id;
                    return newItem;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TEntity> GetAll()
        {
            try
            {
                Query query = _fireStoreDb.Collection(_firebaseSettings.CollectionName);
                QuerySnapshot querySnapshot = query.GetSnapshotAsync().GetAwaiter().GetResult();
                List<TEntity> list = new List<TEntity>();
                foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
                {
                    if (documentSnapshot.Exists)
                    {
                        Dictionary<string, object> item = documentSnapshot.ToDictionary();
                        string json = JsonConvert.SerializeObject(item);
                        TEntity newItem = JsonConvert.DeserializeObject<TEntity>(json);
                        newItem.Id = documentSnapshot.Id;
                        list.Add(newItem);
                    }
                }
                return list;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool Set(TEntity record)
        {
            try
            {
                DocumentReference recordRef = _fireStoreDb.Collection(_firebaseSettings.CollectionName).Document(record.Id);
                recordRef.SetAsync(record).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SetMerge(TEntity record)
        {
            try
            {
                DocumentReference recordRef = _fireStoreDb.Collection(_firebaseSettings.CollectionName).Document(record.Id);
                recordRef.SetAsync(record, SetOptions.MergeAll).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Update(TEntity record)
        {
            try
            {
                DocumentReference recordRef = _fireStoreDb.Collection(_firebaseSettings.CollectionName).Document(record.Id);
                Dictionary<string, object> update = new Dictionary<string, object>();
                
                update = record.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(prop => prop.Name, prop => (object)prop.GetValue(record, null));

                recordRef.UpdateAsync(update).GetAwaiter().GetResult();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
