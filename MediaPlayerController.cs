﻿//* "This work uses content from the Sansar Knowledge Base. © 2017 Linden Research, Inc. Licensed under the Creative Commons Attribution 4.0 International License (license summary available at https://creativecommons.org/licenses/by/4.0/ and complete license terms available at https://creativecommons.org/licenses/by/4.0/legalcode)."

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using Sansar;
using Sansar.Script;
using Sansar.Simulation;

// This script is attached to a 3d model.  It has configuration paramters to identify control surfaces on the 3d model.  
// A control surface is a portion of the model that when left mouse clicked on in desktop mode of touched with your hand and 
// trigger in VR mode by the user it will send a simple message and a Reflex Bang that sends the message associated with the 
// control surface.  A good example of this is that the 3d model is a drum set.  Each drum and cymbal you define a Control Surface
// for.  Control Surfaces are Circles.  The Control Surfaces are configured using the followinng structure"
// EventName, XcenterofControlSurface, YcenterofConntrolSurface, RadiusOfControlSurface, Zminimum, Zmaximum
// SnareDrumHit, -12, 35, 25, 0, 200
// The units above are in centimeters and they are all relative from the center of the model.  So, this means that the control surface
// defined says that if the user hits on the model in an area that is within a circle located with an origin of X=-12cm, Y=35cm with a
// radius of 25cm and anywhere from 0 to 200 cm on the Z axis a Simple Message Event named SnareDrumHit will be sent.
//
// This means that any Simple Script Effector could be listening for the Event SnareDrumHit and then execute things like turning on
// a light, moving an object, generating a sound, etc.


public class MediaPlayerController : SceneObjectScript

{
    #region ConstantsVariables
    [DefaultValue("Click Me!")]
    public readonly Interaction ComplexInteraction;

    private float[] ControlSurfaceAXRelative = new float[30];
    private float[] ControlSurfaceAYRelative = new float[30];
    private float[] ControlSurfaceBXRelative = new float[30];
    private float[] ControlSurfaceBYRelative = new float[30];
    private float[] ControlSurfaceCXRelative = new float[30];
    private float[] ControlSurfaceCYRelative = new float[30];
    private float[] ControlSurfaceDXRelative = new float[30];
    private float[] ControlSurfaceDYRelative = new float[30];
    private float[] ControlSurfaceZMinimum = new float[30];
    private float[] ControlSurfaceZMaximum = new float[30];
    private float[] ControlSurfaceAXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceAYRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceBXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceBYRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceCXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceCYRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceDXRelativeAfterRotation = new float[30];
    private float[] ControlSurfaceDYRelativeAfterRotation = new float[30];

    private string[] ControlSurfaceMessage = new string[30];
    private AgentPrivate Hitman;
    private RigidBodyComponent RigidBody = null;
    private Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    private static readonly Vector WarehouseRot = new Vector(0.0f, 0.0f, 0.0f);
    Quaternion RotQuat = Quaternion.FromEulerAngles(WarehouseRot).Normalized();
    private double ZRotation = new double();
    private int NumOfControlSurfaces = 0;

    //public Vector CurPos = new Vector(0.0f, 0.0f, 0.0f);
    //public double ZRotation = new double();
    public float OffTimer = 0;
    public bool Debug = false;

