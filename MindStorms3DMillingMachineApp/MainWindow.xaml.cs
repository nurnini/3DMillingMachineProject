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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Media3D;
using MillingMachineGeometryParserDll;
using MillingMachineCoreDll;
using AForge.Robotics.Lego;
using System.Threading;

namespace MillingMachineApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    


    public partial class MainWindow : Window
    {
        #region Motor and Sensor Related Configuration Member Variables

        private const int BLUE_COLOR_SENSOR_STATE_INDEX = 16;
        // NXT brick
        private NXTBrick nxt = new NXTBrick();
        // rugulation modes
        private NXTBrick.MotorRegulationMode[] regulationModes = new NXTBrick.MotorRegulationMode[] {
            NXTBrick.MotorRegulationMode.Idle,
            NXTBrick.MotorRegulationMode.Speed,
            NXTBrick.MotorRegulationMode.Sync };
        // run states
        private NXTBrick.MotorRunState[] runStates = new NXTBrick.MotorRunState[] {
            NXTBrick.MotorRunState.Idle,
            NXTBrick.MotorRunState.RampUp,
            NXTBrick.MotorRunState.Running,
            NXTBrick.MotorRunState.RampDown };
        // sensor types
        private NXTBrick.SensorType[] sensorTypes = new NXTBrick.SensorType[] {
            NXTBrick.SensorType.NoSensor, NXTBrick.SensorType.Switch,
            NXTBrick.SensorType.Temperature, NXTBrick.SensorType.Reflection,
            NXTBrick.SensorType.Angle, NXTBrick.SensorType.LightActive,
            NXTBrick.SensorType.LightInactive, NXTBrick.SensorType.SoundDB,
            NXTBrick.SensorType.SoundDBA, NXTBrick.SensorType.Custom,
            NXTBrick.SensorType.Lowspeed, NXTBrick.SensorType.Lowspeed9V,
            NXTBrick.SensorType.Highspeed, NXTBrick.SensorType.ColorFull, NXTBrick.SensorType.ColorRed,
            NXTBrick.SensorType.ColorGreen, NXTBrick.SensorType.ColorBlue, NXTBrick.SensorType.ColorNone, NXTBrick.SensorType.ColorExit};
        // sensor modes
        private NXTBrick.SensorMode[] sensorModes = new NXTBrick.SensorMode[] {
            NXTBrick.SensorMode.Raw, NXTBrick.SensorMode.Boolean,
            NXTBrick.SensorMode.TransitionCounter, NXTBrick.SensorMode.PeriodicCounter,
            NXTBrick.SensorMode.PCTFullScale, NXTBrick.SensorMode.Celsius,
            NXTBrick.SensorMode.Fahrenheit, NXTBrick.SensorMode.AngleSteps };

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            motorCombo.SelectedIndex = 0;
            regulationModeCombo.SelectedIndex = 0;
            runStateCombo.SelectedIndex = 2;
            inputPortCombo.SelectedIndex = 0;
            sensorTypeCombo.SelectedIndex = 0;
            sensorModeCombo.SelectedIndex = 0;

        }


        #region Connection Related Methods

        // Collect information about Lego NXT brick
        private void CollectInformation()
        {
            // ------------------------------------------------
            // get NXT version
            string firmwareVersion;
            string protocolVersion;

            if (nxt.GetVersion(out protocolVersion, out firmwareVersion))
            {
                firmwareBox.Text = firmwareVersion;
                protocolBox.Text = protocolVersion;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed getting verion");
            }

            // ------------------------------------------------
            // get device information
            string deviceName;
            byte[] btAddress;
            int btSignalStrength;
            int freeUserFlesh;

            if (nxt.GetDeviceInformation(out deviceName, out btAddress, out btSignalStrength, out freeUserFlesh))
            {
                deviceNameBox.Text = deviceName;

                btAddressBox.Text = string.Format("{0} {1} {2} {3} {4} {5} {6}",
                    btAddress[0].ToString("X2"),
                    btAddress[1].ToString("X2"),
                    btAddress[2].ToString("X2"),
                    btAddress[3].ToString("X2"),
                    btAddress[4].ToString("X2"),
                    btAddress[5].ToString("X2"),
                    btAddress[6].ToString("X2")
                );

                btSignalStrengthBox.Text = btSignalStrength.ToString();
                freeUserFlashBox.Text = freeUserFlesh.ToString();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed getting device information");
            }


            // ------------------------------------------------
            // get battery level
            int batteryLevel;

            if (nxt.GetBatteryPower(out batteryLevel))
            {
                batteryLevelBox.Text = batteryLevel.ToString();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed getting battery level");
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (nxt.Connect(portBox.Text))
            {
                System.Diagnostics.Debug.WriteLine("Connected successfully");

                CollectInformation();

                // enable controls
                resetMotorButton.IsEnabled = true;
                setMotorStateButton.IsEnabled = true;
                getMotorStateButton.IsEnabled = true;
                getInputButton.IsEnabled = true;
                setInputModeButton.IsEnabled = true;

                connectButton.IsEnabled = false;
                disconnectButton.IsEnabled = true;

                nxt.PlayTone(100, 200, false);
                displayTxt.Text = "";
            }
            else
            {
                displayTxt.Text = "Failed connecting to NXT device";
            }
        }

        private void disconnectButton_Click(object sender, RoutedEventArgs e)
        {
             // On "Disconnect" button click

            nxt.Disconnect( );

            // clear information
            firmwareBox.Text = string.Empty;
            protocolBox.Text = string.Empty;
            deviceNameBox.Text = string.Empty;
            btAddressBox.Text = string.Empty;
            btSignalStrengthBox.Text = string.Empty;
            freeUserFlashBox.Text = string.Empty;
            batteryLevelBox.Text = string.Empty;

            tachoCountBox.Text = string.Empty;
            blockTachoCountBox.Text = string.Empty;
            rotationCountBox.Text = string.Empty;

            validCheck.IsChecked = false;
            calibratedCheck.IsChecked = false;
            sensorTypeBox.Text = string.Empty;
            sensorModeBox.Text = string.Empty;
            rawInputBox.Text = string.Empty;
            normalizedInputBox.Text = string.Empty;
            scaledInputBox.Text = string.Empty;
            calibratedInputBox.Text = string.Empty;

            // disable controls
            resetMotorButton.IsEnabled = false;
            setMotorStateButton.IsEnabled = false;
            getMotorStateButton.IsEnabled = false;
            getInputButton.IsEnabled = false;
            setInputModeButton.IsEnabled = false;

            connectButton.IsEnabled    = true;
            disconnectButton.IsEnabled = false;

        }
        #endregion

        #region Motor Control Related Methods
        // Returns selected motor
        private NXTBrick.Motor GetSelectedMotor()
        {
            return (NXTBrick.Motor)motorCombo.SelectedIndex;
        }


        private void resetMotorButton_Click(object sender, RoutedEventArgs e)
        {
            if (nxt.ResetMotorPosition(GetSelectedMotor(), false, false) != true)
            {
                System.Diagnostics.Debug.WriteLine("Failed reseting motor");
            }
        }

        private void setMotorStateButton_Click(object sender, RoutedEventArgs e)
        {
            NXTBrick.MotorState motorState = new NXTBrick.MotorState( );

            // prepare motor's state to set
            motorState.Power = Convert.ToByte(powerUpDown.Text);
            motorState.TurnRatio = 0;
            motorState.Mode = ( ( modeOnCheck.IsChecked == true) ? NXTBrick.MotorMode.On : NXTBrick.MotorMode.None ) |
                ((modeBrakeCheck.IsChecked == true) ? NXTBrick.MotorMode.Brake : NXTBrick.MotorMode.None) |
                ((modeRegulatedBox.IsChecked == true) ? NXTBrick.MotorMode.Regulated : NXTBrick.MotorMode.None);
            motorState.Regulation = regulationModes[regulationModeCombo.SelectedIndex];
            motorState.RunState = runStates[runStateCombo.SelectedIndex];
            // tacho limit
            try
            {
                motorState.TachoLimit = Math.Max( 0, Math.Min( 100000, int.Parse( tachoLimitBox.Text ) ) );
            }
            catch
            {
                motorState.TachoLimit = 1000;
                tachoLimitBox.Text = motorState.TachoLimit.ToString( );
            }

            // set motor's state
            if ( nxt.SetMotorState( GetSelectedMotor( ), motorState, false ) != true )
            {
                System.Diagnostics.Debug.WriteLine( "Failed setting motor state" );
            }
        }

        private void getMotorStateButton_Click(object sender, RoutedEventArgs e)
        {
            NXTBrick.MotorState motorState;

            // get motor's state
            if (nxt.GetMotorState(GetSelectedMotor(), out motorState))
            {
                tachoCountBox.Text = motorState.TachoCount.ToString();
                blockTachoCountBox.Text = motorState.BlockTachoCount.ToString();
                rotationCountBox.Text = motorState.RotationCount.ToString();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed getting motor state");
            }
        }

        #endregion

        #region Sensor related Methods
        private void getInputButton_Click(object sender, RoutedEventArgs e)
        {
            NXTBrick.SensorValues sensorValues;

            // get input values
            if (nxt.GetSensorValue(GetSelectedSensor(), out sensorValues))
            {
                validCheck.IsChecked = sensorValues.IsValid;
                calibratedCheck.IsChecked = sensorValues.IsCalibrated;
                sensorTypeBox.Text = sensorValues.SensorType.ToString();
                sensorModeBox.Text = sensorValues.SensorMode.ToString();
                rawInputBox.Text = sensorValues.Raw.ToString();
                normalizedInputBox.Text = sensorValues.Normalized.ToString();
                scaledInputBox.Text = sensorValues.Scaled.ToString();
                calibratedInputBox.Text = sensorValues.Calibrated.ToString();
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed getting input values");
            }
        }

        // Returns selected input port
        private NXTBrick.Sensor GetSelectedSensor()
        {
            return (NXTBrick.Sensor)inputPortCombo.SelectedIndex;
        }

        private void setInputModeButton_Click(object sender, RoutedEventArgs e)
        {
            if (nxt.SetSensorMode(GetSelectedSensor(),
               sensorTypes[sensorTypeCombo.SelectedIndex],
               sensorModes[sensorModeCombo.SelectedIndex], false) != true)
            {
                System.Diagnostics.Debug.WriteLine("Failed setting input mode");
            }
        }

        #endregion

        #region ThreeD Geometry Related Methods

        private void shownGeometryBtn_Click(object sender, RoutedEventArgs e)
        {
            GeometryWindow dialogWindow = new GeometryWindow();
            dialogWindow.Owner = this;
            dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            dialogWindow.ShowDialog();

        }


        #endregion


        #region 3D Cuting methods

        // Returns selected motor
        private NXTBrick.Motor GetSelectedMotorZ()
        {
            return (NXTBrick.Motor)1;
        }

        // Returns selected motor
        private NXTBrick.Motor GetSelectedMotorY()
        {
            return (NXTBrick.Motor)0;
        }

        // Returns selected motor
        private NXTBrick.Motor GetSelectedMotorX()
        {
            return (NXTBrick.Motor)2;
        }

        private NXTBrick.Motor GetThreeDSelectedMotor(int axis)
        {
            return (NXTBrick.Motor)axis;
        }

        private NXTBrick.MotorState threeDMotorState;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="axis"> 0: Y axis (cutting motor), 1: Z axis, 2: X axis</param>
        /// <param name="displacement"></param>
        private bool RunMotorInAnAxis(int axis, int displacement)
        {
            if (displacement != 0)
            {
                if (displacement < 0)
                {
                    threeDMotorState.Power = -1 * Convert.ToByte(powerUpDown.Text);
                }
                else
                {
                    threeDMotorState.Power = Convert.ToByte(powerUpDown.Text);
                }

                try
                {
                    threeDMotorState.TachoLimit = Math.Abs(displacement);
                }
                catch
                {
                    threeDMotorState.TachoLimit = 1000;
                    tachoLimitBox.Text = threeDMotorState.TachoLimit.ToString();
                }

                // set motor's state
                if (nxt.SetMotorState(GetThreeDSelectedMotor(axis), threeDMotorState, false) != true)
                {
                    System.Diagnostics.Debug.WriteLine("Failed setting motor state");
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// StartCutting: Main Cutting Method. Before calling this method, be sure that the 3d machine is ready.
        /// </summary>
        /// <param name="calibrationOnly"></param>
        private void StartCutting(bool calibrationOnly)
        {
             MillingMachineManager myMillingMachineManager = new MillingMachineManager(AppDomain.CurrentDomain.BaseDirectory + @"\head-vise.obj");

             threeDMotorState = new NXTBrick.MotorState();
            

            // prepare motor's state to set
             threeDMotorState.Power = Convert.ToByte(powerUpDown.Text);
             threeDMotorState.TurnRatio = 0;
             threeDMotorState.Mode = ((modeOnCheck.IsChecked == true) ? NXTBrick.MotorMode.On : NXTBrick.MotorMode.None) |
                 ((modeBrakeCheck.IsChecked == true) ? NXTBrick.MotorMode.Brake : NXTBrick.MotorMode.None) |
                 ((modeRegulatedBox.IsChecked == true) ? NXTBrick.MotorMode.Regulated : NXTBrick.MotorMode.None);
             threeDMotorState.Regulation = regulationModes[regulationModeCombo.SelectedIndex];
             threeDMotorState.RunState = runStates[2];

            // Prepare sensor state
            if (nxt.SetSensorMode(GetSelectedSensor(),
              sensorTypes[BLUE_COLOR_SENSOR_STATE_INDEX],
              sensorModes[0], false) != true)
            {
                System.Diagnostics.Debug.WriteLine("Failed setting input mode");
            }

            List<Displacement> displacementList;

            if (calibrationOnly)
            {
                displacementList = myMillingMachineManager.GetVertexCalibrationListForMachineCoordinatesAsDisplacement();
            }
            else
            {
                displacementList = myMillingMachineManager.GetVertexListForMachineCoordinatesAsDisplacement();
            }


           
            foreach (Displacement v in displacementList)
            {
                // Run X axis motor

                int xDisplacement = Convert.ToInt32(v.x);

                if (RunMotorInAnAxis(2, xDisplacement))
                {
                    Thread.Sleep(Math.Abs(xDisplacement * 100));
                }

         
                //displayTxt.Text += xDisplacement + ",";

                // Z

                int zDisplacement = Convert.ToInt32(v.z);

                if (RunMotorInAnAxis(1, zDisplacement))
                {
                    Thread.Sleep(Math.Abs(zDisplacement * 100));
                }


                // This is the main rotating/cutting motor. We use actual vertex information rather than displacement information and 
                // use color detector for returning back

                int yPosition;
                if (calibrationOnly)
                {
                    yPosition = Convert.ToInt32(v.y);
                }
                else
                {
                    yPosition = Convert.ToInt32(v.TargetVertex.y);
                }
                

                if (RunMotorInAnAxis(0, yPosition))
                {
                    Thread.Sleep(Math.Abs(yPosition * 10));

                    
                    NXTBrick.SensorValues sensorValues;

                    do
                    {
                        // get input values
                        if (nxt.GetSensorValue(GetSelectedSensor(), out sensorValues))
                        {
                            validCheck.IsChecked = sensorValues.IsValid;
                            calibratedCheck.IsChecked = sensorValues.IsCalibrated;
                            sensorTypeBox.Text = sensorValues.SensorType.ToString();
                            sensorModeBox.Text = sensorValues.SensorMode.ToString();
                            rawInputBox.Text = sensorValues.Raw.ToString();
                            normalizedInputBox.Text = sensorValues.Normalized.ToString();
                            scaledInputBox.Text = sensorValues.Scaled.ToString();
                            calibratedInputBox.Text = sensorValues.Calibrated.ToString();
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed getting input values");
                        }


                        // Now go up until blue color is reached
                        threeDMotorState.Power = -1 * Convert.ToByte(powerUpDown.Text);
                        threeDMotorState.TachoLimit = 5;

                        // set motor's state
                        if (nxt.SetMotorState(GetSelectedMotorY(), threeDMotorState, false) != true)
                        {
                            System.Diagnostics.Debug.WriteLine("Failed setting motor state");
                        }

                        Thread.Sleep(Math.Abs(50));

                    } while (sensorValues.Raw < 250);
                   

                }

            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            StartCutting(false);
        }


        private void calibrateBtn_Click(object sender, RoutedEventArgs e)
        {
            StartCutting(true);
        }

        #endregion

        private void powerUpDown_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
