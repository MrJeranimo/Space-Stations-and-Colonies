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

                    // Stores the original Center of Mass and Bounding Sphere Radius before the new Part Tree is recomputed, as these values will be used to adjust the new Vehicle's position and scale after cloning
                    double3 centerOfMassAsmb = currentV.CenterOfMassAsmb;
                    float boundingSphereRadius = currentV.BoundingSphereRadius;
                    currentV.UpdateAfterPartTreeModification();

                    // There is still an error copying this Vehicle, figure out why
                    Vehicle newVehicle = Vehicle.CreateVehicle(currentV.System, currentV.Body2Cce, currentV.BodyRates, currentV.Parent, $"{currentV.Id}_Clone{i}", newPartTree.Root, currentV.Orbit);

                    // Refills tanks, but only to NepetalactoneActinidine_2 :(
                    newVehicle.RefillConsumables();

                    // Recomputes all derived data for the new Part Tree, such as mass, rocket controls, etc.
                    newPartTree.RecomputeAllDerivedData();

                    StateVectors stateVectors = currentV.Orbit.StateVectors;

                    double3 double3_2 = (newVehicle.CenterOfMassAsmb - centerOfMassAsmb).Transform(currentV.GetAsmb2Cci());

                    Orbit fromStateCci1 = Orbit.CreateFromStateCci(currentV.Parent, stateVectors.StateTime, stateVectors.PositionCci + double3_2, stateVectors.VelocityCci, currentV.OrbitColor);

                    newVehicle.Teleport(fromStateCci1, new doubleQuat?(), new double3?());

                    double3 double3_3 = (currentV.CenterOfMassAsmb - centerOfMassAsmb).Transform(currentV.GetAsmb2Cci());

                    Orbit fromStateCci2 = Orbit.CreateFromStateCci(currentV.Parent, stateVectors.StateTime, stateVectors.PositionCci + double3_3, stateVectors.VelocityCci, currentV.OrbitColor);

                    currentV.Teleport(fromStateCci2, new doubleQuat?(), new double3?());

                    currentV.OrbitView.DistancePower *= (double)boundingSphereRadius / (double)currentV.BoundingSphereRadius;

                    Program.MainViewport.OrbitController.DistancePower = currentV.OrbitView.DistancePower;

                    currentV.Parent.Children.Add((Astronomical)newVehicle);

                    newVehicle.UpdatePerFrameData();
                    i++;
                    Universe.CurrentSystem!.Vehicles.Register(newVehicle);
                    DefaultCategory.Log.Info($"Cloned Vehicle {currentV.Id}");
                }
            }
        }
    }
}