    private string ControlSurface1 = "SendPlayShuffle,-270.3,-2.5,-237.3,-2.5,-237.3,2.5,-270.3,2.5,26.8,59.8";
    private string ControlSurface2 = "SendPlayList,-236.6,-2.5,-203.6,-2.5,-203.6,2.5,-236.6,2.5,26.8,59.8";
    private string ControlSurface3 = "SendStop,-176.5,-2.5,-143.5,-2.5,-143.5,2.5,-176.5,2.5,26.8,59.8";
    private string ControlSurface4 = "SendPrevious,-143,-2.5,-110,-2.5,-110,2.5,-143,2.5,26.8,59.8";
    private string ControlSurface5 = "SendPause,-108.5,-2.5,-75.5,-2.5,-75.5,2.5,-108.5,2.5,26.8,59.8";
    private string ControlSurface6 = "SendResume,-74.8,-2.5,-41.8,-2.5,-41.8,2.5,-74.8,2.5,26.8,59.8";
    private string ControlSurface7 = "SendNext,-40.8,-2.5,-7.8,-2.5,-7.8,2.5,-40.8,2.5,26.8,59.8";
    private string ControlSurface8 = "SendForceVideo,20.2,-2.5,86.2,-2.5,86.2,2.5,20.2,2.5,26.8,59.8";
    private string ControlSurface9 = "SendVolumeDown,111.3,-2.5,144.3,-2.5,144.3,2.5,111.3,2.5,26.8,59.8";
    private string ControlSurface10 = "SendVolumeUp,178.5,-2.5,211.5,-2.5,211.5,2.5,178.5,2.5,26.8,59.8";
    private string ControlSurface11 = "SendEject,237.5,-2.5,270.5,-2.5,270.5,2.5,237.5,2.5,26.8,59.8";
    private string ControlSurface12 = "play1,-268.8,-2.5,-235.8,-2.5,-235.8,2.5,-268.8,2.5,79.9,112.9";
    private string ControlSurface13 = "play2,-235.3,-2.5,-202.3,-2.5,-202.3,2.5,-235.3,2.5,79.9,112.9";
    private string ControlSurface14 = "play3,-201.5,-2.5,-168.5,-2.5,-168.5,2.5,-201.5,2.5,79.9,112.9";
    private string ControlSurface15 = "play4,-167.1,-2.5,-134.1,-2.5,-134.1,2.5,-167.1,2.5,79.9,112.9";
    private string ControlSurface16 = "play5,-133.5,-2.5,-100.5,-2.5,-100.5,2.5,-133.5,2.5,79.9,112.9";
    private string ControlSurface17 = "play6,-83.2,-2.5,-50.2,-2.5,-50.2,2.5,-83.2,2.5,79.9,112.9";
    private string ControlSurface18 = "play7,-49.7,-2.5,-16.7,-2.5,-16.7,2.5,-49.7,2.5,79.9,112.9";
    private string ControlSurface19 = "play8,-32,-2.5,34,-2.5,34,2.5,-32,2.5,79.9,112.9";
    private string ControlSurface20 = "play9,18.3,-2.5,51.3,-2.5,51.3,2.5,18.3,2.5,79.9,112.9";
    private string ControlSurface21 = "play10,51.9,-2.5,84.9,-2.5,84.9,2.5,51.9,2.5,79.9,112.9";
    private string ControlSurface22 = "play11,102.3,-2.5,135.3,-2.5,135.3,2.5,102.3,2.5,79.9,112.9";
    private string ControlSurface23 = "play12,136,-2.5,169,-2.5,169,2.5,136,2.5,79.9,112.9";
    private string ControlSurface24 = "play13,169.9,-2.5,202.9,-2.5,202.9,2.5,169.9,2.5,79.9,112.9";
    private string ControlSurface25 = "play14,203.8,-2.5,236.8,-2.5,236.8,2.5,203.8,2.5,79.9,112.9";
    private string ControlSurface26 = "play15,237.4,-2.5,270.4,-2.5,270.4,2.5,237.4,2.5,79.9,112.9";
    private string ControlSurface27 = "";
    private string ControlSurface28 = "";
    private string ControlSurface29 = "";
    private string ControlSurface30 = "";
    #endregion

