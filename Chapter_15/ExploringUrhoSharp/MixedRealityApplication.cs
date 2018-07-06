#region Using

using System;
using Urho;
using Urho.Actions;
using Urho.Gui;
using Urho.Physics;
using Urho.Shapes;
using Urho.SharpReality;

#endregion

namespace ExploringUrhoSharp
{
    public class MixedRealityApplication : StereoApplication
    {
        #region Fields

        // Text
        private Text text;

        // Nodes
        private Node sphereNode;
        private Node pyramidNode;
        private Node boxNode;
        private Node planeNode;

        // Gaze
        private Node spatialCursorNode;
        private Node focusedNode;

        // Manipulation gesture
        private Node manipulatedHologram;
        private Vector3 previousHandPosition;

        #endregion

        #region Constructor

        public MixedRealityApplication(ApplicationOptions options) : base(options) { }

        #endregion

        #region Start and OnUpdate

        protected override async void Start()
        {
            base.Start();

            // Text
            CreateText("Hello, UrhoSharp!", Color.Magenta);

            // 3D primitives
            CreateSphere(Color.Red);
            CreatePyramid(Color.Green);
            CreateBox(Color.Blue);

            // Spatial cursor (gaze indicator)
            CreateSpatialCursor();

            // Gestures
            EnableGestureTapped = true;
            EnableGestureManipulation = true;

            // Physics
            CreatePlane(Color.Gray);

            AddCollisionShape(sphereNode);
            AddCollisionShape(pyramidNode);
            AddCollisionShape(boxNode);

            // Spatial mapping
            await StartSpatialMapping(new Vector3(5, 5, 5));
        }

        protected override void OnUpdate(float timeStep)
        {
            base.OnUpdate(timeStep);

            // Update text
            text.Value = $"Elapsed time: {Time.ElapsedTime:F1} s";
        }

        #endregion

        #region Gestures

        public override void OnGestureTapped()
        {
            base.OnGestureTapped();

            RaycastTest();

            if (focusedNode != null)
            {
                AddRigidBody(focusedNode, 1f);
            }
        }

        public override void OnGestureDoubleTapped()
        {
            base.OnGestureDoubleTapped();

            if (focusedNode != null)
            {
                var duration = 0.5f;

                var originalScale = focusedNode.Scale.X;
                var intermediateScale = originalScale * 1.5f;

                focusedNode.RunActions(
                    new ScaleTo(duration, intermediateScale),
                    new ScaleTo(duration, originalScale));
            }

            ThrowTheBall(10);
        }

        public override void OnGestureManipulationStarted()
        {
            base.OnGestureManipulationStarted();

            manipulatedHologram = focusedNode;
            previousHandPosition = Vector3.Zero;
        }

        public override void OnGestureManipulationUpdated(Vector3 relativeHandPosition)
        {
            base.OnGestureManipulationUpdated(relativeHandPosition);

            if (manipulatedHologram != null)
            {
                manipulatedHologram.Position += (relativeHandPosition - previousHandPosition);
                previousHandPosition = relativeHandPosition;
            }
        }

        public override void OnGestureManipulationCompleted(Vector3 relativeHandPosition)
        {
            base.OnGestureManipulationCompleted(relativeHandPosition);

            manipulatedHologram = null;
        }

        public override void OnGestureManipulationCanceled()
        {
            base.OnGestureManipulationCanceled();

            manipulatedHologram = null;
        }

        #endregion

        #region Helpers

        #region Text

        private void CreateText(string caption, Color color, float fontSize = 48.0f)
        {
            text = new Text()
            {
                Value = caption,
                HorizontalAlignment = HorizontalAlignment.Center,
                Position = new IntVector2(0, 50)
            };

            text.SetColor(color);
            text.SetFont(CoreAssets.Fonts.AnonymousPro, fontSize);

            UI.Root.AddChild(text);
        }

        #endregion

        #region Nodes

        private Node CreateNode(Vector3 position, float scale)
        {
            var node = new Node()
            {
                Position = position,
                Scale = scale * Vector3.One
            };

            Scene.AddChild(node);

            return node;
        }

        private Node CreateSceneChildNode(Vector3 position, float scale)
        {
            var node = Scene.CreateChild();

            node.Position = position;
            node.Scale = scale * Vector3.One;

            return node;
        }

        #region Spatial cursor

        private void CreateSpatialCursor()
        {
            // Create cursor node
            spatialCursorNode = Scene.CreateChild();

            // Add SpatialCursor component
            var spatialCursorComponent = spatialCursorNode.CreateComponent<SpatialCursor>();

            // Get the static model of the cursor
            var staticModel = spatialCursorComponent.CursorModelNode.GetComponent<StaticModel>();

            // ... and change its color from Cyan to Yellow
            staticModel.Material.SetShaderParameter("MatDiffColor", Color.Yellow);

            // Handle Raycasted event            
            spatialCursorComponent.Raycasted += SpatialCursorComponent_Raycasted;
        }

        private void SpatialCursorComponent_Raycasted(RayQueryResult? obj)
        {
            if (obj.HasValue)
            {
                if (focusedNode != obj.Value.Node)
                {
                    System.Diagnostics.Debug.WriteLine($"ID from the Spatial Cursor: {obj.Value.Node.ID}");
                }

                focusedNode = obj.Value.Node;
            }
            else
            {
                focusedNode = null;
            }
        }

