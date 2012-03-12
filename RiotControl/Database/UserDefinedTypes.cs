﻿using System;
using System.Collections.Generic;

namespace RiotControl
{
	enum RegionType
	{
		NorthAmerica,
		EuropeWest,
		EuropeNordicEast,
	}

	enum MapType
	{
		TwistedTreeline,
		SummonersRift,
		Dominion,
	}

	enum GameModeType
	{
		Custom,
		Normal,
		Bot,
		Solo,
		Premade,
	}

	static class UserDefinedTypes
	{
		static Dictionary<string, RegionType> RegionTypeDictionary = new Dictionary<string, RegionType>()
		{
			{"north_america", RegionType.NorthAmerica},
			{"europe_west", RegionType.EuropeWest},
			{"europe_nordic_east", RegionType.EuropeNordicEast},
		};

		static Dictionary<string, MapType> MapTypeDictionary = new Dictionary<string, MapType>()
		{
			{"twisted_treeline", MapType.TwistedTreeline},
			{"summoners_rift", MapType.SummonersRift},
			{"dominion", MapType.Dominion},
		};

		static Dictionary<string, GameModeType> GameModeTypeDictionary = new Dictionary<string, GameModeType>()
		{
			{"custom", GameModeType.Custom},
			{"normal", GameModeType.Normal},
			{"bot", GameModeType.Bot},
			{"solo", GameModeType.Solo},
			{"premade", GameModeType.Premade},
		};


		static EnumType ToEnumType<EnumType>(this string input, Dictionary<string, EnumType> EnumDictionary)
		{
			EnumType output;
			if (EnumDictionary.TryGetValue(input, out output))
				return output;
			else
				throw new Exception(string.Format("Unknown enum type: {0}", input));
		}

		public static RegionType ToRegionType(this string input)
		{
			return input.ToEnumType<RegionType>(RegionTypeDictionary);
		}

		public static MapType ToMapType(this string input)
		{
			return input.ToEnumType<MapType>(MapTypeDictionary);
		}

		public static GameModeType ToGameModeType(this string input)
		{
			return input.ToEnumType<GameModeType>(GameModeTypeDictionary);
		}
	}
}