    public override void Init()
    {
        Script.UnhandledException += UnhandledException; // Catch errors and keep running unless fatal

        if (RigidBody == null)
        {
            if (!ObjectPrivate.TryGetFirstComponent(out RigidBody))
            {
                // Since object scripts are initialized when the scene loads, no one will actually see this message.
                ScenePrivate.Chat.MessageAllUsers("There is no RigidBodyComponent attached to this object.");
                return;
            }
        }

        CurPos = RigidBody.GetPosition();
        Log.Write("CurPos: " + CurPos);
        ZRotation = RigidBody.GetOrientation().GetEulerAngles().Z* 57.2958;
        Log.Write("Zrotation: " + ZRotation);

        int cntr = 0;

        if (ControlSurface1.Length > 0)
        {
            LoadControlSurfaces(ControlSurface1, cntr);
            cntr++;
        }
        if (ControlSurface2.Length > 0)
        {
            LoadControlSurfaces(ControlSurface2, cntr);
            cntr++;
        }
        if (ControlSurface3.Length > 0)
        {
            LoadControlSurfaces(ControlSurface3, cntr);
            cntr++;
        }
        if (ControlSurface4.Length > 0)
        {
            LoadControlSurfaces(ControlSurface4, cntr);
            cntr++;
        }
        if (ControlSurface5.Length > 0)
        {
            LoadControlSurfaces(ControlSurface5, cntr);
            cntr++;
        }
        if (ControlSurface6.Length > 0)
        {
            LoadControlSurfaces(ControlSurface6, cntr);
            cntr++;
        }
        if (ControlSurface7.Length > 0)
        {
            LoadControlSurfaces(ControlSurface7, cntr);
            cntr++;
        }
        if (ControlSurface8.Length > 0)
        {
            LoadControlSurfaces(ControlSurface8, cntr);
            cntr++;
        }
        if (ControlSurface9.Length > 0)
        {
            LoadControlSurfaces(ControlSurface9, cntr);
            cntr++;
        }
        if (ControlSurface10.Length > 0)
        {
            LoadControlSurfaces(ControlSurface10, cntr);
            cntr++;
        }
        if (ControlSurface11.Length > 0)
        {
            LoadControlSurfaces(ControlSurface11, cntr);
            cntr++;
        }
        if (ControlSurface12.Length > 0)
        {
            LoadControlSurfaces(ControlSurface12, cntr);
            cntr++;
        }
        if (ControlSurface13.Length > 0)
        {
            LoadControlSurfaces(ControlSurface13, cntr);
            cntr++;
        }
        if (ControlSurface14.Length > 0)
        {
            LoadControlSurfaces(ControlSurface14, cntr);
            cntr++;
        }
        if (ControlSurface15.Length > 0)
        {
            LoadControlSurfaces(ControlSurface15, cntr);
            cntr++;
        }
        if (ControlSurface16.Length > 0)
        {
            LoadControlSurfaces(ControlSurface16, cntr);
            cntr++;
        }
        if (ControlSurface17.Length > 0)
        {
            LoadControlSurfaces(ControlSurface17, cntr);
            cntr++;
        }
        if (ControlSurface18.Length > 0)
        {
            LoadControlSurfaces(ControlSurface18, cntr);
            cntr++;
        }
        if (ControlSurface19.Length > 0)
        {
            LoadControlSurfaces(ControlSurface19, cntr);
            cntr++;
        }
        if (ControlSurface20.Length > 0)
        {
            LoadControlSurfaces(ControlSurface20, cntr);
            cntr++;
        }
        if (ControlSurface21.Length > 0)
        {
            LoadControlSurfaces(ControlSurface21, cntr);
            cntr++;
        }
        if (ControlSurface22.Length > 0)
        {
            LoadControlSurfaces(ControlSurface22, cntr);
            cntr++;
        }
        if (ControlSurface23.Length > 0)
        {
            LoadControlSurfaces(ControlSurface23, cntr);
            cntr++;
        }
        if (ControlSurface24.Length > 0)
        {
            LoadControlSurfaces(ControlSurface24, cntr);
            cntr++;
        }
        if (ControlSurface25.Length > 0)
        {
            LoadControlSurfaces(ControlSurface25, cntr);
            cntr++;
        }
        if (ControlSurface26.Length > 0)
        {
            LoadControlSurfaces(ControlSurface26, cntr);
            cntr++;
        }
        if (ControlSurface27.Length > 0)
        {
            LoadControlSurfaces(ControlSurface27, cntr);
            cntr++;
        }
        if (ControlSurface28.Length > 0)
        {
            LoadControlSurfaces(ControlSurface28, cntr);
            cntr++;
        }
        if (ControlSurface29.Length > 0)
        {
            LoadControlSurfaces(ControlSurface29, cntr);
            cntr++;
        }
        if (ControlSurface30.Length > 0)
        {
            LoadControlSurfaces(ControlSurface30, cntr);
            cntr++;
        }
        NumOfControlSurfaces = cntr;
        ComplexInteractionHandler();
    }

    private void UnhandledException(object Sender, Exception Ex)
    {
        Log.Write(LogLevel.Error, GetType().Name, Ex.Message + "\n" + Ex.StackTrace + "\n" + Ex.Source);
        return;
    }//UnhandledException

    #region Communication

