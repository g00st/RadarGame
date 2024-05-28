using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Entities.Weapons;
using RadarGame.others;

namespace RadarGame.Entities;

public class Weaponmanager: IEntitie ,IDrawObject
{
    private List<Weapon> weapons = new List<Weapon>();
    private float energy = 0;
    private float maxEnergy = 100;
    private float energyRegen = 1;
    private Weapon currentWeapon = null;
    private ColoredRectangle cooldownBar = new ColoredRectangle( Vector2.Zero , new Vector2(10,10),  new Color4(0,0,0.2f,0.4f), "cooldownBar" , true);
    private TexturedRectangle Iconframe = new TexturedRectangle(  new Texture("resources/Player/IconFrame.png") , true);
    private TexturedRectangle IconframeSelected = new TexturedRectangle(  new Texture("resources/Player/IconselectedFrame.png") , true);
    private TexturedRectangle IconframeBackground = new TexturedRectangle(  new Texture("resources/Player/IconBackground.png") , true);
     Vector2 iconSize = new Vector2(80,80);
    Vector2 iconPosition = new Vector2(1920/2 - 80*1.5f,100);
   
    public string Name { get; set; }
    
    public Weaponmanager()
    {
        Name = "Weaponmanager";
        weapons.Add(new Machineguns());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        weapons.Add(new Weapon());
        currentWeapon = weapons[0];
        weapons[2].cooldown = 1f;

        
             EntityManager.AddObject(weapons[0]);
             iconPosition = new Vector2(1920/2 - 80* weapons.Count/2.0f,100);
        
    }

    public void addEnergy()
    {
        //todo add energy
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        
        for (int i = 0; i < weapons.Count; i++)
        {
            if (keyboardState.IsKeyDown(Keys.D1 + i))
            {
                currentWeapon = weapons[i];
            }
        }
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            if (currentWeapon.state == Weapon.Weponstate.ready)
            {
                currentWeapon.fire();
            }
           
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
            
            cooldownBar.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X * space, 0);
            Iconframe.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X * space, 0);
            Iconframe.drawInfo.Size = iconSize;
            IconframeSelected.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X* space , 0);
            IconframeSelected.drawInfo.Size = iconSize * 1.1f;
            IconframeBackground.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X* space, 0);
            IconframeBackground.drawInfo.Size = iconSize;
            
            surface.Draw(IconframeBackground);
           if (wepon.state == Weapon.Weponstate.coolingdown)
           {
                           
               cooldownBar.drawInfo.Size = iconSize * new Vector2(wepon.getColdownPercent());
               surface.Draw(cooldownBar);
                           
           }

           if (wepon.icon != null)
           {
                wepon.icon.drawInfo.Position = iconPosition + new Vector2(i * iconSize.X * space, 0);
                wepon.icon.drawInfo.Size = iconSize;
                surface.Draw(wepon.icon);
           }
           
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
       PercentageBar.DrawBar( surface[1], new Vector2(100, 100), 1, 1, new Color4(0,0,1f,1f)); 
       drawIcons( surface[2]);
    }
}