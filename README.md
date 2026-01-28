[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/gYKPNcXQ)
# Durer Renderer Assignment

This repository is a template for an academic project where you will implement a basic perspective renderer using C# and SkiaSharp.

Your goal is to load 3D geometry from a JSON file, project it into 2D, print the intermediate results to the console, and generate a rendered PNG image.

---

## Your Task

You are given a base class that already:

- Loads 3D shapes (Cube and Pyramid) from a JSON file.
- Stores:
  - A vertex table (3D points)
  - An edge table (connections between vertices)
  - Viewport bounds and resolution

You must complete the implementation so that:

1. The 3D vertices are projected into 2D using perspective projection.
2. The projected points are printed to the console in the specified format.
3. The projected points are transformed into screen coordinates.
4. The screen coordinates are printed to the console.
5. A PNG image is generated showing the shape using SkiaSharp.

---

## Autograding

When you push your work, the autograder will verify that:

- The projected vertices are computed correctly.
- The screen-space transformation is correct.
- The console output matches the expected format.
- A valid PNG image file is generated.

---

## Files in This Repository

- `Solution.cs`: Main class you will edit.
- `durer_data.json`: Contains geometry and rendering parameters.