    public interface ISimpleData
    {
        AgentInfo AgentInfo { get; }
        ObjectId ObjectId { get; }
    }

    public class SimpleData : Reflective, ISimpleData
    {
        public AgentInfo AgentInfo { get; set; }
        public ObjectId ObjectId { get; set; }
    }

    public interface IDebugger { bool DebugSimple { get; } }
    private bool __debugInitialized = false;
    private bool __SimpleDebugging = false;
    private string __SimpleTag = "";
    private void SetupSimple()
    {
        __debugInitialized = true;
        __SimpleTag = GetType().Name + " [S:" + Script.ID.ToString() + " O:" + ObjectPrivate.ObjectId.ToString() + "]";
        Wait(TimeSpan.FromSeconds(1));
        IDebugger debugger = ScenePrivate.FindReflective<IDebugger>("SimpleDebugger").FirstOrDefault();
        if (debugger != null) __SimpleDebugging = debugger.DebugSimple;
    }

    private Action SubscribeToAll(string csv, Action<ScriptEventData> callback)
    {
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return null;
        Action unsubscribes = null;
        string[] events = csv.Trim().Split(',');
        if (__SimpleDebugging)
        {
            Log.Write(LogLevel.Info, __SimpleTag, "Subscribing to " + events.Length + " events: " + string.Join(", ", events));
        }
        foreach (string eventName in events)
        {
            if (__SimpleDebugging)
            {
                var sub = SubscribeToScriptEvent(eventName.Trim(), (ScriptEventData data) =>
                {
                    Log.Write(LogLevel.Info, __SimpleTag, "Received event " + eventName);
                    callback(data);
                });
                unsubscribes += sub.Unsubscribe;
            }
            else
            {
                var sub = SubscribeToScriptEvent(eventName.Trim(), callback);
                unsubscribes += sub.Unsubscribe;
            }

        }
        return unsubscribes;
    }

    private void SendToAll(string csv, Reflective data)
    {
        //Log.Write("In SendToAll");
        if (!__debugInitialized) SetupSimple();
        if (string.IsNullOrWhiteSpace(csv)) return;
        string[] events = csv.Trim().Split(',');

        if (__SimpleDebugging) Log.Write(LogLevel.Info, __SimpleTag, "Sending " + events.Length + " events: " + string.Join(", ", events));
        foreach (string eventName in events)
        {
            //Log.Write("EventName: " + eventName);
            PostScriptEvent(eventName.Trim(), data);
        }
    }

    #endregion

    #region Interaction

    private void LoadControlSurfaces(string ControlSurfaceInputString, int cntr)
    {
        //Takes Relative Values read in from configuration and converts them to realworld position 
        string[] values = new string[100];
        //Log.Write("sendSamples.SendSampleLibrary.Count(): " + sendSamples.SendSampleLibrary.Count());
        //Log.Write("sendNotePositions.SendNotePosition.Count(): " + sendNotePositions.SendNotePosition.Count());

        //Log.Write("ZRotation: " + ZRotation);
        //Log.Write("cntr: " + cntr);
        values = ControlSurfaceInputString.Split(',');
        ControlSurfaceMessage[cntr] = values[0];
        ControlSurfaceAXRelative[cntr] = float.Parse(values[1]);
        ControlSurfaceAYRelative[cntr] = float.Parse(values[2]);
        ControlSurfaceBXRelative[cntr] = float.Parse(values[3]);
        ControlSurfaceBYRelative[cntr] = float.Parse(values[4]);
        ControlSurfaceCXRelative[cntr] = float.Parse(values[5]);
        ControlSurfaceCYRelative[cntr] = float.Parse(values[6]);
        ControlSurfaceDXRelative[cntr] = float.Parse(values[7]);
        ControlSurfaceDYRelative[cntr] = float.Parse(values[8]);
        ControlSurfaceZMinimum[cntr] = float.Parse(values[9]);
        //Log.Write("ControlSurfaceZMinimum[" + cntr + "]: " + ControlSurfaceZMinimum[cntr]);
        ControlSurfaceZMaximum[cntr] = float.Parse(values[10]);
        //Log.Write("ControlSurfaceZMaximum[" + cntr + "]: " + ControlSurfaceZMaximum[cntr]);

        float CosAngle = (float)Math.Cos(ZRotation * 0.0174533);
        float SinAngle = (float)Math.Sin(ZRotation * 0.0174533);

        ControlSurfaceAXRelativeAfterRotation[cntr] = (ControlSurfaceAXRelative[cntr] * CosAngle) - (ControlSurfaceAYRelative[cntr] * SinAngle);
        ControlSurfaceAYRelativeAfterRotation[cntr] = (ControlSurfaceAYRelative[cntr] * CosAngle) + (ControlSurfaceAXRelative[cntr] * SinAngle);
        ControlSurfaceBXRelativeAfterRotation[cntr] = (ControlSurfaceBXRelative[cntr] * CosAngle) - (ControlSurfaceBYRelative[cntr] * SinAngle);
        ControlSurfaceBYRelativeAfterRotation[cntr] = (ControlSurfaceBYRelative[cntr] * CosAngle) + (ControlSurfaceBXRelative[cntr] * SinAngle);
        ControlSurfaceCXRelativeAfterRotation[cntr] = (ControlSurfaceCXRelative[cntr] * CosAngle) - (ControlSurfaceCYRelative[cntr] * SinAngle);
        ControlSurfaceCYRelativeAfterRotation[cntr] = (ControlSurfaceCYRelative[cntr] * CosAngle) + (ControlSurfaceCXRelative[cntr] * SinAngle);
        ControlSurfaceDXRelativeAfterRotation[cntr] = (ControlSurfaceDXRelative[cntr] * CosAngle) - (ControlSurfaceDYRelative[cntr] * SinAngle);
        ControlSurfaceDYRelativeAfterRotation[cntr] = (ControlSurfaceDYRelative[cntr] * CosAngle) + (ControlSurfaceDXRelative[cntr] * SinAngle);

    }

