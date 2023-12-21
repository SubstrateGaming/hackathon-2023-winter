//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using Substrate.ServiceLayer.Attributes;
using Substrate.ServiceLayer.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Substrate.Hexalem.NET.RestService.Generated.Storage
{
    
    
    /// <summary>
    /// ITimestampStorage interface definition.
    /// </summary>
    public interface ITimestampStorage : IStorage
    {
        
        /// <summary>
        /// >> Now
        ///  Current time for the current block.
        /// </summary>
        Substrate.NetApi.Model.Types.Primitive.U64 GetNow();
        
        /// <summary>
        /// >> DidUpdate
        ///  Did the timestamp get updated in this block?
        /// </summary>
        Substrate.NetApi.Model.Types.Primitive.Bool GetDidUpdate();
    }
    
    /// <summary>
    /// TimestampStorage class definition.
    /// </summary>
    public sealed class TimestampStorage : ITimestampStorage
    {
        
        /// <summary>
        /// _nowTypedStorage typed storage field
        /// </summary>
        private TypedStorage<Substrate.NetApi.Model.Types.Primitive.U64> _nowTypedStorage;
        
        /// <summary>
        /// _didUpdateTypedStorage typed storage field
        /// </summary>
        private TypedStorage<Substrate.NetApi.Model.Types.Primitive.Bool> _didUpdateTypedStorage;
        
        /// <summary>
        /// TimestampStorage constructor.
        /// </summary>
        public TimestampStorage(IStorageDataProvider storageDataProvider, List<IStorageChangeDelegate> storageChangeDelegates)
        {
            this.NowTypedStorage = new TypedStorage<Substrate.NetApi.Model.Types.Primitive.U64>("Timestamp.Now", storageDataProvider, storageChangeDelegates);
            this.DidUpdateTypedStorage = new TypedStorage<Substrate.NetApi.Model.Types.Primitive.Bool>("Timestamp.DidUpdate", storageDataProvider, storageChangeDelegates);
        }
        
        /// <summary>
        /// _nowTypedStorage property
        /// </summary>
        public TypedStorage<Substrate.NetApi.Model.Types.Primitive.U64> NowTypedStorage
        {
            get
            {
                return _nowTypedStorage;
            }
            set
            {
                _nowTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _didUpdateTypedStorage property
        /// </summary>
        public TypedStorage<Substrate.NetApi.Model.Types.Primitive.Bool> DidUpdateTypedStorage
        {
            get
            {
                return _didUpdateTypedStorage;
            }
            set
            {
                _didUpdateTypedStorage = value;
            }
        }
        
        /// <summary>
        /// Connects to all storages and initializes the change subscription handling.
        /// </summary>
        public async Task InitializeAsync(Substrate.ServiceLayer.Storage.IStorageDataProvider dataProvider)
        {
            await NowTypedStorage.InitializeAsync("Timestamp", "Now");
            await DidUpdateTypedStorage.InitializeAsync("Timestamp", "DidUpdate");
        }
        
        /// <summary>
        /// Implements any storage change for Timestamp.Now
        /// </summary>
        [StorageChange("Timestamp", "Now")]
        public void OnUpdateNow(string data)
        {
            NowTypedStorage.Update(data);
        }
        
        /// <summary>
        /// >> Now
        ///  Current time for the current block.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U64 GetNow()
        {
            return NowTypedStorage.Get();
        }
        
        /// <summary>
        /// Implements any storage change for Timestamp.DidUpdate
        /// </summary>
        [StorageChange("Timestamp", "DidUpdate")]
        public void OnUpdateDidUpdate(string data)
        {
            DidUpdateTypedStorage.Update(data);
        }
        
        /// <summary>
        /// >> DidUpdate
        ///  Did the timestamp get updated in this block?
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool GetDidUpdate()
        {
            return DidUpdateTypedStorage.Get();
        }
    }
}
