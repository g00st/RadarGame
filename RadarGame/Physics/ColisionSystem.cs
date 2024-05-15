using OpenTK.Mathematics;

namespace RadarGame.Physics;

public static class ColisionSystem
{
    private static List<ColisionData> _colisionData = new List<ColisionData>();
    private struct ColisionData
    {
        public IColisionObject O { get; set; }
        public float Distance { get; set; }
         
    }

    public static void AddObject(IColisionObject colisionObject)
    {
        ColisionData newColisionData = new ColisionData();
        newColisionData.O = colisionObject;
        newColisionData.Distance = colisionObject.CollisonShape.Max(x => x.Length);
        _colisionData.Add(newColisionData);
    }
    public static void RemoveObject(IColisionObject colisionObject)
    {
        _colisionData.RemoveAll(x => x.O == colisionObject);
    }
    
    public static void Update()
    {
       
            
            
        for (int i = 0; i < _colisionData.Count; i++)
        {
            for (int j = i+1; j < _colisionData.Count; j++)
            { 
                //if the objects are the same, skip
                if (_colisionData[i].O == _colisionData[j].O)
                {
                    continue;
                }
                //if both objects are static, skip
                if (_colisionData[i].O.Static && _colisionData[j].O.Static)
                {
                    continue;
                }
                //if the distance between the objects is greater than the distance between the objects farthest points, skip
                
                if (_colisionData[i].Distance + _colisionData[j].Distance < Vector2.Distance(_colisionData[i].O.Position, _colisionData[j].O.Position))
                {
                    continue;
                }
                
                
                if (SAT(_colisionData[i].O, _colisionData[j].O))
                {
                    _colisionData[i].O.OnColision(_colisionData[j].O);
                    _colisionData[j].O.OnColision(_colisionData[i].O);
                }
            }
        }
    }

    private static bool SAT( IColisionObject A,  IColisionObject B)
    {
        var VerticesA = A.CollisonShape.Select(x => x + A.Position).ToList();
        var VerticesB = B.CollisonShape.Select(x => x + B.Position).ToList();
        
        for (int i = 0; i < VerticesA.Count; i++)
        {
            var edge = VerticesA[(i + 1) % VerticesA.Count] - VerticesA[i];
            var normal = new Vector2(-edge.Y, edge.X).Normalized();
            var minA = float.MaxValue;
            var maxA = float.MinValue;
            var minB = float.MaxValue;
            var maxB = float.MinValue;
            foreach (var vertex in VerticesA)
            {
                var projection = Vector2.Dot(vertex, normal);
                minA = Math.Min(minA, projection);
                maxA = Math.Max(maxA, projection);
            }
            foreach (var vertex in VerticesB)
            {
                var projection = Vector2.Dot(vertex, normal);
                minB = Math.Min(minB, projection);
                maxB = Math.Max(maxB, projection);
            }
            if (maxA < minB || maxB < minA)
            {
                return false;
            }
        }
        for (int i = 0; i < VerticesB.Count; i++)
        {
            var edge = VerticesB[(i + 1) % VerticesB.Count] - VerticesB[i];
            var normal = new Vector2(-edge.Y, edge.X).Normalized();
            var minA = float.MaxValue;
            var maxA = float.MinValue;
            var minB = float.MaxValue;
            var maxB = float.MinValue;
            foreach (var vertex in VerticesA)
            {
                var projection = Vector2.Dot(vertex, normal);
                minA = Math.Min(minA, projection);
                maxA = Math.Max(maxA, projection);
            }
            foreach (var vertex in VerticesB)
            {
                var projection = Vector2.Dot(vertex, normal);
                minB = Math.Min(minB, projection);
                maxB = Math.Max(maxB, projection);
            }
            if (maxA < minB || maxB < minA)
            {
                return false;
            }
        }
        
        return true;
    }





}