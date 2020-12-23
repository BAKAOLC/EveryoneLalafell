using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Actors;
using Dalamud.Game.ClientState.Actors.Types;
using Dalamud.Game.ClientState.Actors.Types.NonPlayer;
using Dalamud.Game.Command;
using Dalamud.Game.Internal;
using Dalamud.Plugin;
using EveryoneLalafell.Utils;
using EveryoneLalafell.Windows;
using System;
using System.Linq;

namespace EveryoneLalafell
{
    public class EveryoneLalafellPlugin : IDalamudPlugin
	{
		public string Name => "EveryoneLalafell";
		private static string PluginCommandName => "/eollf";
		private static string ModelInfoCommand => "/targetmodel";
		private static string MyModelInfoCommand => "/mymodel";

		private int _ticks;

		public static readonly string[] RaceList = new string[] {
			"人族",
			"精灵族",
			"拉拉菲尔族",
			"猫魅族",
			"鲁加族",
			"敖龙族",
			"硌狮族",
			"维埃拉族"
		};

		private DalamudPluginInterface _pluginInterface;
		private EveryoneLalafellConfiguration _pluginConfig;
		private bool _drawConfig;
		public bool enabled;
		public int _race;

		public bool enabled2;
		private TargetModelParameters _targetModelConfig;
		private bool _drawTargetModelConfig;
		public Actor _target;

		private PlaceholderResolver _placeholderResolver;

		private Action<string> Print => s => _pluginInterface?.Framework.Gui.Chat.Print(s);

		public void Initialize(DalamudPluginInterface pluginInterface)
		{
			_pluginInterface = pluginInterface;
			_pluginConfig = pluginInterface.GetPluginConfig() as EveryoneLalafellConfiguration ?? new EveryoneLalafellConfiguration();
			_pluginConfig.Init(this, pluginInterface);

			enabled = _pluginConfig.Enabled;
			_race = _pluginConfig.Race;

			_targetModelConfig = new TargetModelParameters();
			_targetModelConfig.Init(this, pluginInterface);

			pluginInterface.ClientState.OnLogout += (s, e) => enabled = false;
			pluginInterface.ClientState.OnLogout += (s, e) => enabled2 = false;
			pluginInterface.ClientState.OnLogin += (s, e) => enabled = _pluginConfig.Enabled;
			pluginInterface.ClientState.OnLogin += (s, e) => enabled2 = true;

			_pluginInterface.CommandManager.AddHandler(PluginCommandName, new CommandInfo(PluginCommand)
			{
				HelpMessage = $"打开 EveryoneLalafell 插件的设置界面.",
				ShowInHelp = true
			});

			_pluginInterface.CommandManager.AddHandler(ModelInfoCommand, new CommandInfo(OpenModelParameterWindows)
			{
				HelpMessage = $"打开模型参数修改页面.",
				ShowInHelp = true
			});

			_pluginInterface.CommandManager.AddHandler(MyModelInfoCommand, new CommandInfo(OpenModelParameterWindowsForMyself)
			{
				HelpMessage = $"打开自己的模型参数修改页面.",
				ShowInHelp = true
			});

			_placeholderResolver = new PlaceholderResolver();
			_placeholderResolver.Init(pluginInterface);

			_pluginInterface.UiBuilder.OnBuildUi += BuildUi;
			_pluginInterface.UiBuilder.OnBuildUi += BuildTargetModelUi;
			_pluginInterface.UiBuilder.OnOpenConfigUi += OpenConfigUi;
			_pluginInterface.Framework.OnUpdateEvent += OnUpdateEvent;
		}

		public void Dispose()
		{
			_pluginInterface.UiBuilder.OnBuildUi -= BuildUi;
			_pluginInterface.UiBuilder.OnOpenConfigUi -= OpenConfigUi;
			_pluginInterface.Framework.OnUpdateEvent -= OnUpdateEvent;
			_pluginInterface.CommandManager.RemoveHandler(PluginCommandName);
		}

		private void PluginCommand(string command, string arguments)
		{
			if (string.IsNullOrEmpty(arguments))
			{
				_drawConfig = !_drawConfig;
			}
		}

		private void OpenConfigUi(object sender, EventArgs eventArgs)
		{
			_drawConfig = !_drawConfig;
		}

