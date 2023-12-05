﻿using Serilog;
using Substrate.Hexalem.Integration.Model;
using Substrate.Hexalem.NET;
using Substrate.Hexalem.NET.Extensions;
using Substrate.Hexalem.NET.GameException;
using Substrate.Integration.Helper;
using Substrate.NetApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Substrate.Hexalem.Test")]

namespace Substrate.Hexalem
{
    public partial class HexaGame : IHexaBase
    {
        public TileOffer[] ALL_TILE_OFFERS = new TileOffer[16] {
            new TileOffer {
                TileToBuy = new HexaTile(104), // Tree, level 1
                TileCost = new MaterialCost {
                    MaterialType = RessourceType.Mana,
                    Cost = 1,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(104), // Tree, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(104), // Tree, level 1
                TileCost = new MaterialCost {
                    MaterialType = RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(96), // Mountain, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 1,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(96), // Mountain, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(96), // Mountain, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(80), // Grass, level 1
                TileCost = new MaterialCost {
                    MaterialType = RessourceType.Mana,
                    Cost = 1,
                }
            },
             new TileOffer {
                TileToBuy = new HexaTile(80), // Grass, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 1,
                }
            },
              new TileOffer {
                TileToBuy = new HexaTile(80), // Grass, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(88), // Water, level 1
                TileCost = new MaterialCost {
                    MaterialType = RessourceType.Mana,
                    Cost = 1,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(88), // Water, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 1,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(88), // Water, level 1
                TileCost = new MaterialCost {
                    MaterialType = RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(72), // Home, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(72), // Home, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 2,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(72), // Home, level 1
                TileCost = new MaterialCost {
                    MaterialType =  RessourceType.Mana,
                    Cost = 3,
                }
            },
            new TileOffer {
                TileToBuy = new HexaTile(72), // Home, level 1
                TileCost = new MaterialCost {
                    MaterialType = RessourceType.Mana,
                    Cost = 3,
                }
            },
        };

        public byte[] Id { get; set; }

        public byte[] Value { get; set; }

        /// <summary>
        /// Associate a player and his board
        /// </summary>
        public List<(HexaPlayer player, HexaBoard board)> HexaTuples { get; set; }

        /// <summary>
        /// Tiles that can be bought by players
        /// </summary>
        public List<byte> UnboundTileOffers { get; private set; }

        protected HexaGame()
        {
            Value = new byte[GameConfig.GAME_STORAGE_SIZE];
        }

        public HexaGame(GameSharp game, BoardSharp[] boards) : this()
        {
            if (boards.Any(x => Utils.Bytes2HexString(x.GameId) != Utils.Bytes2HexString(game.GameId)))
                throw new InvalidOperationException($"Error while trying to create an HexaGame instance with different gameId ({game.GameId}/{boards.ToLog()})");

            if (boards.Length != game.Players.Length)
                throw new InvalidOperationException($"Inconsistent boards count (={boards.Length}) and players count (={game.Players.Length})");

            Id = game.GameId;

            switch (game.State)
            {
                case NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.GameState.Matchmaking:
                    HexBoardState = HexBoardState.Preparing;
                    break;

                case NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.GameState.Playing:
                    HexBoardState = HexBoardState.Running;
                    break;

                case NET.NetApiExt.Generated.Model.pallet_hexalem.pallet.GameState.Finished:
                    HexBoardState = HexBoardState.Finish;
                    break;
            }

            HexBoardRound = game.Round;
            PlayerTurn = game.PlayerTurn;
            SelectBase = game.SelectionSize;
            UnboundTileOffers = game.Selection.Select(x => x).ToList();

            // Assume that board and players are ordered
            HexaTuples = new List<(HexaPlayer, HexaBoard)>();
            foreach (var (board, playerAddress) in boards.Zip(game.Players, (b, p) => (b, p)))
            {
                var hexTiles = board.HexGrid.Select(x => new HexaTile(x));
                var currentBoard = new HexaBoard(hexTiles.Select(x => x.Value).ToArray());

                var ressources = new List<byte>()
                {
                    board.Mana,
                    board.Humans,
                    board.Water,
                    board.Food,
                    board.Wood,
                    board.Stone,
                    board.Gold
                };

                var currentPlayer = new HexaPlayer(Utils.GetPublicKeyFrom(playerAddress), ressources.ToArray());

                HexaTuples.Add((currentPlayer, currentBoard));
            }

            PlayersCount = (byte)HexaTuples.Count;
        }

        public HexaGame(byte[] id, List<(HexaPlayer, HexaBoard)> hexaTuples) : this()
        {
            Id = id;

            HexaTuples = hexaTuples;
            UnboundTileOffers = new List<byte>();

            HexBoardState = HexBoardState.Preparing;
            PlayersCount = (byte)hexaTuples.Count;
        }

        public void Init(uint blockNumber)
        {
            HexaTuples.ForEach(p =>
            {
                p.player.Init(blockNumber);
                p.board.Init(blockNumber);
            });

            HexBoardState = HexBoardState.Running;
            HexBoardRound = 0;
            PlayerTurn = 0;
            SelectBase = 2;

            UnboundTileOffers = NewSelection(blockNumber, SelectBase);
        }

        public void NextRound(uint blockNumber)
        {
            HexaTuples.ForEach(p => { p.player.NextRound(blockNumber); p.board.NextRound(blockNumber); });

            PlayerTurn = 0;
            HexBoardRound += 1;
            Log.Information("Next round : reset turn to 0 and increase board round (now = {hbt})", HexBoardRound);
        }

        public void PostMove(uint blockNumber)
        {
            HexaTuples.ForEach(p => { p.player.PostMove(blockNumber); p.board.PostMove(blockNumber); });

            if (UnboundTileOffers.Count < (SelectBase + 1) / 2)
            {
                Log.Debug("UnboundTileOffers is below half");
                if (SelectBase < GameConfig.NB_MAX_UNBOUNDED_TILES / 2)
                {
                    Log.Debug($"Selection is now {SelectBase}");
                    SelectBase += 2;
                }

                UnboundTileOffers = RefillSelection(blockNumber, SelectBase);
            }
        }

        /// <summary>
        /// New selection
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="selectBase">selection size</param>
        /// <returns></returns>
        internal List<byte> NewSelection(uint blockNumber, int selectBase)
        {
            var offSet = (byte)(blockNumber % 32);

            var result = new List<byte>();

            for (int i = 0; i < selectBase; i++)
            {
                byte tileIndex = (byte)(Id[(offSet + i) % 32] % 16);

                result.Add(tileIndex);
            }
            return result;
        }

        /// <summary>
        /// Refill selection
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="selectBase">selection size</param>
        /// <returns></returns>
        internal List<byte> RefillSelection(uint blockNumber, int selectBase)
        {

            var offSet = (byte)(blockNumber % 32);
            var result = new List<byte>();
            for (int i = UnboundTileOffers.Count(); i < selectBase; i++)
            {
                byte tileIndex = (byte)(Id[(offSet + i) % 32] % 16);

                result.Add(tileIndex);
            }

            return result;
        }
    

        /// <param name="playerIndex"></param>
        /// <param name="selectionIndex"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool CanChooseAndPlace(byte playerIndex, int selectionIndex, (int, int) coords)
        {
            if (!EnsureCurrentPlayer(playerIndex))
            {
                return false;
            }

            if (!EnsureValidSelection(selectionIndex))
            {
                return false;
            }

            var (player, board) = HexaTuples[PlayerTurn];

            var tileOffer = ALL_TILE_OFFERS[UnboundTileOffers[selectionIndex]];

            if (!EnsureRessourcesToPlay(player, tileOffer))
            {
                return false;
            }

            if (!EnsureValidCoords(board, coords))
            {
                return false;
            }

            return board.CanPlace(coords);
        }

        /// <summary>
        /// Choose and place a tile on the board
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="selectionIndex"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        internal bool ChooseAndPlace(byte playerIndex, int selectionIndex, (int, int) coords)
        {
            if (!CanChooseAndPlace(playerIndex, selectionIndex, coords))
            {
                return false;
            }

            var (player, board) = HexaTuples[PlayerTurn];

            var tileOffer = ALL_TILE_OFFERS[UnboundTileOffers[selectionIndex]];

            HexaTile tile = tileOffer.TileToBuy;

            // check if tile can be placed
            board.Place(coords, tile);

            // remove ressources from player
            var tileCost = tileOffer.TileCost;

            player[tileCost.MaterialType] -= tileCost.Cost;
            

            UnboundTileOffers.RemoveAt(selectionIndex);
            Log.Debug("UnboundTile num {num} succesfully removed", selectionIndex);

            Played = true;

            return true;
        }

        /// <summary>
        /// Can upgrade a tile
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        public bool CanUpgrade(byte playerIndex, (int q, int r) coords)
        {
            if (!EnsureCurrentPlayer(playerIndex))
            {
                return false;
            }

            var (player, board) = HexaTuples[PlayerTurn];

            var tile = (HexaTile)board[coords.q, coords.r];
            if (!EnsureUpgradableTile(tile))
            {
                return false;
            }

            if (!EnsureRessourcesToUpgrade(player, tile))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Upgrade a tile
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        internal bool Upgrade(byte playerIndex, (int q, int r) coords)
        {
            if (!CanUpgrade(playerIndex, coords))
            {
                return false;
            }

            var (player, board) = HexaTuples[PlayerTurn];

            // Ensure coord have a valid tile
            var existingTile = (HexaTile)board[coords.q, coords.r];

            // Upgrade tile to next level, if failed return
            existingTile.Upgrade();

            HexaTuples[PlayerTurn].board[coords.q, coords.r] = existingTile;
            player[RessourceType.Gold] -= (byte)GameConfig.GoldCostForUpgrade(existingTile.TileRarity);
            player[RessourceType.Humans] -= (byte)GameConfig.MininumHumanToUpgrade(existingTile.TileRarity);

            return true;
        }

        /// <summary>
        /// Update game turn information
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        internal bool UpdateTurnState(uint blockNumber, byte playerIndex)
        {
            // check if correct player
            if (!EnsureCurrentPlayer(playerIndex))
            {
                return false;
            }

            var nbBlockSpentSinceLastMove = blockNumber - BitConverter.ToUInt16(LastMove);
            if (nbBlockSpentSinceLastMove > GameConfig.MAX_TURN_BLOCKS)
            {
                Log.Error(LogMessages.TooMuchTimeToPlay(nbBlockSpentSinceLastMove));
                return false;
            }

            // do storage changes
            LastMove = BitConverter.GetBytes(blockNumber);
            Log.Debug("Saved LastMove {lm}", LastMove.ToLog());

            PlayerTurn = (byte)((PlayerTurn + 1) % PlayersCount);
            Log.Debug("Switch to player {p}", PlayerTurn);

            // If the player has not played, generate a new selection
            if (Played)
            {
                Played = false;
            }
            else
            {
                UnboundTileOffers = NewSelection(blockNumber, SelectBase);
            }

            return true;
        }

        /// <summary>
        /// Check if a player has won the game
        /// </summary>
        /// <returns></returns>
        public bool IsGameWon()
        {
            var player = HexaTuples[PlayerTurn].player;

            if (player.HasWin())
            {
                Log.Information("Player {num} has reached his win condition {winCondition} !", PlayerTurn, player.WinningCondition.WinningCondition);

                return true;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="blockNumber"></param>
        /// <param name="selectBase"></param>
        /// <returns></returns>
        internal List<HexaTile> RenewSelection(uint blockNumber, int selectBase)
        {
            var values = Enum.GetValues(typeof(TileType))
                .Cast<TileType>()
                .Where(v => (int)v > 1).ToArray();

            var offSet = (byte)(blockNumber % 32);
            var result = new List<HexaTile>();
            for (int i = 0; i < selectBase; i++)
            {
                var rawTile = Id[(offSet + selectBase) % 32];

                result.Add(new HexaTile(values[rawTile % values.Length], TileRarity.Normal, TilePattern.Normal));
            }
            return result;
        }

        internal void CalcRewards(uint blockNumber, byte playerIndex)
        {
            var hexaPlayer = HexaTuples[playerIndex].player;
            var hexaBoard = HexaTuples[playerIndex].board;

            Evaluate(hexaBoard, hexaPlayer);
        }

        internal void Evaluate(HexaBoard hexaBoard, HexaPlayer player)
        {
            SimpleBoardStats boardStats = hexaBoard.SimpleStats();

            // https://www.simplypsychology.org/maslow.html

            player[RessourceType.Mana] += (byte)(boardStats.Homes * 1); // 1 Mana from Home
            player[RessourceType.Mana] += (byte)(player[RessourceType.Humans] / 2); // 1 Mana from 2 Humans

            // Additional pattern logic
            // none

            // Physiological needs: breathing, food, water, shelter, clothing, sleep
            byte foodAndWaterEaten = (byte)Math.Min(
                player[RessourceType.Food] * GameConfig.FOOD_PER_HUMANS,
                player[RessourceType.Water] * GameConfig.WATER_PER_HUMANS
            );

            player[RessourceType.Humans] = (byte)(
                Math.Min(
                    foodAndWaterEaten,
                    boardStats.Homes * GameConfig.HOME_PER_HUMANS
                )
            );

            // Additional pattern logic
            // none


            player[RessourceType.Water] += (byte)(boardStats.Waters * GameConfig.WATER_PER_WATER);

            // Additional pattern logic
            player[RessourceType.Water] += (byte)(boardStats.Rivers * 3);
            player[RessourceType.Water] += boardStats.ExtremeMountains;


            player[RessourceType.Food] += (byte)(boardStats.Grass * GameConfig.FOOD_PER_GRASS);

            // Additional pattern logic
            player[RessourceType.Food] += (byte)(boardStats.Farms * 3);

            // 1 tree needs 2 humans for 1 (First tree needs just 1 human)
            player[RessourceType.Wood] += (byte)Math.Min(boardStats.Trees, (player[RessourceType.Humans] + 1) / 2);

            // Additional pattern logic
            player[RessourceType.Wood] += (byte)(boardStats.Forrests * 3);


            // 1 Mountain needs 2 humans
            player[RessourceType.Stone] += (byte)Math.Min(boardStats.Mountains, player[RessourceType.Humans] / 2);

            // 1 Cave can create wood for 4 humans, but need 2 humans for 1
            /// Not implemented
            // result += (byte)Math.Min(boardStats[TileType.Cave] * 2, player[RessourceType.Humans] / 2);

            // Additional pattern logic

            player[RessourceType.Stone] += (byte)(boardStats.Mountains * 3);
        }

        /// <summary>
        /// Check if playerIndex is the right <see cref="PlayerTurn"/>
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns>True if it is a valid playerIndex</returns>
        private bool EnsureCurrentPlayer(byte playerIndex)
        {
            if (PlayerTurn != playerIndex)
            {
                Log.Error(LogMessages.InvalidPlayerTurn(playerIndex, PlayerTurn));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure that the tile can be upgraded
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool EnsureUpgradableTile(HexaTile tile)
        {
            var upgradableTileTypes = GameConfig.UpgradableTypeTile();

            if (tile.TileType == TileType.Empty
             || tile.TileRarity == TileRarity.Legendary
             || !upgradableTileTypes.Contains(tile.TileType))
            {
                Log.Error(LogMessages.InvalidTileToUpgrade(tile));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure that the player have enough ressources to upgrade the tile
        /// </summary>
        /// <param name="player"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool EnsureRessourcesToUpgrade(HexaPlayer player, HexaTile tile)
        {
            // Check if player have enought ressources to upgrade
            var goldRequired = GameConfig.GoldCostForUpgrade(tile.TileRarity);
            var humansRequired = GameConfig.MininumHumanToUpgrade(tile.TileRarity);
            if (player[RessourceType.Gold] < goldRequired
             || player[RessourceType.Humans] < humansRequired)
            {
                Log.Error(LogMessages.MissingRessourcesToUpgrade(player, tile, goldRequired, humansRequired));
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="player"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        private bool EnsureRessourcesToPlay(HexaPlayer player, TileOffer tileOffer)
        {
            var tileCost = tileOffer.TileCost;


            if (player[tileCost.MaterialType] < tileCost.Cost)
            {
                Log.Error(LogMessages.MissingRessourcesToPlay(player, tileOffer.TileToBuy, tileCost.MaterialType, tileCost.Cost));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure that the selection index is valid
        /// </summary>
        /// <param name="selectionIndex"></param>
        /// <returns></returns>
        private bool EnsureValidSelection(int selectionIndex)
        {
            if (selectionIndex < 0 || selectionIndex >= UnboundTileOffers.Count)
            {
                Log.Error(LogMessages.InvalidTileSelection(selectionIndex));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensure that the coords are valid
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        private bool EnsureValidCoords(HexaBoard board, (int q, int r) coords)
        {
            if (!board.IsValidHex(coords.q, coords.r))
            {
                Log.Error(LogMessages.InvalidCoords(coords.q, coords.r));
                return false;
            }

            return true;
        }

        public HexaGame Clone()
        {
            var gameId = (byte[])this.Id.Clone();
            var players = this.HexaTuples.Select(x => (x.player.Clone(), x.board.Clone())).ToList();

            var cloneGame = new HexaGame(gameId, players);

            cloneGame.HexBoardState = this.HexBoardState;
            cloneGame.HexBoardRound = this.HexBoardRound;
            cloneGame.PlayerTurn = this.PlayerTurn;
            cloneGame.SelectBase = this.SelectBase;

            cloneGame.UnboundTileOffers = this.UnboundTileOffers.Select(x => x).ToList();

            return cloneGame;
        }

        public override string ToString()
        {
            string log = $"HexaGame value :";

            log += $"\n\t Id = {Utils.Bytes2HexString(Id)}";
            log += $"\n\t HexBoardState = {HexBoardState}";
            log += $"\n\t HexBoardRound = {HexBoardRound}";
            log += $"\n\t PlayerTurn = {PlayerTurn}";
            log += $"\n\t UnboundTileOffers.Length = {UnboundTileOffers.Count}";

            log += $"\n\t Nb players = {PlayersCount}";
            for (int i = 0; i < PlayersCount; i++)
            {
                log += $"\n\t\t Player {i} = {HexaTuples[i].player.Id.ToAddress()}";
            }

            return log;
        }
    }

    /// <summary>
    /// HexaGame storage class
    /// </summary>
    public partial class HexaGame
    {
        public HexBoardState HexBoardState
        {
            get => (HexBoardState)Value[0];
            set => Value[0] = (byte)value;
        }

        /// <summary>
        /// Holding the current round number
        /// There is a maximum of 64 rounds per game
        /// </summary>
        public byte HexBoardRound
        {
            get => Value[1];
            set => Value[1] = value;
        }

        /// <summary>
        /// Number of players in the game
        /// </summary>
        public byte PlayersCount
        {
            get => Value[3];
            set => Value[3] = value;
        }

        /// <summary>
        /// Player index which is currently play
        /// </summary>
        public byte PlayerTurn
        {
            get => Value[4];
            set => Value[4] = value;
        }

        /// <summary>
        /// Nb tiles a player can buy during his turn
        /// </summary>
        public byte SelectBase
        {
            get => Value[5];
            set => Value[5] = value;
        }

        /// <summary>
        /// Last block number when a player made a move
        /// </summary>
        public byte[] LastMove
        {
            get => Value.Skip(6).Take(4).ToArray();
            set => value.CopyTo(Value, 6);
        }

        public bool Played { get; set; } = false;
    }
}