    float Sign(float p1x, float p1y, float p2x, float p2y, float p3x, float p3y)
    {
        return (p1x - p3x) * (p2y - p3y) - (p2x - p3x) * (p1y - p3y);
    }

    bool IsPointInTri(float ptX, float ptY, float v1X, float v1Y, float v2X, float v2Y, float v3X, float v3Y)
    {
        bool b1, b2, b3;

        b1 = Sign(ptX, ptY, v1X, v1Y, v2X, v2Y) < 0.0f;
        //float b1float = Sign(ptX, ptY, v1X, v1Y, v2X, v2Y);
        //Log.Write("b1float: " + b1float);
        //Log.Write("b1: " + b1);
        b2 = Sign(ptX, ptY, v2X, v2Y, v3X, v3Y) < 0.0f;
        //float b2float = Sign(ptX, ptY, v2X, v2Y, v3X, v3Y);
        //Log.Write("b2float: " + b2float);
        //Log.Write("b2: " + b2);
        b3 = Sign(ptX, ptY, v3X, v3Y, v1X, v1Y) < 0.0f;
        //float b3float = Sign(ptX, ptY, v3X, v3Y, v1X, v1Y);
        //Log.Write("b3float: " + b3float);
        //Log.Write("b3: " + b3);

        return ((b1 == b2) && (b2 == b3));
    }

    bool PointInRectangle(float ptX, float ptY, float AX, float AY, float BX, float BY, float CX, float CY, float DX, float DY)
    {
        //bool test1 = IsPointInTri(ptX, ptY, AX, AY, BX, BY, CX, CY);
        //bool test2 = IsPointInTri(ptX, ptY, AX, AY, CX, CY, DX, DY);
        //Log.Write("Test1: " + test1);
        //Log.Write("Test2: " + test2);
        if (IsPointInTri(ptX, ptY, AX, AY, BX, BY, CX, CY)) return true;   //(X, Y, Z, P)) return true;
        if (IsPointInTri(ptX, ptY, AX, AY, CX, CY, DX, DY)) return true;
        //if (PointInTriangle(X, Z, W, P)) return true;
        return false;
    }

    private void ComplexInteractionHandler()
    {
        ComplexInteraction.Subscribe((InteractionData idata) =>
        {
            if (Debug)
            {
                ComplexInteraction.SetPrompt("Debug: " 
                    + "\nHit:" + idata.HitPosition.ToString()
                    + "\nFrom:" + idata.Origin.ToString()
                    + "\nNormal:" + idata.HitNormal.ToString()
                    + "\nBy:" + ScenePrivate.FindAgent(idata.AgentId).AgentInfo.Name);
                //Vector hitPosition = idata.HitPosition;
                Log.Write("Hit:  " + idata.HitPosition.ToString());
            }
            ExecuteInteraction(idata);
            //Log.Write("idata.HitPosition.ToString()" + idata.HitPosition.ToString());
        });
    }

