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
    [FuseeApplication(Name = "Tut09_HierarchyAndInput", Description = "Yet another FUSEE App.")]
    public class Tut09_HierarchyAndInput : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRendererForward _sceneRenderer;
        private float _camAngle = 0;
        private Transform _baseTransform;
        private Transform _bodyTransform;
        private Transform _upperArmTransform;
        private Transform _foreArmTransform;
        private Transform _kralleoben;
        private Transform _kralleunten;


       SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new Transform
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

            _bodyTransform = new Transform {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 6, 0)
            };

            _upperArmTransform = new Transform{
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 4, 0)
            };

            _foreArmTransform = new Transform {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-2, 4, 0)
            };

            _kralleoben = new Transform {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(1, 5, 0)
            };

             _kralleunten = new Transform {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(-1, 5, 0)
            };

            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNode>
                {
                    // Graue Basis
                    new SceneNode
                    {
                        Components = new List<SceneComponent>
                        {
                            // TRANSFORM COMPONENT
                            _baseTransform,

                            // SHADER EFFECT COMPONENT
                            ShaderCodeBuilder.MakeShaderEffect(new float4 (0.8f, 0.8f, 0.8f, 1)),

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },
                    //Roter Körper
                    new SceneNode {
                        Components = new List<SceneComponent> 
                        {
                            _bodyTransform,

                            ShaderCodeBuilder.MakeShaderEffect(new float4 (1, 0, 0, 1)),

                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                        },
                        Children = new ChildList {

                        //grün Oberarm
                        new SceneNode {
                            Components = new List<SceneComponent> {

                                _upperArmTransform,
                            },
                            Children = new ChildList {
                                new SceneNode {

                                    Components = new List<SceneComponent>{

                                    new Transform {
                                        Rotation = new float3(0, 0, 0),
                                        Scale = new float3(1, 1, 1),
                                        Translation = new float3(0, 4, 0)
                                    },
                                    ShaderCodeBuilder.MakeShaderEffect(new float4(0, 1, 0, 1)),
                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))

                                    },
                                    Children = new ChildList {
                                        // blauer Unterarm
                                        new SceneNode {
                                            Components = new List<SceneComponent> {

                                                _foreArmTransform,

                                                new Transform {
                                                    Rotation = new float3(0, 0, 0),
                                                    Scale = new float3(1, 1, 1),
                                                    Translation = new float3(0, 4, 0)
                                                },
                                                ShaderCodeBuilder.MakeShaderEffect(new float4(0, 0, 1, 1)),
                                                SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                            },
                                            //Hand oben
                                            Children = new ChildList {
                                                new SceneNode {
                                                    Components = new List<SceneComponent> {
                                                        _kralleoben,
                                                        },
                                                        Children = new ChildList {
                                                            new SceneNode {
                                                                Components = new List<SceneComponent> {
                                                                    new Transform {
                                                                    Rotation = new float3(0, 0, 0),
                                                                    Scale = new float3(1, 1, 1),
                                                                    Translation = new float3(0, 1, 0) 
                                                                },
                                                                ShaderCodeBuilder.MakeShaderEffect(new float4(0, 1, 1, 1)),
                                                                SimpleMeshes.CreateCuboid(new float3(1, 2, 1))
                                                            }
                                                        }                  
                                                    }
                                                }, 
                                                  // Hand unten
                                                    new SceneNode {
                                                        Components = new List<SceneComponent> {
                                                            _kralleunten,
                                                         }, 

                                                         Children = new ChildList {
                                                             new SceneNode {
                                                                 Components = new List<SceneComponent> {
                                                                        new Transform {
                                                                        Rotation = new float3(0, 0, 0),
                                                                        Scale = new float3(1, 1, 1),
                                                                        Translation = new float3(0, 1, 0)
                                                                 },
                                                                 
                                                                ShaderCodeBuilder.MakeShaderEffect(new float4(0, 1, 1, 1)),
                                                                SimpleMeshes.CreateCuboid(new float3(1, 2, 1))
                                                             }
                                                         }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                     }
                 }
            }
        };
    }


        // Init is called on startup. 
        public override void Init()
        {
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.8f, 0.9f, 0.7f, 1);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRendererForward(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Steuerung
            float bodyRot = _bodyTransform.Rotation.y;
            bodyRot += 2 * Keyboard.LeftRightAxis * DeltaTime;
            _bodyTransform.Rotation = new float3(0, bodyRot, 0);

            float upperArm = _upperArmTransform.Rotation.x;
            upperArm += 2 * Keyboard.UpDownAxis * DeltaTime;
            _upperArmTransform.Rotation = new float3(upperArm, 0, 0);

            float underArm = _foreArmTransform.Rotation.x;
            underArm += 2 * Keyboard.WSAxis * DeltaTime;
            _foreArmTransform.Rotation = new float3(underArm, 0, 0);

            // Kamera 
            if (Mouse.LeftButton) {
            _camAngle += 0.01f * Mouse.Velocity.x * DeltaTime; 
            }

            // Kralle unten
            float kralleunten = _kralleunten.Rotation.z;
            kralleunten += 2 * Keyboard.ADAxis * DeltaTime;
            
            // Kralle oben
            float kralleoben = _kralleoben.Rotation.z;
            kralleoben += -2* Keyboard.ADAxis * DeltaTime;

            //Krallen beschränken
            if( kralleoben < 0.206 & kralleoben > -1.509){
                _kralleunten.Rotation = new float3(0, 0, kralleunten);
                _kralleoben.Rotation = new float3(0,0, kralleoben); 
            }

            SetProjectionAndViewport();
            
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_camAngle);

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