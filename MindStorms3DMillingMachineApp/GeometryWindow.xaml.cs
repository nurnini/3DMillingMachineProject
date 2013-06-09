using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using MillingMachineCoreDll;
using MillingMachineGeometryParserDll;

namespace MillingMachineApplication
{
    /// <summary>
    /// Interaction logic for GeometryWindow.xaml
    /// </summary>
    public partial class GeometryWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        private GeometryModel3D geometryModel;
        private int degree = 90;

       

        public GeometryWindow()
        {
            InitializeComponent();

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }

        private void rotateHead()
        {
            degree += 30;
            RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = new Vector3D(0, 3, 0);
            myAxisAngleRotation3d.Angle = degree;
            myRotateTransform3D.Rotation = myAxisAngleRotation3d;
            geometryModel.Transform = myRotateTransform3D;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            rotateHead();
        }

        public void Basic3DShapeExample()
        {
            // Declare scene objects.
            Viewport3D myViewport3D = new Viewport3D();
            Model3DGroup myModel3DGroup = new Model3DGroup();
            GeometryModel3D myGeometryModel = new GeometryModel3D();
            ModelVisual3D myModelVisual3D = new ModelVisual3D();
            // Defines the camera used to view the 3D object. In order to view the 3D object,
            // the camera must be positioned and pointed such that the object is within view 
            // of the camera.
            PerspectiveCamera myPCamera = new PerspectiveCamera();

            // Specify where in the 3D scene the camera is.
            myPCamera.Position = new Point3D(0, 0, 2);

            // Specify the direction that the camera is pointing.
            myPCamera.LookDirection = new Vector3D(0, 0, -1);

            // Define camera's horizontal field of view in degrees.
            myPCamera.FieldOfView = 60;

            // Asign the camera to the viewport
            myViewport3D.Camera = myPCamera;
            // Define the lights cast in the scene. Without light, the 3D object cannot 
            // be seen. Note: to illuminate an object from additional directions, create 
            // additional lights.
            DirectionalLight myDirectionalLight = new DirectionalLight();
            myDirectionalLight.Color = Colors.White;
            myDirectionalLight.Direction = new Vector3D(-0.61, -0.5, -0.61);

            myModel3DGroup.Children.Add(myDirectionalLight);

            // The geometry specifes the shape of the 3D plane. In this sample, a flat sheet 
            // is created.
            MeshGeometry3D myMeshGeometry3D = new MeshGeometry3D();

            // Create a collection of normal vectors for the MeshGeometry3D.
            /*  Vector3DCollection myNormalCollection = new Vector3DCollection();
              myNormalCollection.Add(new Vector3D(0,0,1));
              myNormalCollection.Add(new Vector3D(0,0,1));
              myNormalCollection.Add(new Vector3D(0,0,1));
              myNormalCollection.Add(new Vector3D(0,0,1));
              myNormalCollection.Add(new Vector3D(0,0,1));
              myNormalCollection.Add(new Vector3D(0,0,1));
              myMeshGeometry3D.Normals = myNormalCollection; */

            // Create a collection of vertex positions for the MeshGeometry3D. 
            Point3DCollection myPositionCollection = new Point3DCollection();

            MillingMachineManager myMillingMachineManager = new MillingMachineManager(AppDomain.CurrentDomain.BaseDirectory + @"\piramid.obj");



            foreach (Vertex v in myMillingMachineManager.GetNormalisedVertexList())
            {
                myPositionCollection.Add(new Point3D(v.x, v.y, v.z));
            }


            myMeshGeometry3D.Positions = myPositionCollection;

            /*// Create a collection of texture coordinates for the MeshGeometry3D.
            PointCollection myTextureCoordinatesCollection = new PointCollection();
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 0));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(1, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 1));
            myTextureCoordinatesCollection.Add(new Point(0, 0));
            myMeshGeometry3D.TextureCoordinates = myTextureCoordinatesCollection; */

            // Create a collection of triangle indices for the MeshGeometry3D.
            Int32Collection myTriangleIndicesCollection = new Int32Collection();

            foreach (Face f in myMillingMachineManager.ObjectGeomertyParser.FaceList)
            {
                myTriangleIndicesCollection.Add(f.firstIndex - 1);
                myTriangleIndicesCollection.Add(f.secondIndex - 1);
                myTriangleIndicesCollection.Add(f.thirdIndex - 1);

                // myTriangleIndicesCollection.Add(f.firstIndex);

                // myTriangleIndicesCollection.Add(f.thirdIndex);
                // myTriangleIndicesCollection.Add(f.fourthIndex);

            }

            /*myTriangleIndicesCollection.Add(0);
            myTriangleIndicesCollection.Add(1);
            myTriangleIndicesCollection.Add(2);
            myTriangleIndicesCollection.Add(3);
            myTriangleIndicesCollection.Add(4);
            myTriangleIndicesCollection.Add(5); */
            myMeshGeometry3D.TriangleIndices = myTriangleIndicesCollection;

            // Apply the mesh to the geometry model.
            myGeometryModel.Geometry = myMeshGeometry3D;

            // The material specifies the material applied to the 3D object. In this sample a  
            // linear gradient covers the surface of the 3D object.

            // Create a horizontal linear gradient with four stops.   
            LinearGradientBrush myHorizontalGradient = new LinearGradientBrush();
            myHorizontalGradient.StartPoint = new Point(0, 0.5);
            myHorizontalGradient.EndPoint = new Point(1, 0.5);
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            // Define material and apply to the mesh geometries.
            DiffuseMaterial myMaterial = new DiffuseMaterial(myHorizontalGradient);
            myGeometryModel.Material = myMaterial;

            // Apply a transform to the object. In this sample, a rotation transform is applied,  
            // rendering the 3D object rotated.
            RotateTransform3D myRotateTransform3D = new RotateTransform3D();
            AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
            myAxisAngleRotation3d.Axis = new Vector3D(0, 3, 0);
            myAxisAngleRotation3d.Angle = 90;
            myRotateTransform3D.Rotation = myAxisAngleRotation3d;
            myGeometryModel.Transform = myRotateTransform3D;

            // Add the geometry model to the model group.
            myModel3DGroup.Children.Add(myGeometryModel);

            geometryModel = myGeometryModel;

            // Add the group of models to the ModelVisual3d.
            myModelVisual3D.Content = myModel3DGroup;

            // 
            myViewport3D.Children.Add(myModelVisual3D);

            // Apply the viewport to the page so it will be rendered.
            this.Content = myViewport3D;

            dispatcherTimer.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Basic3DShapeExample();
        }
    }
}
