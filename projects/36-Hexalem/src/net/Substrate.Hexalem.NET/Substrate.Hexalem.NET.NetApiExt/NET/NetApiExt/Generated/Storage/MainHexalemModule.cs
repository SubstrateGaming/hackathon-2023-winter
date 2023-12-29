//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Meta;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Substrate.Hexalem.NET.NetApiExt.Generated.Storage
{
    
    
    public sealed class HexalemModuleStorage
    {
        
        // Substrate client for the storage calls.
        private SubstrateClientExt _client;
        
        public HexalemModuleStorage(SubstrateClientExt client)
        {
            this._client = client;
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("HexalemModule", "GameStorage"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8), typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Game)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("HexalemModule", "HexBoardStorage"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32), typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.HexBoard)));
            _client.StorageKeyDict.Add(new System.Tuple<string, string>("HexalemModule", "TargetGoalStorage"), new System.Tuple<Substrate.NetApi.Model.Meta.Storage.Hasher[], System.Type, System.Type>(new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                            Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32), typeof(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr16U8)));
        }
        
        /// <summary>
        /// >> GameStorageParams
        /// </summary>
        public static string GameStorageParams(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8 key)
        {
            return RequestGenerator.GetStorage("HexalemModule", "GameStorage", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> GameStorageDefault
        /// Default value as hex string
        /// </summary>
        public static string GameStorageDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> GameStorage
        /// </summary>
        public async Task<Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Game> GameStorage(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8 key, CancellationToken token)
        {
            string parameters = HexalemModuleStorage.GameStorageParams(key);
            var result = await _client.GetStorageAsync<Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Game>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> HexBoardStorageParams
        /// </summary>
        public static string HexBoardStorageParams(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("HexalemModule", "HexBoardStorage", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> HexBoardStorageDefault
        /// Default value as hex string
        /// </summary>
        public static string HexBoardStorageDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> HexBoardStorage
        /// </summary>
        public async Task<Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.HexBoard> HexBoardStorage(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key, CancellationToken token)
        {
            string parameters = HexalemModuleStorage.HexBoardStorageParams(key);
            var result = await _client.GetStorageAsync<Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.HexBoard>(parameters, token);
            return result;
        }
        
        /// <summary>
        /// >> TargetGoalStorageParams
        /// </summary>
        public static string TargetGoalStorageParams(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key)
        {
            return RequestGenerator.GetStorage("HexalemModule", "TargetGoalStorage", Substrate.NetApi.Model.Meta.Storage.Type.Map, new Substrate.NetApi.Model.Meta.Storage.Hasher[] {
                        Substrate.NetApi.Model.Meta.Storage.Hasher.BlakeTwo128Concat}, new Substrate.NetApi.Model.Types.IType[] {
                        key});
        }
        
        /// <summary>
        /// >> TargetGoalStorageDefault
        /// Default value as hex string
        /// </summary>
        public static string TargetGoalStorageDefault()
        {
            return "0x00";
        }
        
        /// <summary>
        /// >> TargetGoalStorage
        /// </summary>
        public async Task<Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr16U8> TargetGoalStorage(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 key, CancellationToken token)
        {
            string parameters = HexalemModuleStorage.TargetGoalStorageParams(key);
            var result = await _client.GetStorageAsync<Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr16U8>(parameters, token);
            return result;
        }
    }
    
    public sealed class HexalemModuleCalls
    {
        
        /// <summary>
        /// >> create_game
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method CreateGame(Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> players, Substrate.NetApi.Model.Types.Primitive.U8 grid_size)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(players.Encode());
            byteArray.AddRange(grid_size.Encode());
            return new Method(21, "HexalemModule", 0, "create_game", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> play
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Play(Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Move move_played)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(move_played.Encode());
            return new Method(21, "HexalemModule", 1, "play", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> upgrade
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method Upgrade(Substrate.NetApi.Model.Types.Primitive.U8 place_index)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(place_index.Encode());
            return new Method(21, "HexalemModule", 2, "upgrade", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> finish_turn
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method FinishTurn()
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            return new Method(21, "HexalemModule", 3, "finish_turn", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> force_finish_turn
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ForceFinishTurn(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8 game_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(game_id.Encode());
            return new Method(21, "HexalemModule", 4, "force_finish_turn", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> receive_reward
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method ReceiveReward()
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            return new Method(21, "HexalemModule", 5, "receive_reward", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> root_delete_game
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method RootDeleteGame(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8 game_id)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(game_id.Encode());
            return new Method(21, "HexalemModule", 6, "root_delete_game", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> root_set_game
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method RootSetGame(Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr32U8 game_id, Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Game game)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(game_id.Encode());
            byteArray.AddRange(game.Encode());
            return new Method(21, "HexalemModule", 7, "root_set_game", byteArray.ToArray());
        }
        
        /// <summary>
        /// >> root_set_hex_board
        /// Contains a variant per dispatchable extrinsic that this pallet has.
        /// </summary>
        public static Method RootSetHexBoard(Substrate.Hexalem.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 player, Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.HexBoard hex_board)
        {
            System.Collections.Generic.List<byte> byteArray = new List<byte>();
            byteArray.AddRange(player.Encode());
            byteArray.AddRange(hex_board.Encode());
            return new Method(21, "HexalemModule", 8, "root_set_hex_board", byteArray.ToArray());
        }
    }
    
    public sealed class HexalemModuleConstants
    {
        
        /// <summary>
        /// >> MaxPlayers
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxPlayers()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x64000000");
            return result;
        }
        
        /// <summary>
        /// >> MinPlayers
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 MinPlayers()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x01");
            return result;
        }
        
        /// <summary>
        /// >> MaxRounds
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 MaxRounds()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x19");
            return result;
        }
        
        /// <summary>
        /// >> BlocksToPlayLimit
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 BlocksToPlayLimit()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x0A");
            return result;
        }
        
        /// <summary>
        /// >> MaxHexGridSize
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxHexGridSize()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x31000000");
            return result;
        }
        
        /// <summary>
        /// >> MaxTileSelection
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxTileSelection()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U32();
            result.Create("0x10000000");
            return result;
        }
        
        /// <summary>
        /// >> TileCosts
        /// </summary>
        public Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Arr15TileCost TileCosts()
        {
            var result = new Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Arr15TileCost();
            result.Create("0x1000011000011000011800011800011800012000012000012000012800012800012800013000013" +
                    "80001380001");
            return result;
        }
        
        /// <summary>
        /// >> TileResourceProductions
        /// </summary>
        public Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Arr8ResourceProductions TileResourceProductions()
        {
            var result = new Substrate.Hexalem.NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.Arr8ResourceProductions();
            result.Create("0x0000000000000000000000000000000100000000000000000000000000000002000000000000000" +
                    "00000000002000000000000000000000000000000000400000000000004000000000103000000000" +
                    "00002000000000000000000000000000000000000000000020100000000000203");
            return result;
        }
        
        /// <summary>
        /// >> WaterPerHuman
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 WaterPerHuman()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x02");
            return result;
        }
        
        /// <summary>
        /// >> FoodPerHuman
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 FoodPerHuman()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x01");
            return result;
        }
        
        /// <summary>
        /// >> HomePerHumans
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 HomePerHumans()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x03");
            return result;
        }
        
        /// <summary>
        /// >> FoodPerTree
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 FoodPerTree()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x01");
            return result;
        }
        
        /// <summary>
        /// >> DefaultPlayerResources
        /// </summary>
        public Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr7U8 DefaultPlayerResources()
        {
            var result = new Substrate.Hexalem.NET.NetApiExt.Generated.Types.Base.Arr7U8();
            result.Create("0x01010000000000");
            return result;
        }
        
        /// <summary>
        /// >> TargetGoalGold
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 TargetGoalGold()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x0A");
            return result;
        }
        
        /// <summary>
        /// >> TargetGoalHuman
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 TargetGoalHuman()
        {
            var result = new Substrate.NetApi.Model.Types.Primitive.U8();
            result.Create("0x07");
            return result;
        }
    }
    
    public enum HexalemModuleErrors
    {
        
        /// <summary>
        /// >> AlreadyPlaying
        /// </summary>
        AlreadyPlaying,
        
        /// <summary>
        /// >> GameNotInitialized
        /// </summary>
        GameNotInitialized,
        
        /// <summary>
        /// >> HexBoardNotInitialized
        /// </summary>
        HexBoardNotInitialized,
        
        /// <summary>
        /// >> CreatorNotInPlayersAtIndexZero
        /// </summary>
        CreatorNotInPlayersAtIndexZero,
        
        /// <summary>
        /// >> GameAlreadyCreated
        /// </summary>
        GameAlreadyCreated,
        
        /// <summary>
        /// >> InternalError
        /// </summary>
        InternalError,
        
        /// <summary>
        /// >> NumberOfPlayersIsTooSmall
        /// </summary>
        NumberOfPlayersIsTooSmall,
        
        /// <summary>
        /// >> NumberOfPlayersIsTooLarge
        /// </summary>
        NumberOfPlayersIsTooLarge,
        
        /// <summary>
        /// >> MathOverflow
        /// </summary>
        MathOverflow,
        
        /// <summary>
        /// >> NotEnoughResources
        /// </summary>
        NotEnoughResources,
        
        /// <summary>
        /// >> NotEnoughPopulation
        /// </summary>
        NotEnoughPopulation,
        
        /// <summary>
        /// >> BuyIndexOutOfBounds
        /// </summary>
        BuyIndexOutOfBounds,
        
        /// <summary>
        /// >> PlaceIndexOutOfBounds
        /// </summary>
        PlaceIndexOutOfBounds,
        
        /// <summary>
        /// >> PlayerNotOnTurn
        /// </summary>
        PlayerNotOnTurn,
        
        /// <summary>
        /// >> PlayerNotInGame
        /// </summary>
        PlayerNotInGame,
        
        /// <summary>
        /// >> CurrentPlayerCannotForceFinishTurn
        /// </summary>
        CurrentPlayerCannotForceFinishTurn,
        
        /// <summary>
        /// >> GameNotPlaying
        /// </summary>
        GameNotPlaying,
        
        /// <summary>
        /// >> BadGridSize
        /// </summary>
        BadGridSize,
        
        /// <summary>
        /// >> TileIsNotEmpty
        /// </summary>
        TileIsNotEmpty,
        
        /// <summary>
        /// >> TileOnMaxLevel
        /// </summary>
        TileOnMaxLevel,
        
        /// <summary>
        /// >> CannotLevelUpEmptyTile
        /// </summary>
        CannotLevelUpEmptyTile,
        
        /// <summary>
        /// >> CannotLevelUp
        /// </summary>
        CannotLevelUp,
        
        /// <summary>
        /// >> TileSurroundedByEmptyTiles
        /// </summary>
        TileSurroundedByEmptyTiles,
        
        /// <summary>
        /// >> BlocksToPlayLimitNotPassed
        /// </summary>
        BlocksToPlayLimitNotPassed,
    }
}
