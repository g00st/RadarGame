using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class Weaponmanager: IEntitie
{
    private List<Weapon> weapons;
    private float energy;
    private float maxEnergy;
    private float energyRegen;
    private Weapon currentWeapon;
    public string Name { get; set; }
    
    public Weaponmanager()
    {
        Name = "Weaponmanager";
    }

    public void addEnergy()
    {
        //todo add energy
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        foreach (var weapon in weapons)
        {
            weapon.Update(args);
        }
        
        
        
    }

    public void onDeleted()
    {
        throw new NotImplementedException();
    }
}