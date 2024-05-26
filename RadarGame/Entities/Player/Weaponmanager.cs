using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class Weaponmanager: IEntitie ,IDrawObject
{
    private List<Weapon> weapons = new List<Weapon>();
    private float energy = 0;
    private float maxEnergy = 100;
    private float energyRegen = 1;
    private Weapon currentWeapon = null;
    
    private TexturedRectangle Iconframe = new TexturedRectangle(  new Texture("resources/Player/IconFrame.png") , true);
    private TexturedRectangle IconframeSelected = new TexturedRectangle(  new Texture("resources/Player/IconselectedFrame.png") , true);
    private TexturedRectangle IconframeBackground = new TexturedRectangle(  new Texture("resources/Player/IconBackground.png") , true);
     Vector2 iconSize = new Vector2(80,80);
    Vector2 iconPosition = new Vector2(1920/2 - 80*1.5f,100);
   
    public string Name { get; set; }
    
    public Weaponmanager()
    {
        Name = "Weaponmanager";
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        currentWeapon = weapons[0];
        
        
        
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
        
        if (keyboardState.IsKeyDown(Keys.D1))
        {
            currentWeapon = weapons[0];
        }
        if (keyboardState.IsKeyDown(Keys.D2))
        {
            currentWeapon = weapons[1];
        }
        if (keyboardState.IsKeyDown(Keys.D3))
        {
            currentWeapon = weapons[2];
        }
        
        
        
    }

    public void onDeleted()
    {
        //throw new NotImplementedException();
    }
    
    private void drawIcons(View surface)
    {
        int i = 0;
        float space = 1.5F;
        foreach (var wepon in weapons)
        {
            Iconframe.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X * space, 0);
            Iconframe.drawInfo.Size = iconSize;
            IconframeSelected.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X* space , 0);
            IconframeSelected.drawInfo.Size = iconSize;
            IconframeBackground.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X* space, 0);
            IconframeBackground.drawInfo.Size = iconSize;
            
            surface.Draw(IconframeBackground);
           
            if (wepon == currentWeapon)
            {
                surface.Draw(IconframeSelected);
            }
            else
            {
                surface.Draw(Iconframe);
            }
            i++;
            
        }
        
    }


    public void Draw(List<View> surface)
    {
       drawIcons( surface[2]);
    }
}