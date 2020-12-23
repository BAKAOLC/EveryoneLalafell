using Dalamud.Game.ClientState.Actors.Types;
using Dalamud.Plugin;
using ImGuiNET;
using System.Numerics;

namespace EveryoneLalafell.Windows
{
    public class TargetModelParameters
	{
		private EveryoneLalafellPlugin _plugin;
		private DalamudPluginInterface _pluginInterface;

		public void Init(EveryoneLalafellPlugin plugin, DalamudPluginInterface pluginInterface)
		{
			_plugin = plugin;
			_pluginInterface = pluginInterface;
		}

		public void Update()
		{
			var data = (_plugin._target as PlayerCharacter).Customize;
			Race = data[0] - 1;
			Gender = data[1];
			ModelType = data[2];
			Height = data[3];
			Tribe = (data[4] - 1) % 2;
			FaceType = data[5];
			HairStyle = data[6];
			HasHighlights = data[7];
			SkinColor = data[8];
			EyeColor = data[9];
			HairColor = data[10];
			HairColor2 = data[11];
			FaceFeatures = data[12];
			FaceFeaturesColor = data[13];
			Eyebrows = data[14];
			EyeColor2 = data[15];
			EyeShape = data[16];
			NoseShape = data[17];
			JawShape = data[18];
			LipStyle = data[19];
			LipColor = data[20];
			RaceFeatureSize = data[21];
			RaceFeatureType = data[22];
			BustSize = data[23];
			Facepaint = data[24];
			FacepaintColor = data[25];
		}

		public void Enable()
		{
			if (_plugin._target != null)
			{
				var race = Race + 1;
				_plugin._target.SetActorData(0, (byte)race);
				_plugin._target.SetActorData(1, (byte)Gender);
				_plugin._target.SetActorData(2, (byte)ModelType);
				_plugin._target.SetActorData(3, (byte)Height);
				_plugin._target.SetActorData(4, (byte)((race * 2) - ((Tribe + 1) % 2)));
				_plugin._target.SetActorData(5, (byte)FaceType);
				_plugin._target.SetActorData(6, (byte)HairStyle);
				_plugin._target.SetActorData(7, (byte)HasHighlights);
				_plugin._target.SetActorData(8, (byte)SkinColor);
				_plugin._target.SetActorData(9, (byte)EyeColor);
				_plugin._target.SetActorData(10, (byte)HairColor);
				_plugin._target.SetActorData(11, (byte)HairColor2);
				_plugin._target.SetActorData(12, (byte)FaceFeatures);
				_plugin._target.SetActorData(13, (byte)FaceFeaturesColor);
				_plugin._target.SetActorData(14, (byte)Eyebrows);
				_plugin._target.SetActorData(15, (byte)EyeColor2);
				_plugin._target.SetActorData(16, (byte)EyeShape);
				_plugin._target.SetActorData(17, (byte)NoseShape);
				_plugin._target.SetActorData(18, (byte)JawShape);
				_plugin._target.SetActorData(19, (byte)LipStyle);
				_plugin._target.SetActorData(20, (byte)LipColor);
				_plugin._target.SetActorData(21, (byte)RaceFeatureSize);
				_plugin._target.SetActorData(22, (byte)RaceFeatureType);
				_plugin._target.SetActorData(23, (byte)BustSize);
				_plugin._target.SetActorData(24, (byte)Facepaint);
				_plugin._target.SetActorData(25, (byte)FacepaintColor);
				_plugin._target.Rerender();
			}
		}

		public int Race = 0;
		public int Gender = 0;
		public int ModelType = 0;
		public int Height = 0;
		public int Tribe = 0;
		public int FaceType = 0;
		public int HairStyle = 0;
		public int HasHighlights = 0;
		public int SkinColor = 0;
		public int EyeColor = 0;
		public int HairColor = 0;
		public int HairColor2 = 0;
		public int FaceFeatures = 0;
		public int FaceFeaturesColor = 0;
		public int Eyebrows = 0;
		public int EyeColor2 = 0;
		public int EyeShape = 0;
		public int NoseShape = 0;
		public int JawShape = 0;
		public int LipStyle = 0;
		public int LipColor = 0;
		public int RaceFeatureSize = 0;
		public int RaceFeatureType = 0;
		public int BustSize = 0;
		public int Facepaint = 0;
		public int FacepaintColor = 0;