		private void BuildUi()
		{
			_drawConfig = _drawConfig && _pluginConfig.DrawConfigUi();
		}

		private void BuildTargetModelUi()
		{
			_drawTargetModelConfig = _drawTargetModelConfig && _targetModelConfig.DrawUi();
		}

		private void OnUpdateEvent(Framework framework)
		{
			if (_target == null || _target.ActorId == 0)
				_target = _pluginInterface.ClientState.LocalPlayer;

			if (_pluginInterface.ClientState.Condition[ConditionFlag.BetweenAreas]
				|| _pluginInterface.ClientState.Condition[ConditionFlag.BetweenAreas51]
				|| _ticks + 20 > Environment.TickCount)
			{
				return;
			}
			_ticks = Environment.TickCount & int.MaxValue;

			if (enabled)
			{
				var actorTable = _pluginInterface.ClientState.Actors
					.Where(x => x.ObjectKind == ObjectKind.Player && x is PlayerCharacter pc
					&& pc.ActorId != _pluginInterface.ClientState.LocalPlayer?.ActorId
					&& pc.HomeWorld.Id != ushort.MaxValue
					&& pc.CurrentWorld.Id != ushort.MaxValue).ToArray();

				var race = _race + 1;
				foreach (PlayerCharacter pc in actorTable)
				{
					if (pc.Customize[0] != race)
					{
						//CustomizeIndex.Race
						pc.SetActorData(0, (byte)race);
						//CustomizeIndex.Tribe
						pc.SetActorData(4, (byte)((race * 2) - (pc.Customize[4] % 2)));
						pc.Rerender();
					}
				}
			}

			if (enabled2 && _targetModelConfig.AutoChangeCharacter && _drawTargetModelConfig)
			{
				if (_pluginInterface.ClientState.Targets.CurrentTarget is PlayerCharacter actor
					&& actor != null && actor.ActorId != 0 && actor.ActorId != _target?.ActorId)
				{
					_target = actor;
					_targetModelConfig.Update();
				}
				/*
				if (_pluginInterface.ClientState.Actors
				   .SingleOrDefault(x => x is PlayerCharacter
										 && x.ActorId != 0
										 && x.ActorId != _pluginInterface.ClientState.LocalPlayer?.ActorId
										 && x.ActorId == _pluginInterface.ClientState.LocalPlayer?.TargetActorID) is PlayerCharacter actor && actor.ActorId != _target.ActorId)
				{
					_target = actor;
					_targetModelConfig.Update();
				}
				*/
			}
		}

		public void OpenModelParameterWindows(string command, string arguments)
		{
			/*
			if (_pluginInterface.ClientState.Actors
				.SingleOrDefault(x => x is PlayerCharacter
									  && x.ActorId != 0
									  && x.ActorId != _pluginInterface.ClientState.LocalPlayer?.ActorId
									  && x.ActorId == _pluginInterface.ClientState.LocalPlayer?.TargetActorID) is PlayerCharacter actor)
			{
				_target = actor;
				_targetModelConfig.Update();
			}
			*/
			if (_pluginInterface.ClientState.Targets.CurrentTarget is PlayerCharacter actor
				&& actor != null && actor.ActorId != 0)
			{
				_target = actor;
				_targetModelConfig.Update();
			}
			_drawTargetModelConfig = true;
		}

		public void OpenModelParameterWindowsForMyself(string command, string arguments)
		{
			_target = _pluginInterface.ClientState.LocalPlayer;
			_targetModelConfig.Update();
			_drawTargetModelConfig = true;
		}

		public void RefreshActors()
		{
			var actors = from actor in _pluginInterface.ClientState.Actors
						 where !(actor is BattleNpc)
						 && actor.ActorId != _pluginInterface.ClientState.LocalPlayer?.ActorId
						 select actor;

			var battleActors = from actor in _pluginInterface.ClientState.Actors
							   where actor is BattleNpc npc
							   && npc.BattleNpcKind != BattleNpcSubKind.Enemy
							   && npc.OwnerId != _pluginInterface.ClientState.LocalPlayer?.ActorId
							   select actor as BattleNpc;

			foreach (var actor in actors)
			{
				actor.Rerender();
			}

			foreach (var actor in battleActors)
			{
				actor.Rerender();
			}
		}
	}
}
