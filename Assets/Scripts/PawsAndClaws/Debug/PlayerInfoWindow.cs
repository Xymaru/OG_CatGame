using ImGuiNET;

namespace PawsAndClaws.Debugging
{
    public class PlayerInfoWindow : DebugWindow
    {
        public override void OnImGuiRenderer()
        {
            ImGui.Begin("Player Info");

            ImGui.Text($"Name: {Networking.NetworkData.PublicName}");

            ImGui.Separator();

            ImGui.Text($"Team: {GameManager.Instance.playerTeam}");
            ImGui.Text($"Current State: {GameManager.Instance.playerManager.GetCurrentStateName()}");

            if(ImGui.CollapsingHeader("Stats"))
            {
                var playerStats = GameManager.Instance.playerManager.CharacterStats;
                ImGui.Text($"Health: {playerStats.Health}/{playerStats.MaxHealth}");
                ImGui.Text($"Health regen: {playerStats.HealthRegen} * {playerStats.HealthRegenMultiplier}/s");
                ImGui.Text($"Mana: {playerStats.Mana}/{playerStats.MaxMana}");
                ImGui.Text($"Mana regen: {playerStats.ManaRegen} * {playerStats.ManaRegenMultiplier}/s");
                ImGui.Text($"Lvl: {playerStats.Level} XP: {playerStats.Experience}/{playerStats.ExpToNextLevel}");
                ImGui.Text($"Shield: {playerStats.Shield} * {playerStats.ShieldMultiplier}");
                ImGui.Text($"Dmg: {playerStats.Damage} * {playerStats.DamageMultiplier}");

            }

            ImGui.End();
        }
    }
}