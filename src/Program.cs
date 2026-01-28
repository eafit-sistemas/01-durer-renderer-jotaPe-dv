using System;
using SkiaSharp;

public static class Program
{
    public static void Main()
    {
        InputData data = InputData.LoadFromJson("input.json");
        Shape2D projected = ProjectShape(data.Model);
        projected.Print (); // The tests check for the correct projected data to be printed
        Render(projected, data.Parameters, "output.jpg");
    }

    private static void Render(Shape2D shape, RenderParameters parameters, string outputPath)
    {
        int resolution = parameters.Resolution;
        int margin = 50; 
        int drawArea = resolution - 2 * margin;
        
        // Crear una superficie de dibujo
        using (var surface = SKSurface.Create(new SKImageInfo(resolution, resolution)))
        {
            SKCanvas canvas = surface.Canvas;
            
            // Limpiar el canvas con color blanco
            canvas.Clear(SKColors.White);

            // Configurar el pincel para dibujar
            using (var paint = new SKPaint())
            {
                paint.Color = SKColors.Black;
                paint.StrokeWidth = 3;
                paint.IsAntialias = true;
                paint.Style = SKPaintStyle.Stroke;

                // Calcular escalas para mapear de coordenadas del mundo a píxeles (con margen)
                float worldWidth = parameters.XMax - parameters.XMin;
                float worldHeight = parameters.YMax - parameters.YMin;
                float scaleX = drawArea / worldWidth;
                float scaleY = drawArea / worldHeight;

                // Dibujar las líneas (aristas)
                foreach (var line in shape.Lines)
                {
                    int startIdx = line[0];
                    int endIdx = line[1];

                    // Convertir coordenadas del mundo a coordenadas de píxeles
                    float x1 = (shape.Points[startIdx][0] - parameters.XMin) * scaleX + margin;
                    float y1 = (shape.Points[startIdx][1] - parameters.YMin) * scaleY + margin;
                    float x2 = (shape.Points[endIdx][0] - parameters.XMin) * scaleX + margin;
                    float y2 = (shape.Points[endIdx][1] - parameters.YMin) * scaleY + margin;

                    // Invertir Y porque en pantalla Y crece hacia abajo
                    y1 = resolution - y1;
                    y2 = resolution - y2;

                    canvas.DrawLine(x1, y1, x2, y2, paint);
                }

                // Dibujar los vértices como puntos más grandes
                paint.Style = SKPaintStyle.Fill;
                paint.StrokeWidth = 0;
                foreach (var point in shape.Points)
                {
                    float x = (point[0] - parameters.XMin) * scaleX + margin;
                    float y = (point[1] - parameters.YMin) * scaleY + margin;
                    
                    // Invertir Y
                    y = resolution - y;
                    
                    canvas.DrawCircle(x, y, 5, paint);
                }
            }

            // Guardar la imagen
            using (var image = surface.Snapshot())
            using (var data = image.Encode(SKEncodedImageFormat.Jpeg, 100))
            using (var stream = System.IO.File.OpenWrite(outputPath))
            {
                data.SaveTo(stream);
            }

            Console.WriteLine($"Imagen generada exitosamente: {outputPath}");
        }
    }

    private static Shape2D ProjectShape(Model3D model)
    {
        // Implementar proyección perspectiva (proyección de Dürer)
        // La proyección perspectiva simula cómo vemos los objetos en la realidad
        int vertexCount = model.VertexTable.Length;
        float[][] projectedPoints = new float[vertexCount][];

        // Parámetros de proyección perspectiva
        float distance = 5.0f; // Distancia del observador al plano de proyección
        
        for (int i = 0; i < vertexCount; i++)
        {
            float x = model.VertexTable[i][0];
            float y = model.VertexTable[i][1];
            float z = model.VertexTable[i][2];
            
            // Proyección perspectiva: factor = distancia / (distancia + z)
            float factor = distance / (distance + z);
            float x2d = x * factor;
            float y2d = y * factor;
            
            projectedPoints[i] = new float[] { x2d, y2d };
        }

        // Las aristas permanecen iguales
        return new Shape2D
        {
            Points = projectedPoints,
            Lines = model.EdgeTable
        };
    }
}

