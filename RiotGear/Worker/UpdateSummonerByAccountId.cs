﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using System.Data.Common;

using LibOfLegends;

using com.riotgames.platform.statistics;
using com.riotgames.platform.summoner;
using com.riotgames.platform.gameclient.domain;

namespace RiotGear
{
	public partial class Worker
	{
		public OperationResult UpdateSummonerByAccountId(int accountId)
		{
			if (!Connected)
				return OperationResult.NotConnected;

			WriteLine("Updating account {0}", accountId);
			ConcurrentRPC concurrentRPC = new ConcurrentRPC(RPC, accountId);
			OperationResult result = concurrentRPC.Run();
			if (result == OperationResult.Success)
			{
				if (concurrentRPC.PublicSummonerData == null)
				{
					//This means that the summoner was not found, even though the other structures are actually non-null
					return OperationResult.NotFound;
				}
				Summoner newSummoner = new Summoner(concurrentRPC.PublicSummonerData, Region);
				Summoner summoner = StatisticsService.GetSummoner(Region, accountId);
				if (summoner == null)
				{
					//The summoner wasn't in the database yet, add them
					using (var connection = Provider.GetConnection())
						InsertNewSummoner(newSummoner, connection);
					summoner = newSummoner;
				}
				else
				{
					//Copy data that might have been changed
					summoner.SummonerName = newSummoner.SummonerName;
					summoner.InternalName = newSummoner.InternalName;

					summoner.SummonerLevel = newSummoner.SummonerLevel;
					summoner.ProfileIcon = newSummoner.ProfileIcon;
				}
				//Perform a full update
				using (var connection = Provider.GetConnection())
					UpdateSummoner(summoner, concurrentRPC.PublicSummonerData, concurrentRPC.AggregatedStatistics, concurrentRPC.LifeTimeStatistics, concurrentRPC.RecentGameData, connection);
				return OperationResult.Success;
			}
			return result;
		}
	}
}
