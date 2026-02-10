using StarMap.API;
using ModMenu;
using Brutal.ImGuiApi;
using KSA;
using Brutal.Numerics;
using Brutal.Logging;

namespace Space_Stations_and_Colonies
{
    [StarMapMod]
    public class SSaCMod
    {
        public static int i = 1;

        [ModMenuEntry("Space Stations and Colonies")]
        public static void Menu()
        {
            Vehicle? currentV = Program.ControlledVehicle;
            if (ImGui.Button("Clone Vehicle"))
            {
                if (currentV != null)
                {
                    Part newRoot = new Part("", currentV.Parts.Root.Template);
                    Vehicle newVehicle = Vehicle.CreateVehicle(currentV.System, currentV.Body2Cce, currentV.BodyRates, currentV.Parent, "test", newRoot, currentV.Orbit);
                    newVehicle.UpdatePerFrameData();
                    //newVehicle.SetName($"Copy of Vehicle {i}");
                    i++;
                    Universe.CurrentSystem!.Vehicles.Register(newVehicle);
                    DefaultCategory.Log.Info("Cloned Vehicle");
                } else
                {
                    DefaultCategory.Log.Error($"Current System {currentV.System != null}, Template {(VehicleTemplate)currentV.BodyTemplate != null}, Parent {currentV.Parent != null}");
                }
            }
        }
    }
}