		private static readonly string[] _gender = { "男", "女" };
		private static readonly string[] _tribe = { "Type A", "Type B" };

		public bool AutoChangeCharacter = false;

		public bool DrawUi()
		{
			var draw = true;
			var scale = ImGui.GetIO().FontGlobalScale;
			ImGui.SetNextWindowSize(new Vector2(400 * scale, 820 * scale), ImGuiCond.Always);
			ImGui.Begin("模型设置", ref draw, ImGuiWindowFlags.NoResize);

			ImGui.Text("自动切换目标角色：");
			ImGui.SameLine();
			ImGui.Checkbox($"###{nameof(AutoChangeCharacter)}", ref AutoChangeCharacter);
			ImGui.SameLine();
			ImGui.Text("当前设置角色：" + _plugin._target?.Name);
			ImGui.Separator();

			ImGui.Text("种族");
			ImGui.SameLine();
			ImGui.Combo($"###{nameof(Race)}", ref Race, EveryoneLalafellPlugin.RaceList, EveryoneLalafellPlugin.RaceList.Length);
			ImGui.Text("（设置为硌狮族或维埃拉族时可能出现较多的异常）");

			ImGui.Text("性别");
			ImGui.SameLine();
			ImGui.Combo($"###{nameof(Gender)}", ref Gender, _gender, _gender.Length);

			ImGui.Text("ModelType");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(ModelType)}", ref ModelType);

			ImGui.Text("部落");
			ImGui.SameLine();
			ImGui.Combo($"###{nameof(Tribe)}", ref Tribe, _tribe, _tribe.Length);

			ImGui.Text("身高");
			ImGui.SameLine();
			ImGui.SliderInt($"###{nameof(Height)}", ref Height, 0, 100);

			ImGui.Text("HairStyle");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(HairStyle)}", ref HairStyle);

			ImGui.Text("HairColor");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(HairColor)}", ref HairColor);

			ImGui.Text("HairColor2");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(HairColor2)}", ref HairColor2);

			ImGui.Text("HasHighlights");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(HasHighlights)}", ref HasHighlights);

			ImGui.Text("FaceType");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(FaceType)}", ref FaceType);

			ImGui.Text("FaceFeatures");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(FaceFeatures)}", ref FaceFeatures);

			ImGui.Text("FaceFeaturesColor");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(FaceFeaturesColor)}", ref FaceFeaturesColor);

			ImGui.Text("SkinColor");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(SkinColor)}", ref SkinColor);

			ImGui.Text("Eyebrows");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(Eyebrows)}", ref Eyebrows);

			ImGui.Text("EyeShape");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(EyeShape)}", ref EyeShape);

			ImGui.Text("EyeColor");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(EyeColor)}", ref EyeColor);

			ImGui.Text("EyeColor2");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(EyeColor2)}", ref EyeColor2);

			ImGui.Text("NoseShape");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(NoseShape)}", ref NoseShape);

			ImGui.Text("JawShape");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(JawShape)}", ref JawShape);

			ImGui.Text("LipStyle");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(LipStyle)}", ref LipStyle);

			ImGui.Text("LipColor");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(LipColor)}", ref LipColor);

			ImGui.Text("RaceFeatureSize");
			ImGui.SameLine();
			ImGui.SliderInt($"###{nameof(RaceFeatureSize)}", ref RaceFeatureSize, 0, 100);

			ImGui.Text("RaceFeatureType");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(RaceFeatureType)}", ref RaceFeatureType);

			ImGui.Text("BustSize");
			ImGui.SameLine();
			ImGui.SliderInt($"###{nameof(BustSize)}", ref BustSize, 0, 100);

			ImGui.Text("Facepaint");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(Facepaint)}", ref Facepaint);

			ImGui.Text("FacepaintColor");
			ImGui.SameLine();
			ImGui.InputInt($"###{nameof(FacepaintColor)}", ref FacepaintColor);

			if (ImGui.Button("应用"))
				Enable();
			ImGui.SameLine();
			if (ImGui.Button("刷新"))
				Update();

			ImGui.End();
			return draw;
		}
	}
}
