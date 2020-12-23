using Dalamud.Game;
using Dalamud.Game.Internal;
using System;

namespace EveryoneLalafell.Utils
{
    internal class AddressResolver : BaseAddressResolver
	{
		public IntPtr ResolvePlaceholderText { get; private set; }

		protected override void Setup64Bit(SigScanner sig)
		{
			ResolvePlaceholderText = sig.ScanText("E8 ?? ?? ?? ?? 48 8B 5C 24 ?? EB 0C");
		}
	}
}
