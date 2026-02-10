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
                    // Creates a true clone of the Vehicle's Part Tree
                    PartTree newPartTree = currentV.Parts.Clone();

                    // Refills the tanks to full. Does NOT preserve old tank state
                    newPartTree.RefillConsumables();

                    // There is still an error copying this Vehicle, figure out why
                    Vehicle newVehicle = Vehicle.CreateVehicle(currentV.System, currentV.Body2Cce, currentV.BodyRates, currentV.Parent, $"{currentV.Id}_Clone{i}", newPartTree.Root, currentV.Orbit);
                    newVehicle.UpdatePerFrameData();
                    i++;
                    Universe.CurrentSystem!.Vehicles.Register(newVehicle);
                    DefaultCategory.Log.Info($"Cloned Vehicle {currentV.Id}");
                }
            }
        }
    }
}