    private void ExecuteInteraction(InteractionData idata)
    {
        //loopNote = false;
        float hitXRelative = 0;
        float hitYRelative = 0;
        float hitZRelative = 0;
        Vector hitPosition = idata.HitPosition;
        //Log.Write("CurPosX: " + CurPos.X);
        //Log.Write("CurPosY: " + CurPos.Y);
        //Log.Write("hitPosition.X: " + hitPosition.X);
        //Log.Write("hitPosition.Y: " + hitPosition.Y);
        //normalize to origin 0,0

        if (hitPosition.X > CurPos.X)
        {
            hitXRelative = (hitPosition.X - CurPos.X) * 100;
        }
        else
        {
            hitXRelative = (hitPosition.X - CurPos.X) * 100;
        }

        if (hitPosition.Y > CurPos.Y)
        {
            hitYRelative = (hitPosition.Y - CurPos.Y) * 100;
        }
        else
        {
            hitYRelative = (hitPosition.Y - CurPos.Y) * 100;
        }
        if (hitPosition.Z > CurPos.Z)
        {
            hitZRelative = (hitPosition.Z - CurPos.Z) * 100;
        }
        else
        {
            hitZRelative = (hitPosition.Z - CurPos.Z) * 100;
        }
        Log.Write("hitXRelative: " + hitXRelative);
        Log.Write("hitYRelative: " + hitYRelative);
        Log.Write("hitZRelative: " + hitZRelative);
        int cntr = 0;
        do
        {
            //Log.Write("AX: " + ControlSurfaceAXRelativeAfterRotation[cntr]);
            //Log.Write("AY: " + ControlSurfaceAYRelativeAfterRotation[cntr]);
            //Log.Write("BX: " + ControlSurfaceBXRelativeAfterRotation[cntr]);
            //Log.Write("BY: " + ControlSurfaceBYRelativeAfterRotation[cntr]);
            //Log.Write("CX: " + ControlSurfaceCXRelativeAfterRotation[cntr]);
            //Log.Write("CY: " + ControlSurfaceCYRelativeAfterRotation[cntr]);
            //Log.Write("DX: " + ControlSurfaceDXRelativeAfterRotation[cntr]);
            //Log.Write("DY: " + ControlSurfaceDYRelativeAfterRotation[cntr]);
            bool pointInRectangle = PointInRectangle(hitXRelative, hitYRelative,
                ControlSurfaceAXRelativeAfterRotation[cntr], ControlSurfaceAYRelativeAfterRotation[cntr],
                ControlSurfaceBXRelativeAfterRotation[cntr], ControlSurfaceBYRelativeAfterRotation[cntr],
                ControlSurfaceCXRelativeAfterRotation[cntr], ControlSurfaceCYRelativeAfterRotation[cntr],
                ControlSurfaceDXRelativeAfterRotation[cntr], ControlSurfaceDYRelativeAfterRotation[cntr]);
            if (pointInRectangle)
            {
                if (hitZRelative >= ControlSurfaceZMinimum[cntr] && hitZRelative <= ControlSurfaceZMaximum[cntr])
                {
                    //Simple Message
                    string hitControlSurface = ControlSurfaceMessage[cntr];
                    Log.Write("Hit Control Surface: " + hitControlSurface);
                    sendSimpleMessage(ControlSurfaceMessage[cntr], idata);
                    break;
                }
            }
            cntr++;
        } while (cntr<NumOfControlSurfaces);
    }

    private void sendSimpleMessage(string msg, InteractionData data)
    {
        SimpleData sd = new SimpleData();
        sd.AgentInfo = ScenePrivate.FindAgent(data.AgentId)?.AgentInfo;
        sd.ObjectId = sd.AgentInfo.ObjectId;
        SendToAll(msg, sd);

        if (OffTimer > 0.0)
        {
            Wait(TimeSpan.FromMilliseconds((int)OffTimer * 1000));
            SendToAll(msg + "Off", sd);
        }
    }

    #endregion
}
