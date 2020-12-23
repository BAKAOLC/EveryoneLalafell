using Dalamud.Game.ClientState.Actors;
using Dalamud.Game.ClientState.Actors.Types;
using Dalamud.Plugin;
using EveryoneLalafell.Utils;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EveryoneLalafell
{
    public static class Extensions
	{
		public static bool IsStatus(this Actor actor, StatusFlags flag)
		{
			return (Marshal.ReadByte(actor.Address + 0x1906) & (byte) flag) > 0;
		}

		public static string GetFirstname(this Actor actor)
		{
			return actor.Name.Split(' ')[0];
		}

		public static string GetLastname(this Actor actor)
		{
			return actor.Name.Split(' ')[1];
		}

		public static string ByteToString(this byte[] arr)
		{
			return Encoding.Default.GetString(arr).Replace("\0", string.Empty);
		}

		public static string ToUppercase(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return string.Empty;
			}
			
			var arr = str.ToCharArray();
			arr[0] = char.ToUpper(arr[0]);
			return new string(arr);
		}

		public static void SetActorData(this Actor a, int offset, byte value)
		{
			try
			{
				Marshal.WriteByte(a.Address + Definitions.ActorAppearance + offset, value);
			}
			catch (Exception ex)
			{
#if DEBUG
				PluginLog.LogError(ex.ToString());
#endif
			}
		}

		public static void Render(this Actor a)
		{
			try
			{
				var addrRenderToggle = a.Address + Definitions.ActorRender;
				var renderToggle = Marshal.ReadInt32(addrRenderToggle);
				if ((renderToggle & (int)EveryoneLalafellFlags.Invisible) != (int)EveryoneLalafellFlags.Invisible &&
					(a.ObjectKind != ObjectKind.MountType ||
					 (renderToggle & (int)EveryoneLalafellFlags.Unknown15) != (int)EveryoneLalafellFlags.Unknown15)) return;
				renderToggle &= ~(int)EveryoneLalafellFlags.Invisible;
				Marshal.WriteInt32(addrRenderToggle, renderToggle);
			}
			catch (Exception ex)
			{
#if DEBUG
				PluginLog.LogError(ex.ToString());
#endif
			}
		}

		public static void Hide(this Actor a)
		{
			try
			{
				var addrRenderToggle = a.Address + Definitions.ActorRender;
				var renderToggle = Marshal.ReadInt32(addrRenderToggle);

				renderToggle |= (int)EveryoneLalafellFlags.Invisible;
				Marshal.WriteInt32(addrRenderToggle, renderToggle);
			}
			catch (Exception ex)
			{
#if DEBUG
				PluginLog.LogError(ex.ToString());
#endif
			}
		}

		public static async void Rerender(this Actor a)
		{
			await Task.Run(async () =>
			{
				try
				{
					Marshal.WriteByte(a.Address + Definitions.ActorRender, 2);
					await Task.Delay(50);
					Marshal.WriteByte(a.Address + Definitions.ActorRender, 0);
				}
				catch (Exception ex)
				{
#if DEBUG
					PluginLog.LogError(ex.ToString());
#endif
				}
			});
		}
	}
}
