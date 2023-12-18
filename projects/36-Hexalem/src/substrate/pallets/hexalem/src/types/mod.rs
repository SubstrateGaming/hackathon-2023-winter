use frame_support::pallet_prelude::*;
use sp_runtime::SaturatedConversion;
use sp_std::vec;

// Custom trait for Tile definition
pub trait GetTileInfo {
	fn get_level(&self) -> u8;
	fn set_level(&mut self, level: u8) -> ();

	fn get_type(&self) -> TileType;

	fn get_pattern(&self) -> TilePattern;
	fn set_pattern(&mut self, value: TilePattern) -> ();

	fn same(&self, other: &Self) -> bool {
		self.get_type() == other.get_type()
	}

	fn get_home() -> Self;
}

pub trait GameProperties<Account, MaxPlayers> {
	// Player made a move
	// It is used for determining whether to generate a new selection
	fn get_played(&self) -> bool;
	fn set_played(&mut self, played: bool) -> ();

	fn get_round(&self) -> u8;
	fn set_round(&mut self, round: u8) -> ();

	fn get_player_turn(&self) -> u8;
	fn set_player_turn(&mut self, turn: u8) -> ();

	fn get_state(&self) -> GameState;
	fn set_state(&mut self, state: GameState) -> ();

	fn borrow_players(&self) -> &Players<Account, MaxPlayers>;

	fn get_selection_size(&self) -> u8;
	fn set_selection_size(&mut self, selection_size: u8) -> ();

	fn next_turn(&mut self) -> ();
}

pub type GameId = [u8; 32];

pub type TargetGoalHash = [u8; 16];

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen, PartialEq, Copy, Clone, Debug)]
pub enum GameState {
	Matchmaking,
	Playing,
	Finished { winner: Option<u8> }, // Ready to reward players
}

// Index used for referencing the TileCost
pub type TileCostIndex = u8;

// Tiles to select
pub type TileSelection<N> = BoundedVec<TileCostIndex, N>;

pub type Players<Account, N> = BoundedVec<Account, N>;

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen)]
pub struct Game<Account, BlockNumber, MaxPlayers, MaxTiles> {
	pub state: GameState,
	pub player_turn_and_played: u8,
	pub last_played_block: BlockNumber,
	pub players: Players<Account, MaxPlayers>, // Player AccountIds
	pub selection: TileSelection<MaxTiles>,
	pub selection_size: u8,
	pub round: u8,
	pub max_rounds: u8,
}

impl<Account, BlockNumber, MaxPlayers, MaxTiles> GameProperties<Account, MaxPlayers>
	for Game<Account, BlockNumber, MaxPlayers, MaxTiles>
{
	fn get_played(&self) -> bool {
		((self.player_turn_and_played & 0x80) >> 7) == 1
	}

	fn set_played(&mut self, played: bool) -> () {
		self.player_turn_and_played = (self.player_turn_and_played & 0x7F) | (played as u8) << 7
	}

	fn get_round(&self) -> u8 {
		self.round
	}

	fn set_round(&mut self, round: u8) -> () {
		self.round = round;
	}

	fn get_player_turn(&self) -> u8 {
		self.player_turn_and_played & 0x7F
	}

	fn set_player_turn(&mut self, turn: u8) -> () {
		self.player_turn_and_played = (self.player_turn_and_played & 0x80) | turn;
	}

	fn get_state(&self) -> GameState {
		self.state
	}

	fn set_state(&mut self, state: GameState) -> () {
		self.state = state;
	}

	fn borrow_players(&self) -> &Players<Account, MaxPlayers> {
		&self.players
	}

	fn get_selection_size(&self) -> u8 {
		self.selection_size
	}

	fn set_selection_size(&mut self, selection_size: u8) -> () {
		self.selection_size = selection_size;
	}

	fn next_turn(&mut self) -> () {
		let player_turn = self.get_player_turn();

		let next_player_turn =
			(player_turn + 1) % self.borrow_players().len().saturated_into::<u8>();

		self.set_player_turn(next_player_turn);

		if next_player_turn == 0 {
			let round = self.get_round() + 1;
			self.set_round(round);

			if round > self.max_rounds {
				self.set_state(GameState::Finished { winner: None });
			}
		}
	}
}

pub type ResourceUnit = u8;

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen, Clone, Copy, PartialEq, Debug)]
pub enum ResourceType {
	Mana = 0,
	Human = 1,
	Water = 2,
	Food = 3,
	Wood = 4,
	Stone = 5,
	Gold = 6,
}

pub const NUMBER_OF_RESOURCE_TYPES: usize = 7;

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen, Clone, Copy, PartialEq, Eq, Debug)]
pub enum TileType {
	Empty = 0,
	Home = 1,
	Grass = 2,
	Water = 3,
	Mountain = 4,
	Tree = 5,
	Desert = 6,
	Cave = 7,
}

pub const NUMBER_OF_TILE_TYPES: usize = 8;
pub const NUMBER_OF_LEVELS: usize = 4;

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen, Copy, Clone, PartialEq, Debug)]
pub struct ResourceAmount {
	pub resource_type: ResourceType,
	pub amount: ResourceUnit,
}

impl From<u8> for TileType {
	fn from(value: u8) -> Self {
		match value {
			1 => TileType::Home,
			2 => TileType::Grass,
			3 => TileType::Water,
			4 => TileType::Mountain,
			5 => TileType::Tree,
			6 => TileType::Desert,
			7 => TileType::Cave,
			_ => TileType::Empty,
		}
	}
}

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen, Clone, Copy, PartialEq, Eq, Debug)]
pub enum TilePattern {
	Normal = 0,
	Delta = 1,
	Line = 2,
	Ypsilon = 3,
}