        private void RaycastTest()
        {
            // Create ray
            var ray = LeftCamera.GetScreenRay(0.5f, 0.5f);

            // Send the ray towards the scene
            var raycastResult = Octree.RaycastSingle(ray);

            // Debug an information about the detected nodes
            var debugInfo = "Nothing detected";
            if (raycastResult.HasValue)
            {
                debugInfo = $"ID from the RaycastTest: {raycastResult.Value.Node.ID}";
            }

            System.Diagnostics.Debug.WriteLine(debugInfo);
        }

        #endregion

        #endregion

        #region Primitives

        private void CreateSphere(Color color)
        {
            sphereNode = CreateNode(new Vector3(0.15f, 0, 1f), 0.1f);

            // Add sphere component
            sphereNode.CreateComponent<Sphere>().Color = color;
        }

        private void CreatePyramid(Color color)
        {
            pyramidNode = CreateNode(new Vector3(-0.2f, 0, 1.25f), 0.1f);

            // Add pyramid component
            pyramidNode.CreateComponent<Pyramid>().Material = Material.FromColor(color);
        }

        private void CreateBox(Color color)
        {
            boxNode = CreateNode(new Vector3(0, -0.025f, 1f), 0.1f);

            // Add box component            
            boxNode.CreateComponent<Box>().Color = color;

            // Rotate the node
            boxNode.Rotate(Quaternion.FromAxisAngle(Vector3.Right, 30));
        }

        #endregion

        #region Physics

        private RigidBody AddRigidBody(
            Node node,
            float mass = 1,
            bool useGravity = true,
            bool isKinematic = false,
            float bounciness = 0.75f)
        {
            // Check, whether the RigidBody was already created
            var rigidBody = node.GetComponent<RigidBody>();

            if (rigidBody == null)
            {
                // If not, create the new one
                rigidBody = node.CreateComponent<RigidBody>();

                // Set the mass, gravity, kinematic, and restitution (bounciness)
                rigidBody.Mass = mass;
                rigidBody.UseGravity = useGravity;
                rigidBody.Kinematic = isKinematic;
                rigidBody.Restitution = bounciness;
            }

            return rigidBody;
        }

        private CollisionShape AddCollisionShape(Node node)
        {
            var shape = node.GetComponent<Shape>();

            if (shape == null)
            {
                return null;
            }
            else
            {
                return SetCollisionShape(node, shape);
            }
        }

        private CollisionShape SetCollisionShape(Node node, Shape shape)
        {
            var collisionShape = node.CreateComponent<CollisionShape>();

            var one = Vector3.One;
            var position = Vector3.Zero;
            var rotation = Quaternion.Identity;

            if (shape.GetType() == typeof(Sphere))
            {
                collisionShape.SetSphere(1, position, rotation);
            }
            else if (shape.GetType() == typeof(Box))
            {
                collisionShape.SetBox(one, position, rotation);
            }
            else if (shape.GetType() == typeof(Urho.Shapes.Plane))
            {
                var size = new Vector3(planeNode.Scale.X,
                    0.01f, planeNode.Scale.Z);

                collisionShape.SetBox(size, position, rotation);
            }
            else if (shape.GetType() == typeof(Pyramid))
            {
                collisionShape.SetConvexHull(CoreAssets.Models.Cone,
                    0, one, position, rotation);
            }

            return collisionShape;
        }

        private void CreatePlane(Color color)
        {
            planeNode = CreateNode(new Vector3(0, -0.5f, 1f), 1f);
            planeNode.CreateComponent<Urho.Shapes.Plane>().Color = color;

            AddRigidBody(planeNode, 10, false, true, 1);
            AddCollisionShape(planeNode);

            planeNode.NodeCollision += PlaneNode_NodeCollision;
        }

        private void PlaneNode_NodeCollision(NodeCollisionEventArgs obj)
        {
            var otherNode = obj.OtherNode;

            otherNode.RunActions(new TintBy(0.5f, Color.White));
        }

        private void ThrowTheBall(float speed)
        {
            // Create the new node
            var ballNode = CreateNode(HeadPosition, 0.1f);

            // Create the sphere component
            ballNode.CreateComponent<Sphere>().Color = Color.Yellow;

            // Configure physics
            var ballRigidBody = AddRigidBody(ballNode, 0.5f);
            AddCollisionShape(ballNode);

            // Throw the ball towards the gaze direction
            var ray = LeftCamera.GetScreenRay(0.5f, 0.5f);
            ballRigidBody.SetLinearVelocity(ray.Direction * speed);
        }

        #endregion

        #region Spatial mapping

        public override void OnSurfaceAddedOrUpdated(SpatialMeshInfo surface, Model generatedModel)
        {
            base.OnSurfaceAddedOrUpdated(surface, generatedModel);

            // Create the node for the spatial surface
            var surfaceNode = CreateNode(surface.BoundsCenter, 1.0f);
            surfaceNode.Rotation = surface.BoundsRotation;

            // Create and configure the static model component
            var staticModelComponent = surfaceNode.CreateComponent<StaticModel>();
            staticModelComponent.Model = generatedModel;
            staticModelComponent.ViewMask = 0x80000000;

            // Set the wireframe material for the model
            var material = Material.FromColor(Color.Gray);
            material.FillMode = FillMode.Wireframe;
            staticModelComponent.SetMaterial(material);

            // Add the rigid body
            AddRigidBody(surfaceNode, 10, false, true, 1);

            // Create and configure the collider
            var collisionShape = surfaceNode.CreateComponent<CollisionShape>();
            collisionShape.SetTriangleMesh(generatedModel, 0, Vector3.One,
                Vector3.Zero, Quaternion.Identity);
        }

        #endregion

        #endregion
    }
}