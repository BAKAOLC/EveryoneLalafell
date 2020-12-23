using Dalamud.Configuration;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace EveryoneLalafell.Windows
{
    public class EveryoneLalafellConfiguration : IPluginConfiguration
	{
		public int Version { get; set; }
		public bool Enabled { get; set; } = false;
		public int Race { get; set; } = 2;

		[NonSerialized]
		private EveryoneLalafellPlugin _plugin;

		[NonSerialized]
		private DalamudPluginInterface _pluginInterface;

		[NonSerialized]
		public readonly Dictionary<string, Action<bool>> settingDictionary = new Dictionary<string, Action<bool>>();

		[NonSerialized]
		public readonly HashSet<ushort> territoryTypeWhitelist = new HashSet<ushort>
		{
			792,
			898,
			899
		};

		public void Init(EveryoneLalafellPlugin plugin, DalamudPluginInterface pluginInterface)
		{
			_plugin = plugin;
			_pluginInterface = pluginInterface;
		}

		public void Save()
		{
			_pluginInterface.SavePluginConfig(this);
		}

		public bool DrawConfigUi()
		{
			var drawConfig = true;
			var scale = ImGui.GetIO().FontGlobalScale;
			ImGui.SetNextWindowSize(new Vector2(375 * scale, 250 * scale), ImGuiCond.Always);
			ImGui.Begin($"{_plugin.Name} Config", ref drawConfig, ImGuiWindowFlags.NoResize);
			ImGui.Text("启用");
			ImGui.SameLine();
			if (ImGui.Checkbox($"###{nameof(_plugin.enabled)}", ref _plugin.enabled))
			{
				Enabled = _plugin.enabled;
				Save();
			}
			ImGui.Text("种族");
			ImGui.SameLine();
			if (ImGui.Combo($"###{nameof(_plugin._race)}", ref _plugin._race,
				EveryoneLalafellPlugin.RaceList, EveryoneLalafellPlugin.RaceList.Length))
			{
				Race = _plugin._race;
				Save();
			}
			ImGui.Text("（设置为硌狮族或维埃拉族时可能出现较多的异常）");
			ImGui.Separator();
			ImGui.Text("如果存在玩家模型没有成功应用设置");
			ImGui.Text("请点击刷新模型按钮刷新");
			ImGui.Text("注：如需恢复角色模型参数请先关闭功能");
			ImGui.Text("之后进行场景切换来重新加载并恢复角色模型数据");
			if (ImGui.Button("刷新模型"))
				_plugin.RefreshActors();
			ImGui.End();
			return drawConfig;
		}
	}
}