pub const NUMBER_OF_PATTERNS: usize = 8;

impl From<u8> for TilePattern {
	fn from(value: u8) -> Self {
		match value {
			1 => TilePattern::Delta,
			2 => TilePattern::Line,
			3 => TilePattern::Ypsilon,
			_ => TilePattern::Normal,
		}
	}
}

#[derive(Encode, TypeInfo)]
pub struct ResourceProductions {
	pub produces: [ResourceUnit; NUMBER_OF_RESOURCE_TYPES],
	pub human_requirements: [ResourceUnit; NUMBER_OF_RESOURCE_TYPES],
}

#[derive(Encode, Decode, TypeInfo, MaxEncodedLen, Copy, Clone, PartialEq)]
#[scale_info(skip_type_params(T))]
pub struct TileCost<Tile> {
	pub tile_to_buy: Tile,
	pub cost: ResourceAmount,
}

#[derive(Encode, Decode, TypeInfo, PartialEq, Clone, Debug)]
pub struct Move {
	pub place_index: u8,
	pub buy_index: u8,
}

// The board hex grid
pub type HexGrid<Tile, N> = BoundedVec<Tile, N>;

// The board of the player, with all stats and resources
#[derive(Encode, Decode, TypeInfo, MaxEncodedLen)]
pub struct HexBoard<Tile, MaxGridSize> {
	pub resources: [ResourceUnit; NUMBER_OF_RESOURCE_TYPES],
	pub hex_grid: HexGrid<Tile, MaxGridSize>, // Board with all tiles
	pub game_id: GameId,                      // Game key
}

impl<Tile, MaxGridSize> HexBoard<Tile, MaxGridSize>
where
	Tile: Default + Clone + GetTileInfo,
	MaxGridSize: Get<u32>,
{
	pub fn try_new<DefaultPlayerResources>(
		size: usize,
		game_id: GameId,
	) -> Result<HexBoard<Tile, MaxGridSize>, ()>
	where
		DefaultPlayerResources: Get<[ResourceUnit; 7]>,
	{
		let mut new_hex_grid: HexGrid<Tile, MaxGridSize> =
			vec![Default::default(); size].try_into().map_err(|_| ())?;

		new_hex_grid[size / 2] = Tile::get_home();

		Ok(HexBoard::<Tile, MaxGridSize> {
			resources: DefaultPlayerResources::get(),
			hex_grid: new_hex_grid,
			game_id,
		})
	}

	pub fn get_stats(&self) -> BoardStats {
		let mut stats = BoardStats::default();

		for tile in self.hex_grid.clone() {
			let tile_type = tile.get_type();
			stats.set_tiles(tile_type, stats.get_tiles(tile_type).saturating_add(1));

			stats.set_levels(
				tile_type,
				tile.get_level(),
				stats.get_levels(tile_type, tile.get_level() as usize).saturating_add(1),
			);

			stats.set_patterns(
				tile_type,
				tile.get_pattern(),
				stats.get_patterns(tile_type, tile.get_pattern()).saturating_add(1),
			);
		}

		stats
	}
}

pub struct BoardStats {
	tiles: [u8; NUMBER_OF_TILE_TYPES],
	levels: [u8; NUMBER_OF_TILE_TYPES * NUMBER_OF_LEVELS],
	patterns: [u8; NUMBER_OF_TILE_TYPES * NUMBER_OF_PATTERNS],
}

impl Default for BoardStats {
	fn default() -> Self {
		Self {
			tiles: [0; NUMBER_OF_TILE_TYPES],
			levels: [0; NUMBER_OF_TILE_TYPES * NUMBER_OF_LEVELS],
			patterns: [0; NUMBER_OF_TILE_TYPES * NUMBER_OF_PATTERNS],
		}
	}
}

impl BoardStats {
	pub fn get_tiles(&self, tile_type: TileType) -> u8 {
		self.tiles[tile_type as usize]
	}

	pub fn get_tiles_by_tile_index(&self, tile_type_index: usize) -> u8 {
		self.tiles[tile_type_index]
	}

	pub fn set_tiles(&mut self, tile_type: TileType, value: u8) -> () {
		self.tiles[tile_type as usize] = value;
	}

	pub fn get_levels(&self, tile_type: TileType, level: usize) -> u8 {
		self.levels[(tile_type as usize).saturating_mul(NUMBER_OF_LEVELS).saturating_add(level)]
	}

	pub fn set_levels(&mut self, tile_type: TileType, level: u8, value: u8) -> () {
		self.levels[(tile_type as usize)
			.saturating_mul(NUMBER_OF_LEVELS)
			.saturating_add(level as usize)] = value;
	}

	pub fn get_patterns(&self, tile_type: TileType, pattern: TilePattern) -> u8 {
		self.patterns[(tile_type as usize)
			.saturating_mul(NUMBER_OF_PATTERNS)
			.saturating_add(pattern as usize)]
	}

	pub fn set_patterns(&mut self, tile_type: TileType, pattern: TilePattern, value: u8) -> () {
		self.patterns[(tile_type as usize)
			.saturating_mul(NUMBER_OF_PATTERNS)
			.saturating_add(pattern as usize)] = value;
	}
}
