//! Benchmarking setup for pallet-parachain-template

use super::*;

#[allow(unused)]
use crate::Pallet as Template;
use frame_benchmarking::{benchmarks, impl_benchmark_test_suite, whitelisted_caller};
use frame_system::RawOrigin;
use sp_runtime::BoundedVec;

fn account<T: Config>(index: u32) -> T::AccountId {
	let seed = 0;
	let name = "";
	frame_benchmarking::account(name, index, seed)
}

benchmarks! {
	create_game {
		let n in 0 .. (T::MaxPlayers::get() - 1);

		let caller: T::AccountId = whitelisted_caller();
		let mut players = vec![caller.clone()];

		for pp in 0..n {
			players.push(account::<T>(pp));
		}

	}: _(RawOrigin::Signed(caller.clone()), players.clone(), 25)
	verify {
		let hex_board_option: Option<crate::HexBoard<T>> =
			HexBoardStorage::<T>::get(caller);

		let hex_board = hex_board_option.unwrap();

		assert_eq!(GameStorage::<T>::get(hex_board.game_id), Some(Game {
			state: GameState::Playing,
			selection_size: 2,
			round: 0,
			player_turn_and_played: 0,
			last_played_block: 1u8.into(),
			players: players.clone().try_into().unwrap(),
			selection: BoundedVec::<u8, T::MaxTileSelection>::try_from(vec![4, 9]).unwrap(),
		}));
	}

	/*play {
		let b in 0..1;
		let p in 0..16;

		let caller: T::AccountId = whitelisted_caller();
	}: _(RawOrigin::Signed(caller.clone()), Move { place_index: p as u8, buy_index: b as u8 })
	verify {

	}*/
	
	impl_benchmark_test_suite!(Template, crate::mock::new_test_ext(), crate::mock::TestRuntime);

}
