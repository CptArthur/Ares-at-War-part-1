﻿using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleSyncManager {
	public static class SyncManager {

		public static ushort NetworkId = 42017;

		public static void SendNetworkData(SyncContainer container, ulong target = 0, bool sendToServer = false, bool sendToOthers = false) {

			var containerData = MyAPIGateway.Utilities.SerializeToBinary<SyncContainer>(container);

			if (target > 0)
				MyAPIGateway.Multiplayer.SendMessageTo(NetworkId, containerData, target);

			if (sendToServer)
				MyAPIGateway.Multiplayer.SendMessageToServer(NetworkId, containerData);

			if (sendToOthers)
				MyAPIGateway.Multiplayer.SendMessageToOthers(NetworkId, containerData);

		}

		public static void ReceivedNetworkData(ushort networkId, byte[] receivedData, ulong senderSteamId, bool isArrivedFromServer) {

			var container = MyAPIGateway.Utilities.SerializeFromBinary<SyncContainer>(receivedData);

			if (container == null || container.Sender != "AaW")
				return;

			if (container.Mode == SyncMode.None)
				return;

			if (container.Mode == SyncMode.ChatMessage)
				ChatManager.ServerChatProcessing(container, senderSteamId);

			if (container.Mode == SyncMode.Clipboard)
				ClientProcessing.ProcessClipboard(container);

		}

	}
}
