using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
//TODO: Fix horrible code and mvp mess
namespace App.Engine;

public class View
{
    private List<DrawObject> drawObjects;
    public int  Width, Height;
    public Vector2 vpossition;
    public Vector2 vsize;
    public float rotation;
    public VBO _rendertarget;
   
    
   
    

    public void Resize(int width, int height )
    {
        Width = width;
        Height = height;
       
        GL.Viewport(0, 0, Width, Height);
    }
    

        public void Draw(DrawObject todraw)
        {
            GL.Viewport(0, 0, Width, Height);
            _rendertarget.Bind();
            Matrix4 camera =  calcCameraProjection();



            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            //statt liste an drawobjects dann eine liste an renderables
          
                DrawInfo obj = todraw.drawInfo;
       
                Matrix4 ObjectScalematrix = Matrix4.CreateScale(obj.Size.X,obj.Size.Y, 1.0f);
                Matrix4 ObjectRotaionmatrix = Matrix4.CreateRotationZ(obj.Rotation);
                Matrix4 ObjectTranslationmatrix = Matrix4.CreateTranslation(obj.Position.X,obj.Position.Y,0);

                Matrix4 objectransform = Matrix4.Identity * ObjectScalematrix;
                objectransform *= ObjectRotaionmatrix;
                objectransform *= ObjectTranslationmatrix;
            
                //Console.Write(objectransform.ToString() + "\n" +" \n");
            
               
                Matrix4 translateToOrigin = Matrix4.CreateTranslation(-vpossition.X, -vpossition.Y, 0);
                Matrix4 rotate = Matrix4.CreateRotationZ(rotation);
                Matrix4 translateBack = Matrix4.CreateTranslation(vpossition.X, vpossition.Y, 0);
                Matrix4 view = translateToOrigin * rotate * translateBack;

                Matrix4 projection = calcCameraProjection();
            
            
                Vector3 cameraRotationAxis = new Vector3(0, 0, 1);
                Matrix4 cameraRotationMatrix = Matrix4.CreateFromAxisAngle(cameraRotationAxis, MathHelper.DegreesToRadians(rotation));
                cameraRotationMatrix = Matrix4.CreateRotationZ(rotation);
                Matrix4 comb =   objectransform* Matrix4.CreateTranslation(-vpossition.X,-vpossition.Y,0) * cameraRotationMatrix *Matrix4.CreateTranslation(vpossition.X,vpossition.Y,0)  * camera;
                //prüfe was gamestate

                obj.mesh.Draw(comb, obj, view, projection);
                _rendertarget.Unbind();
        }
    

    private Matrix4 calcCameraProjection()
    {

        //Matrix4.CreateOrthographic(1920, 1080, -1.0f, 1.0f)* Matrix4.CreateTranslation(-1,-1,0);
        
        float left = vpossition.X - vsize.X / 2.0f;
        float right = vpossition.X + vsize.X / 2.0f;
        float bottom = vpossition.Y -  ((vsize.X/Width)*Height)/ 2.0f;
        float top = vpossition.Y +  ((vsize.X/Width)*Height)/ 2.0f;
        return  Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1.0f, 1.0f);

        
    }
    
    public Vector2 ScreenToViewSpace(Vector2 screenCoordinate)
    {
        float centerX = Width / 2.0f;
        float centerY = Height / 2.0f;
    
        // Center the screen coordinates around the center of the viewport
        float centeredX = screenCoordinate.X - centerX;
        float centeredY = centerY - screenCoordinate.Y; // Y-axis is inverted in screen coordinates
    
        // Normalize centered screen coordinates
        float normalizedX = (2.0f * centeredX / Width);
        float normalizedY = (2.0f * centeredY / Height);
    
        // Create the camera projection matrix
        Matrix4 projectionMatrix = calcCameraProjection();
    
        // Create the camera rotation matrix
        Matrix4 cameraRotationMatrix = Matrix4.CreateRotationZ(rotation);
    
        // Calculate the translation to move the camera position to the origin
        Matrix4 translateToOrigin = Matrix4.CreateTranslation(-vpossition.X, -vpossition.Y, 0);
    
        // Calculate the translation to move back to the original position after rotation
        Matrix4 translateBack = Matrix4.CreateTranslation(vpossition.X, vpossition.Y, 0);
    
        // Combine the translations, rotation, and projection matrices
        Matrix4 combinedMatrix = translateToOrigin * cameraRotationMatrix * translateBack * projectionMatrix;
    
        // Calculate the inverse of the combined matrix
        Matrix4 inverseMatrix = Matrix4.Invert(combinedMatrix);
    
        // Transform the screen coordinate to view space
        Vector4 viewSpace = new Vector4(normalizedX, normalizedY, 0, 1) * inverseMatrix;
    
        // Return the transformed coordinates
        return new Vector2(viewSpace.X, viewSpace.Y);
    }
    public View()
    {
        
        _rendertarget = VBO.VBO_0();
        vsize = new Vector2(1920 ,1080);
        vpossition = new Vector2(1920/2, 1080/2);
        drawObjects = new List<DrawObject>();
        rotation = 0;
    }
    

    
}