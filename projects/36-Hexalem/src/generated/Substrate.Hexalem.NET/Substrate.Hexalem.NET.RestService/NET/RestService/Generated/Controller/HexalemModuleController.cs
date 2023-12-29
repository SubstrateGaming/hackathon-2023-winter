//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Substrate.Hexalem.NET.RestService.Generated.Storage;
using Substrate.NetApi.Model.Types.Base;
using Substrate.ServiceLayer.Attributes;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Substrate.Hexalem.NET.RestService.Generated.Controller
{
    
    
    /// <summary>
    /// HexalemModuleController controller to access storages.
    /// </summary>
    [ApiController()]
    [Route("[controller]")]
    public sealed class HexalemModuleController : ControllerBase
    {
        
        private IHexalemModuleStorage _hexalemModuleStorage;
        
        /// <summary>
        /// HexalemModuleController constructor.
        /// </summary>
        public HexalemModuleController(IHexalemModuleStorage hexalemModuleStorage)
        {
            _hexalemModuleStorage = hexalemModuleStorage;
        }
        
        /// <summary>
        /// >> GameStorage
        /// </summary>
        [HttpGet("GameStorage")]
        [ProducesResponseType(typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Game), 200)]
        [StorageKeyBuilder(typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Storage.HexalemModuleStorage), "GameStorageParams", typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8))]
        public IActionResult GetGameStorage(string key)
        {
            return this.Ok(_hexalemModuleStorage.GetGameStorage(key));
        }
        
        /// <summary>
        /// >> HexBoardStorage
        /// </summary>
        [HttpGet("HexBoardStorage")]
        [ProducesResponseType(typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.HexBoard), 200)]
        [StorageKeyBuilder(typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Storage.HexalemModuleStorage), "HexBoardStorageParams", typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32))]
        public IActionResult GetHexBoardStorage(string key)
        {
            return this.Ok(_hexalemModuleStorage.GetHexBoardStorage(key));
        }
        
        /// <summary>
        /// >> TargetGoalStorage
        /// </summary>
        [HttpGet("TargetGoalStorage")]
        [ProducesResponseType(typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr16U8), 200)]
        [StorageKeyBuilder(typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Storage.HexalemModuleStorage), "TargetGoalStorageParams", typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32))]
        public IActionResult GetTargetGoalStorage(string key)
        {
            return this.Ok(_hexalemModuleStorage.GetTargetGoalStorage(key));
        }
    }
}
