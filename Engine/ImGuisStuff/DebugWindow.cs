using ImGuiNET;

namespace App.Engine.ImGuisStuff;

public class DebugWindow
{
    List<IDebugable> _debugables = new List<IDebugable>();
    
    public void AddDebugable(IDebugable debugable)
    {
        _debugables.Add(debugable);
    }
    
public void RemoveDebugable(IDebugable debugable)
    {
        _debugables.Remove(debugable);
    }
    
    public static void Draw()
    {
        ImGui.Begin("Debug Window");
        ImGui.Text("Hello, world!");
        ImGui.End();
    }
}