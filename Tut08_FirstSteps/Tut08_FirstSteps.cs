using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.Scene;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;
using Fusee.Engine.GUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuseeApp
{
    [FuseeApplication(Name = "Tut08_FirstSteps", Description = "Yet another FUSEE App.")]
    public class Tut08_FirstSteps : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private Transform _cubeTransform;  
        private Transform _cubeTransform2;
        private Transform _cubeTransform3;
        private float _camAngle = 0;

        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.87f, 0.55f, 0.47f, 1);

            // Create a scene with a cube
            //The three components: one XForm, one Material and the Mesh
            //cube 1
            _cubeTransform = new Transform {Scale = new float3(2, 1, 1)};
            var cubeShader = ShaderCodeBuilder.MakeShaderEffect(new float4 (1, 0, 0, 1));
            var cubeMesh = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            //cube 2
            _cubeTransform2 = new Transform {Scale = new float3(0.5f, 0.5f, 0.5f)};
            var cubeShader2 = ShaderCodeBuilder.MakeShaderEffect(new float4 (0.87f, 0.87f, 0.87f, 1));
            var cubeMesh2 = SimpleMeshes.CreateCuboid(new float3(10, 10, 10));

            //cube 3
            _cubeTransform3 = new Transform {Scale = new float3(1, 1, 1)};
            var cubeShader3 = ShaderCodeBuilder.MakeShaderEffect(new float4 (0, 1, 1, 1));
            var cubeMesh3 = SimpleMeshes.CreateCuboid(new float3(10, 5, 10));

            // Assemble the cube node containing the three components
            // cube 1
            var cubeNode = new SceneNode();
            cubeNode.Components.Add(_cubeTransform);
            cubeNode.Components.Add(cubeShader);
            cubeNode.Components.Add(cubeMesh);

            //cube 2
            var cubeNode2 = new SceneNode();
            cubeNode2.Components.Add(_cubeTransform2);
            cubeNode2.Components.Add(cubeShader2);
            cubeNode2.Components.Add(cubeMesh2);

            //cube 3
            var cubeNode3 = new SceneNode();
            cubeNode3.Components.Add(_cubeTransform3);
            cubeNode3.Components.Add(cubeShader3);
            cubeNode3.Components.Add(cubeMesh3);

            // Create the scene containing the cube as the only object
            _scene = new SceneContainer();
            _scene.Children.Add(cubeNode);
            _scene.Children.Add(cubeNode2);
            _scene.Children.Add(cubeNode3);

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            SetProjectionAndViewport();

                // Clear the backbuffer
                RC.Clear(ClearFlags.Color | ClearFlags.Depth);

                // Animate the camera angle
                _camAngle = _camAngle + 90.0f * M.Pi/180.0f * DeltaTime;

                // Setup the camera 
                RC.View = float4x4.CreateTranslation(0, 0, 50) * float4x4.CreateRotationY(_camAngle);

                 // Animate the cube
                 //ort
                _cubeTransform.Translation = new float3(0, 1 * M.Sin(2 * TimeSinceStart), 0);
                _cubeTransform2.Translation = new float3(0, 5 * M.Cos(2 * TimeSinceStart), 10);
                _cubeTransform3.Translation = new float3(0, 5 * M.Sin(2 * TimeSinceStart), 20);

                //skale 
                _cubeTransform.Scale = new float3(1, M.Cos(2 * TimeSinceStart), 1);

                //Rotation
                _cubeTransform.Rotation = new float3(M.Cos(2 * TimeSinceStart), 1, 1);

                // Render the scene on the current render context
                _sceneRenderer.Render(RC);

                // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
                Present();
        }

        public void SetProjectionAndViewport()
        {
            // Set the rendering area to the entire window size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }        

    }
}